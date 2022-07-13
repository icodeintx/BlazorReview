using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace WebsiteBlazor.Pages.Trader
{
    /// <summary>
    ///
    /// </summary>
    public partial class ClosedTradesMenu : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToClosedTradesByDate()
        {
            NavigationManager.NavigateTo("/closedtrades/bydate");
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToClosedTradesByMonthYear()
        {
            NavigationManager.NavigateTo("/closedtrades/bymonthyear");
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToClosedTradesByTradeID()
        {
            NavigationManager.NavigateTo("/closedtrades/bytradeid");
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToClosedTradesByTradeWeek()
        {
            NavigationManager.NavigateTo("/closedtrades/bytradeweek");
        }
    }
}