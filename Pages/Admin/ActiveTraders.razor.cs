using AmaraCode.RainMaker.SignalrClient;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin
{
    public partial class ActiveTraders : ComponentBase
    {
        private TraderHubClient _traderHub;

        /// <summary>
        ///
        /// </summary>
        public ActiveTraders()
        {
        }

        public List<string> Traders { get; set; } = new List<string>();

        [Inject]
        protected SignalrService _signalrService { get; set; }

        [Inject]
        protected AlertService AlertService { get; set; }

        [Inject]
        protected EngineService EngineService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task GetActiveTraders()
        {
            Traders = await EngineService.GetActiveTraders();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            //call to the database and get the user information
            await GetActiveTraders();
            SetupSignalR();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        protected async void SendDumpCommandToSignalR(string traderName)
        {
            await _traderHub.TraderHubConnection.SendAsync("SendDumpCommand", traderName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        protected async void SendPauseToggleToSignalr(string traderName)
        {
            await _traderHub.TraderHubConnection.SendAsync("SendPauseToggleToTrader", traderName);

            AlertService.AddMessage($"{traderName} sent message to update config");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        /// <returns></returns>
        protected async void SendShutdownToSignalr(string traderName)
        {
            //call the PanicShutdownTrader method in TraderHubServer
            await _traderHub.TraderHubConnection.SendAsync("PanicShutdownTrader", traderName);

            AlertService.AddMessage($"{traderName} sent shutdown command");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        /// <returns></returns>
        protected async void SendUpdateToSignalr(string traderName)
        {
            //EngineService.NotifyTraderToUpdateConfig(traderName);
            await _traderHub.TraderHubConnection.SendAsync("SendConfigChangeNotification", traderName);

            AlertService.AddMessage($"{traderName} sent message to update config");
        }

        /// <summary>
        ///
        /// </summary>
        private void SetupSignalR()
        {
            //_traderHub = _signalrService.TraderHubServerConnection();
            _traderHub = _signalrService.GetTraderHubClient("admin");
        }
    }
}