using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Trader
{
    /// <summary>
    ///
    /// </summary>
    public partial class ClosedTradeGrid : ComponentBase
    {
        [Parameter]
        public List<ClosedTradeDTO> Trades { get; set; }
    }
}