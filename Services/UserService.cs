using AmaraCode.RainMaker.DataServiceWrapper.Abstract;
using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebsiteBlazor.Models;

namespace WebsiteBlazor.Services;

public class UserService : BaseService
{
    private ICompanyWrapper _companyWrapper;

    private IUserWrapper _userWrapper;

    /// <summary>
    ///
    /// </summary>
    /// <param name="auth"></param>
    /// <param name="redis"></param>
    /// <param name="appConfig"></param>
    public UserService(AuthenticationStateProvider auth, ICacheService redis, AppConfig appConfig) : base(auth, redis, appConfig)
    {
        Initialize().Wait();
    }

    /// <summary>
    /// Inserts a new user into Identity and RM system.
    /// </summary>
    /// <param name="user"></param>
    public async Task CreateUserAsync(UserDto user)
    {
        await _userWrapper.CreateUser(user);
    }

    /// <summary>
    /// Validation check to ensure User is still assigned to company.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="companyName"></param>
    /// <returns></returns>
    public async Task<bool> CurrentCompanyStillAssigned(string userName, string companyName)
    {
        var companies = await _companyWrapper.GetCompaniesByUser(userName);

        var result = companies.Where(c => c.Name == companyName).Select(x => x.Name).FirstOrDefault() == companyName;

        return result;
    }

    /// <summary>
    /// Returns Authenticated UserName
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetCurrentUserName()
    {
        try
        {
            var authenticationState = await _authProvider.GetAuthenticationStateAsync();
            if (authenticationState is not null)
            {
                return authenticationState.User?.Claims.Where(x => x.Type == "name").FirstOrDefault().Value;
            }
            else
            {
                return "Invalid";
            }
        }
        catch
        {
            return "Invalid";
            //maybe do something here in the future
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetCurrentEmailAddress()
    {
        try
        {
            var authenticationState = await _authProvider.GetAuthenticationStateAsync();
            if (authenticationState is not null)
            {
                return authenticationState.User?.Claims.Where(x => x.Type == "preferred_username").FirstOrDefault().Value;
            }
            else
            {
                return "Invalid";
            }
        }
        catch
        {
            return "Invalid";
            //maybe do something here in the future
        }
    }

    /// <summary>
    /// Reads User data from database
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<UserDto> GetUserByUserName(string userName)
    {
        var user = await _userWrapper.GetUserByUserName(userName);

        return user;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<UserDto> GetUserByEmailAddress(string emailAddress)
    {
        var user = await _userWrapper.GetUserByEmailAddress(emailAddress);

        return user;
    }

    /// <summary>
    /// Reads UserInfo from Redis.  If not there or invalid new data will be
    /// pulled from database.
    /// </summary>
    /// <returns></returns>
    public async Task<UserInfo> GetUserInfo()
    {
        //get the state to read the user
        //var authenticationState = await _authProvider.GetAuthenticationStateAsync();

        //if the user is not authenticate then just exit
        if (_authenticationState.User.Identity.IsAuthenticated == false)
        {
            return null;
        }

        //put the Identity user name in a variable for use in multiple places
        //var identityName = authenticationState.User?.Identity?.Name;
        var userName = _authenticationState.User?.Claims.Where(x => x.Type == "name").FirstOrDefault();
        var emailAddress = _authenticationState.User?.Claims.Where(x => x.Type == "preferred_username").FirstOrDefault();

        //first attempt to get the info from cache (redis)
        //if it is not there then build it and save it to cache

        var cacheUserInfo = await GetUserInfoFromCache(userName.Value);
        if (cacheUserInfo != null && UserInfoIsValid(cacheUserInfo))
        {
            //if we get here then we have valid cache data. Now we need
            //to check and see if the current company is still assigned to user
            if (await CurrentCompanyStillAssigned(cacheUserInfo.UserName, cacheUserInfo.CompanyName) == true)
            {
                return cacheUserInfo;
            }
        }

        //if we get here then the UserInfo is not in Cache so build it
        //and save it to cache

        //first create the UserInfo object and poulate it with what is in cache
        UserInfo model = new();
        model.UserName = userName.Value;
        model.EmailAddress = emailAddress.Value;

        //if any of the values are empty then get all values
        if (!UserInfoIsValid(model))
        {
            // get the defaults from DataService
            await PopulateDefaults(model);
        }

        //save the UserInfo json to redis
        await _redis.SetCacheValueAsync(model.UserName, model.ToJson());

        return model;

        //nested method
        bool UserInfoIsValid(UserInfo model)
        {
            bool anyInvalid = (model.CompanyID == Guid.Empty ||
                string.IsNullOrEmpty(model.CompanyName) ||
                model.CompanyBrokerID == Guid.Empty ||
                model.UserId == Guid.Empty ||
                string.IsNullOrEmpty(model.AccountNumber));

            return !anyInvalid;
        }
    }

    /// <summary>
    /// Read UserInfo data in Redis
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<UserInfo> GetUserInfoFromCache(string userName)
    {
        UserInfo userInfo = null;
        var json = await _redis.GetCacheValueAsync<UserInfo>(userName);

        return json;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserDto>> GetUsers()
    {
        return await _userWrapper.GetUsers();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public async Task<bool> IsAuthorizedUser(string emailAddress)
    {
        return await _userWrapper.IsAuthorizedUser(emailAddress);
    }

    /// <summary>
    ///Save UserInfo data to Redis
    /// </summary>
    /// <param name="userInfo"></param>
    public async Task SetUserInfoToCacheAsync(UserInfo userInfo)
    {
        await _redis.SetCacheValueAsync(userInfo.UserName, userInfo.ToJson());
    }

    /// <summary>
    ///Update user in RM and Identity
    /// </summary>
    /// <param name="user"></param>
    public async Task UpdateUser(UserDto user)
    {
        await _userWrapper.UpdateUser(user);
    }

    private async Task Initialize()
    {
        //get variables once for usage below
        var userName = await this.GetCurrentUserName();

        _userWrapper = new AmaraCode.RainMaker.DataServiceWrapper.UserWrapper(userName, _appConfig.DataSericeURL, _appConfig.RmApiKey);
        _companyWrapper = new AmaraCode.RainMaker.DataServiceWrapper.CompanyWrapper(userName, _appConfig.DataSericeURL, _appConfig.RmApiKey);
    }

    /// <summary>
    /// Populates the UserInfo class with the First Company and the First Broker
    /// </summary>
    /// <param name="userInfo"></param>
    private async Task PopulateDefaults(UserInfo userInfo)
    {
        if (userInfo.UserId == Guid.Empty)
        {
            var user = await GetUserByEmailAddress(userInfo.EmailAddress);
            if (user != null)
            {
                userInfo.UserId = user.UserId;
            }
        }

        //create instance of CompanyService
        CompanyService companyService = new(base._authProvider, base._redis, base._appConfig, this);
        //get first company user has access to
        //if not access then navigate to error
        var companies = await companyService.GetCompaniesByUserName(userInfo.UserName);
        if (companies.Count == 0)
        {
            //the user has no assigned companies. navigate to error
            throw new MissingAssignmentException($"{userInfo.UserName} is not assigned to any companies.");
        }
        //use the first company
        var company = companies[0];
        userInfo.CompanyID = company.CompanyID;
        userInfo.CompanyName = company.Name;
        //now get the first CompanyBroker
        var companyBrokers = await companyService.GetCompanyBrokers(company.Name);
        if (companyBrokers.Count == 0)
        {
            throw new MissingAssignmentException($"{company.Name} is not assigned to any CompanyBrokers.");
        }
        //if we get here then we have companyBrokers.  So we will use the first one
        var cb = companyBrokers[0];
        userInfo.CompanyBrokerID = cb.CompanyBrokerID;
        userInfo.BrokerID = cb.BrokerID;
        userInfo.BrokerName = cb.BrokerName;
        userInfo.AccountNumber = cb.AccountNumber;
    }

    /// <summary>
    /// Create Role in Identity
    ///
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<int> CreateRole(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
        { return 0; }

        //add the Role
        var result = await _userWrapper.CreateRole(roleName);
        return result;
    }

    /// <summary>
    /// Adds User-Role in Identity database.
    /// Note: Roles are "claims" stored in AspNetUserClaims
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="roleName"></param>
    /// <returns>Tuple (bool Succeeded, string Message)</returns>
    public async Task<int> CreateUserRole(string userName, string roleName)
    {
        var result = await _userWrapper.CreateUserRole(userName, roleName);
        return result;
    }

    /// <summary>
    /// Delete Role from Identity
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<int> DeleteRole(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
        { return 0; }

        return await _userWrapper.DeleteRole(roleName);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public async Task<int> CreateAuthorizedUser(string emailAddress)
    {
        return await _userWrapper.CreateAuthorizedUser(emailAddress);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public async Task<int> RemoveAuthorizedUser(string emailAddress)
    {
        return await _userWrapper.RemoveAuthorizedUser(emailAddress);
    }

    /// <summary>
    /// Returns Identity Role for given roleName
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<RoleDto> GetRole(string roleName)
    {
        return await _userWrapper.GetRole(roleName);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<List<RoleDto>> GetRoles()
    {
        return await _userWrapper.GetRoles();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<List<AuthorizedUserDto>> GetAuthorizedUsers()
    {
        var result = await _userWrapper.GetAuthorizedUsers();
        return result;
    }

    public async Task<List<UserRoleDto>> GetUserRoles(string userName)
    {
        //get all the claims for a user
        var results = await _userWrapper.GetUserRoles(userName);
        return results;
    }

    /// <summary>
    /// Removes User-Role in Identity database.
    /// Note: Roles are "claims" stored in AspNetUserClaims
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="roleName"></param>
    /// <returns>Tuple (bool Succeeded, string Message)</returns>
    public async Task<int> RemoveUserRole(string userName, string roleName)
    {
        return await _userWrapper.RemoveUserRole(userName, roleName);
    }

    /// <summary>
    /// Add/Remove roles for user matching the list of roleNames
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="roleNames"></param>
    /// <returns></returns>
    public async Task UpdateUserRoles(string userName, List<string> roleNames)
    {
        await _userWrapper.UpdateUserRoles(userName, roleNames);
    }
}