using AmaraCode.RainMaker.DataServiceWrapper;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using WebsiteBlazor.Models;
using WebsiteBlazor.Abstract;

namespace WebsiteBlazor.Services
{
    public class EngineService : BaseService
    {
        private readonly UserService _userService;
        private EngineWrapper _engineWrapper;

        public EngineService(AuthenticationStateProvider auth, ICacheService redis, AppConfig appConfig, UserService userService) : base(auth, redis, appConfig)
        {
            _userService = userService;
            Initialize().Wait();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetActiveTraders()
        {
            var result = await _engineWrapper.GetActiveTraders();
            return result ?? new List<string>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        /// <returns></returns>
        public void NotifyTraderToUpdateConfig(string traderName)
        {
            _engineWrapper.SendConfigChangeNotification(traderName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private async Task Initialize()
        {
            var userName = await _userService.GetCurrentUserName();
            _engineWrapper = new EngineWrapper(userName, _appConfig.DataSericeURL, _appConfig.RmApiKey);
        }
    }
}