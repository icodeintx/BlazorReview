using AmaraCode.RainMaker.Models;
using AmaraCode.RainMaker.Strategy;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Services;
using Newtonsoft.Json;

namespace WebsiteBlazor.Pages.Temperature
{
    public partial class Temperature : ComponentBase
    {
        private bool isLoading = true;

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected BrokerService BrokerService { get; set; }

        protected List<CurrencyItem> CurrencyList { get; set; } = new List<CurrencyItem>();

        [Inject]
        protected CurrencyService CurrencyService { get; set; }

        protected List<InstrumentDTO> Instruments { get; set; }
        protected List<TemperatureResult> Temperatures { get; set; } = new List<TemperatureResult>();

        protected string UserName { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

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
        protected async Task GetTemperatures()
        {
            //Get the current UserName for the key needed in Redis
            string userName = await _userService.GetCurrentUserName();

            //check to see if the Redis key exists for this user
            if (await _cacheService.KeyExistsAsync($"{userName}_Temperatures") == true)
            {
                //get the information from Redis and display
                var temperatures =  await _cacheService.GetCacheValueAsync<List<TemperatureResult>>($"{userName}_Temperatures");

                //var temperatures = JsonConvert.DeserializeObject<List<TemperatureResult>>(temperatures_string);

                if (temperatures is not null)
                {
                    SetTemperatures(temperatures);
                }
            }
            else
            {
                //first update Redis
                await UpdateTemperatures();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetInstruments();

                await GetTemperatures();

                //isLoading = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            UserName = await _userService.GetCurrentUserName();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="temperatures"></param>
        protected void SetTemperatures(List<TemperatureResult> temperatures)
        {
            //update the module property
            Temperatures = temperatures;
            isLoading = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected async Task UpdateTemperatures()
        {
            //if guest user is logged in then don't really make the call to the broker
            if (UserName.ToLower() == "guest")
            {
                Toast.DisplaySuccessToast("Simulated for Guest user");
                return;
            }

            isLoading = true;
            await Task.Delay(1000);

            //get the temperatures from the broker and save to Redis

            //Get a IBrokerService (BrokerService Library) instance from the BrokerAppService
            //in this project.
            var broker = await BrokerService.GetBroker();

            //create instance of the Temperature class.
            var tmp = new AmaraCode.RainMaker.Strategy.Temperature(broker);

            try
            {
                //get the Temperature Results from the Strategy Temperature class.
                var temperatures = await tmp.GetTemperatures(Instruments);

                //convert the Temperatures to JSON
                var jsonTemperatures = JsonConvert.SerializeObject(temperatures);

                //Get the current UserName for the key needed in Redis
                string userName = await _userService.GetCurrentUserName();

                //Save the updated information to Redis.
                await _cacheService.SetCacheValueAsync($"{userName}_Temperatures", jsonTemperatures);

                //call the method to set the property for Temperatures
                SetTemperatures(temperatures);
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }
        }
    }
}