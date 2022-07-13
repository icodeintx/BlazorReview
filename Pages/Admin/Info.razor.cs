using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin
{
    /// <summary>
    ///
    /// </summary>
    public partial class Info : ComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        [CascadingParameter(Name = "UserInfo")]
        public UserInfo UserInfo { get; set; }


        [CascadingParameter(Name = "AuthState")]
        public AuthenticationState AuthState { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Inject]
        protected CompanyService CompanyService { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Inject]
        protected UserService UserService { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            //var x = UserInfo.BrokerName;
            //var y = AuthState.User.Identity.Name;
        }

        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected UserInfo GetRedisInfo(string userName)
        {
            var result = _cacheService.GetCacheValueAsync<UserInfo>(userName).Result;

            if (result is null)
            {
                return new UserInfo();
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected async Task SaveRedis()
        {
            await _cacheService.SetCacheValueAsync(UserInfo.UserName, UserInfo.ToJson());
        }
    }
}