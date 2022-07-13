using AmaraCode.RainMaker.Models;
using AmaraCode.RainMaker.Services.Broker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Pricing;

/// <summary>
///
/// </summary>
public partial class PriceGrid : ComponentBase
{
    [Parameter]
    public List<Quote> Quotes { get; set; }
}