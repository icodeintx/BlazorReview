using AmaraCode.RainMaker.Models;
using AmaraCode.RainMaker.Services.Broker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Pricing;

public partial class CurrentPrices : ComponentBase
{
    //create backing field for data
    protected List<Quote> _quotes = new();

    [Inject]
    protected BrokerService BrokerService { get; set; }

    [CascadingParameter(Name = "UserInfo")]
    protected UserInfo UserInfo { get; set; }

    protected async Task GetQuotes()
    {
        var broker = await BrokerService.GetBroker();

        //call RMAPI to get List of Instrument Names
        var instruments = await BrokerService.Wrapper.GetInstruments(UserInfo.BrokerID);

        //create a variable to hold the list of instruments
        var instrumentsRequest = new List<string>();
        //populate the list of instruments
        foreach (var item in instruments)
        {
            instrumentsRequest.Add(item.Name);
        }

        //create a request object
        var request = QuoteRequest.CreateForMultipleQuotes(UserInfo.AccountNumber, instrumentsRequest);

        //make call to broker service to get the quotes
        _quotes = await broker.GetQuotes(request);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        await GetQuotes();
        //return base.OnParametersSetAsync();
    }
}