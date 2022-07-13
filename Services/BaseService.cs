using Microsoft.AspNetCore.Components.Authorization;
using WebsiteBlazor.Models;
using WebsiteBlazor.Abstract;

namespace WebsiteBlazor.Services
{
    public class BaseService
    {
        protected AppConfig _appConfig;
        protected AuthenticationStateProvider _authProvider;
        protected ICacheService _redis;
        //protected UserService _userService;
        protected AuthenticationState _authenticationState;


        public BaseService(AuthenticationStateProvider auth, ICacheService redis, AppConfig appConfig)
        {
            _authProvider = auth;
            _redis = redis;
            _appConfig = appConfig;

            _ = GetAuthenticationState();
        }

        private async Task GetAuthenticationState()
        {
            _authenticationState = await _authProvider.GetAuthenticationStateAsync();
        }
    }
}