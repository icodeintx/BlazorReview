using AmaraCode.RainMaker.Models;
using AmaraCode.RainMaker.Services.Broker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Pricing;

public partial class CurrentMajors : ComponentBase
{
    //create backing field for data
    protected List<Quote> _quotes = new();

    [Inject]
    protected BrokerService BrokerService { get; set; }

    [CascadingParameter(Name = "UserInfo")]
    protected UserInfo UserInfo { get; set; }

    protected async Task GetQuotes()
    {
        //_quotes = await TraderService.GetClosedTradesByTradeWeek(this.CompanyBrokerID, ClosedTradeQueryModel.Year, ClosedTradeQueryModel.Week);
        var broker = await BrokerService.GetBroker();

        var instrumentsRequest = new List<string>();
        instrumentsRequest.Add("EUR_USD");
        instrumentsRequest.Add("USD_JPY");
        instrumentsRequest.Add("GBP_USD");
        instrumentsRequest.Add("USD_CHF");
        instrumentsRequest.Add("AUD_USD");
        instrumentsRequest.Add("USD_CAD");
        instrumentsRequest.Add("NZD_USD");

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
        var x = UserInfo.BrokerName;
    }
}