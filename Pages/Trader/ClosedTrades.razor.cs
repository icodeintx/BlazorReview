using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace WebsiteBlazor.Pages.Trader
{
    /// <summary>
    ///
    /// </summary>
    public partial class ClosedTrades : ComponentBase
    {
        [CascadingParameter(Name = "CompanyBrokerID")]
        protected Guid CompanyBrokerID { get; set; }
    }
}