using AmaraCode.RainMaker.Services.Broker;
using AmaraCode.RainMaker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteBlazor.Services
{
    public class CurrencyService
    {
        public CurrencyService()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<Currency> GetCurrencies(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Currencies.Data;
            }
            else
            {
                List<Currency> list = new List<Currency>();

                var result = Currencies.Data.Where(x => x.Code.ToLower() == code.ToLower()).FirstOrDefault();

                if (result is not null)
                {
                    list.Add(result);
                }

                return list;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Currency GetCurrency(string code)
        {
            Currency result = Currencies.Data.Where(x => x.Code.ToLower() == code.ToLower()).FirstOrDefault();

            return result;
        }
    }
}