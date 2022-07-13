using AmaraCode.RainMaker.DataServiceWrapper;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebsiteBlazor.Services;

public class SystemService : BaseService
{
    private SystemWrapper _systemWrapper;
    private UserService _userService;

    public SystemService(AuthenticationStateProvider auth, ICacheService redis, AppConfig appConfig, UserService userService) : base(auth, redis, appConfig)
    {
        _userService = userService;
        Initialize().Wait();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="companyBrokerID"></param>
    /// <returns></returns>
    public async Task UpdateInstrments(Guid companyBrokerID)
    {
        await _systemWrapper.UpdateSystemInstruments(companyBrokerID);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private async Task Initialize()
    {
        _systemWrapper = new SystemWrapper((await _userService.GetCurrentUserName()), _appConfig.DataSericeURL, _appConfig.RmApiKey);
    }
}