using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Trader
{
    /// <summary>
    ///
    /// </summary>
    public partial class ClosedTradesByDate : ComponentBase
    {
        protected List<ClosedTradeDTO> _trades = new();

        public ClosedTradesByDate()
        {
            //default the year and month to current year and month
            ClosedTradeQueryModel.EndDate = DateTime.Now;
        }

        protected ClosedTradeQueryModel ClosedTradeQueryModel { get; set; } = new();

        [CascadingParameter(Name = "UserInfo")]
        protected UserInfo UserInfo { get; set; }

        [Inject]
        protected TraderService TraderService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task GetTrades()
        {
            if (UserInfo is null)
            {
                UserInfo = await UserService.GetUserInfo();
            }
            _trades = await TraderService.GetClosedTradesByDate(UserInfo.CompanyBrokerID, ClosedTradeQueryModel.EndDate);
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