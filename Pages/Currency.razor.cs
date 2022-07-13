using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages
{
    public partial class Currency : ComponentBase
    {
        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected BrokerService BrokerService { get; set; }

        /*[Inject]
        protected AppConfig config { get; set; }
*/
        protected List<CurrencyItem> CurrencyList { get; set; } = new List<CurrencyItem>();

        [Inject]
        protected CurrencyService CurrencyService { get; set; }

        protected List<InstrumentDTO> Instruments { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected void BuildCurrencyList()
        {
            foreach (var item in Instruments.OrderBy(x => x.Name))
            {
                CurrencyItem ci = new();
                var baseCurrency = CurrencyService.GetCurrency(item.Base);
                var quoteCurrency = CurrencyService.GetCurrency(item.Quote);

                ci.Instrument = item;
                ci.Base_Code = baseCurrency.Code;
                ci.Base_Country = baseCurrency.Country;
                ci.Base_CurrencyName = baseCurrency.CurrencyName;
                ci.Quote_Code = quoteCurrency.Code;
                ci.Quote_Country = quoteCurrency.Country;
                ci.Quote_CurrencyName = quoteCurrency.CurrencyName;

                CurrencyList.Add(ci);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task GetInstruments()
        {
            //get the broker name from the current user
            var userInfo = await UserService.GetUserInfo();

            Instruments = await BrokerService.GetInstruments(userInfo.BrokerName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetInstruments();

                BuildCurrencyList();
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }
        }
    }

    public class CurrencyItem
    {
        public string Base_Code { get; set; }
        public string Base_Country { get; set; }
        public string Base_CurrencyName { get; set; }
        public InstrumentDTO Instrument { get; set; }
        public string Quote_Code { get; set; }
        public string Quote_Country { get; set; }
        public string Quote_CurrencyName { get; set; }
    }
}