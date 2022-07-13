using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace WebsiteBlazor.Pages.Trader
{
    /// <summary>
    ///
    /// </summary>
    public partial class State : ComponentBase
    {
        private bool isLoading = true;

        public TraderState Trader_State { get; set; } = null;

        [Parameter]
        public string TraderName { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="traderName"></param>
        protected async Task GetTraderState(string traderName)
        {
            string keyName = $"TraderState_{traderName}";

            Trader_State = await _cacheService.GetCacheValueAsync<TraderState>(keyName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Run(() => GetTraderState(TraderName));

                //await Task.Run(() => GetTemperatures());

                isLoading = false;

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}