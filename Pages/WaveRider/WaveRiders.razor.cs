using AmaraCode.RainMaker.Models;
using AmaraCode.RainMaker.SignalrClient;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System.Collections.Generic;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.WaveRider
{
    public partial class WaveRiders : ComponentBase
    {
        public UserInfo userInfo;
        protected List<string> _activeTraders = new();
        protected List<WaveRiderSummaryDTO> WaveRiderList = new();
        private TraderHubClient _traderHub;
        private bool isLoading = true;

        [Inject]
        protected SignalrService _signalrService { get; set; }

        [Inject]
        protected AppConfig AppConfig { get; set; }

        /// <summary>
        /// Gets TraderState from Redis and validates if trade exists
        /// </summary>
        /// <param name="traderName"></param>
        /// <returns></returns>
        protected bool DoesTradeExistAsync(string traderName)
        {
            bool result = false;

            //create the correct key to get from Redis
            var keyName = $"TraderState_{traderName}";

            //if the key exist in Redis then continue
            var keyExist = RedisKeyExistsAsync(keyName);

            if (keyExist)
            {
                var state = Task.Run(() => _cacheService.GetCacheValueAsync<TraderState>(keyName));

                return state.Result.ActiveTrade;
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Retrieves list of active Traders from Signalr
        /// </summary>
        /// <returns></returns>
        protected async Task GetActiveTrader()
        {
            _activeTraders = await _traderHub.GetActiveTraders();
        }

        /// <summary>
        /// Returns the list of all active WaveRiders from DataService
        /// </summary>
        /// <returns></returns>
        protected async Task<List<WaveRiderSummaryDTO>> GetWaveRiderList()
        {
            isLoading = true;
            AmaraCode.RainMaker.DataServiceWrapper.WaveRiderWrapper wrWrapper =
                new AmaraCode.RainMaker.DataServiceWrapper.WaveRiderWrapper(userInfo.UserName,
                AppConfig.DataSericeURL, AppConfig.RmApiKey);

            var result = await wrWrapper.GetWaveRiders(userInfo.CompanyID);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToCreateUser()
        {
            navigationManager.NavigateTo("/user/create");
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToCreateWaveRider()
        {
            navigationManager.NavigateTo($"/waverider/create/");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        protected void NavigateToEditWaveRider(string name)
        {
            navigationManager.NavigateTo($"/waverider/edit/{name}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        protected void NavigateToHours(string name)
        {
            navigationManager.NavigateTo($"/waverider/hours/{name}");
        }

        protected void NavigateToMonitor(string name)
        {
            //get the uri for this location

            navigationManager.NavigateTo($"/tradermonitor/{name}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        protected void NavigateToTraderState(string traderName)
        {
            navigationManager.NavigateTo($"/trader/state/{traderName}");
        }

        /// <summary>
        ///
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            //userInfo = await Task.Run(() => _userService.GetUserInfo());
            //WaveRiderList = await Task.Run(() => GetWaveRiderList());
            userInfo = await _userService.GetUserInfo();
            WaveRiderList = await GetWaveRiderList();

            SetupSignalR();

            await GetActiveTrader();

            isLoading = false;
        }

        protected override async void OnParametersSet()
        {
            //            base.OnParametersSet();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool RedisKeyExistsAsync(string key)
        {
            var keyExists = Task.Run(() => _cacheService.KeyExistsAsync(key));

            return keyExists.Result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        protected async void SendCloseTradeCommand(string traderName)
        {
            //await Task.Run(() => _traderHub.SendAsync("SendCloseTradeCommand", traderName));
            await _traderHub.SendCloseTradeCommand(traderName);
        }

        /// <summary>
        /// Include this method if the screen should be refreshed
        /// like when coming back from db insert (cqrs)
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldRender()
        {
            var renderUI = true;

            return renderUI;
        }

        /// <summary>
        ///
        /// </summary>
        private void SetupSignalR()
        {
            _traderHub = _signalrService.GetTraderHubClient(userInfo.UserName);
        }
    }
}