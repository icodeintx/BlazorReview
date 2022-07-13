using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Trader
{
    /// <summary>
    ///
    /// </summary>
    public partial class ClosedTradesByTradeWeek : ComponentBase
    {
        protected List<ClosedTradeDTO> _trades = new();

        public ClosedTradesByTradeWeek()
        {
            //default the year and week
            ClosedTradeQueryModel.Year = DateTime.UtcNow.Year;

            var tw = AmaraCode.RainMaker.Models.TradeWeek.CreateForNow();
            ; ClosedTradeQueryModel.Week = (int)tw.Week;
        }

        protected ClosedTradeQueryModel ClosedTradeQueryModel { get; set; } = new();

        [CascadingParameter(Name = "UserInfo")]
        protected UserInfo UserInfo { get; set; }

        [Inject]
        protected UserService UserService { get; set; }


        [Inject]
        protected TraderService TraderService { get; set; }

        /// <summary>
        ///  Will get the trade for the current trade week.
        /// </summary>
        /// <returns></returns>
        protected async Task GetTrades()
        {
            if (UserInfo is null)
            {
                UserInfo = await UserService.GetUserInfo();
            }

            _trades = await TraderService.GetClosedTradesByTradeWeek(UserInfo.CompanyBrokerID, ClosedTradeQueryModel.Year, ClosedTradeQueryModel.Week);
        }



        /// <summary>
        ///
        /// </summary>
        protected override async void OnInitialized()
        {
            await GetTrades();
            //base.OnInitialized();
        }
    }
}