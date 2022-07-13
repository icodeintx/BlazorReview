using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using AmaraCode.RainMaker.SignalrClient;
using WebsiteBlazor.Services;
using Microsoft.JSInterop;

namespace WebsiteBlazor.Pages.Trader
{
    public class SignalrPageModel
    {
        public List<MarkupString> LineItems { get; set; } = new();
    }

    /// <summary>
    ///
    /// </summary>
    public partial class TraderMonitor : ComponentBase
    {
        public TraderMonitor()
        {
        }

        public SignalrPageModel SignalrPageModel { get; set; } = new();

        [Parameter]
        public string TraderName { get; set; }

        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        [Inject]
        protected SignalrService SignalrService { get; set; }

        protected TraderMonitorClient TraderMonitorClient { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            //Create the client object for Signalr
            TraderMonitorClient = SignalrService.GetTradeMonitorClient(TraderName);

            //subscribe to the event
            TraderMonitorClient.MessageReceived += TraderMonitorClient_MessageReceived;
        }

        /// <summary>
        ///
        /// </summary>
        private void NavigateToReturnUrl()
        {
            _jsRuntime.InvokeVoidAsync("history.go", -1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraderMonitorClient_MessageReceived(object sender, string e)
        {
            if (SignalrPageModel.LineItems.Count > 15) { SignalrPageModel.LineItems.RemoveAt(0); }

            var htmlString = new MarkupString(e);

            SignalrPageModel.LineItems.Add(htmlString);

            StateHasChanged();
        }
    }
}