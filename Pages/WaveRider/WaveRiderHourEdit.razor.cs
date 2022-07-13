using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using WebsiteBlazor.Services;
using WebsiteBlazor.Models;

namespace WebsiteBlazor.Pages.WaveRider
{
    public partial class WaveRiderHourEdit : ComponentBase
    {
        protected WaveRiderHourEditModel editWaveRiderHour = new();

        private IEnumerable<InstrumentSelectDTO> _availableInstruments = new List<InstrumentSelectDTO>();

        [ParameterAttribute]
        public string Hour { get; set; }

        [ParameterAttribute]
        public string Name { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected string CompanyName { get; set; }

        [Inject]
        private EngineService EngineService { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected async Task GetWaveRiderHourEdit()
        {
            var model = await _traderService.GetWaveRiderHour(Name, Convert.ToInt32(Hour));

            if (model is not null)
            {
                editWaveRiderHour = new WaveRiderHourEditModel
                {
                    ChannelWidth= model.ChannelWidth,
                    Name = Name,
                    CurveIntervals= model.CurveIntervals,
                    EngineDelay=model.EngineDelay,
                    Hour=model.Hour,
                    Instrument=model.Instrument,
                    LongZone=model.LongZone,
                    MaxChannelHeight=model.MaxChannelHeight,
                    MaxSpread=model.MaxSpread,
                    MinChannelHeight=model.MinChannelHeight,
                    ProfitStrategy=model.ProfitStrategy,
                    ShortZone=model.ShortZone,
                    StopLossMultiplier=model.StopLossMultiplier,
                    TradeExpirationMinutes=model.TradeExpirationMinutes
                };
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateBack()
        {
            navigationManager.NavigateTo($"/waverider/hours/{Name}", true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            //CompanyName = GetCompanyName();
            await GetInstruments();
            await GetWaveRiderHourEdit();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task SaveWaveRiderHour()
        {
            if ((await _userService.GetCurrentUserName()).ToLower() == "guest")
            {
                Toast.DisplaySuccessToast("Simulated for guest user");
                return;
            }

            /*first we have to convert the wave the WaveRiderEditModel
             * which is only used in the website
             * to a WaveRideHourDTO class to send back to the Wrapper
             */

            try
            {
                var dtoModel = new WaveRiderHourDTO
                {
                    ChannelWidth=editWaveRiderHour.ChannelWidth,
                    CurveIntervals=editWaveRiderHour.CurveIntervals,
                    EngineDelay=editWaveRiderHour.EngineDelay,
                    Hour=editWaveRiderHour.Hour,
                    Instrument=editWaveRiderHour.Instrument,
                    LongZone=editWaveRiderHour.LongZone,
                    MaxChannelHeight=editWaveRiderHour.MaxChannelHeight,
                    MaxSpread=editWaveRiderHour.MaxSpread,
                    MinChannelHeight=editWaveRiderHour.MinChannelHeight,
                    ProfitStrategy=editWaveRiderHour.ProfitStrategy,
                    ShortZone=editWaveRiderHour.ShortZone,
                    StopLossMultiplier=editWaveRiderHour.StopLossMultiplier,
                    TradeExpirationMinutes=editWaveRiderHour.TradeExpirationMinutes,
                };

                await _traderService.SaveWaveRiderHour(Name, dtoModel);

                //send Signalr message to Update Config
                EngineService.NotifyTraderToUpdateConfig(Name);

                Toast.DisplaySuccessToast("WaveRider Hour has been Updated");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            NavigateBack();
        }

        private string ConvertInstrument(InstrumentSelectDTO inst) => inst.InstrumentName;

        /// <summary>
        ///
        /// </summary>
        private async Task GetInstruments()
        {
            var user = await _userService.GetUserInfo();
            _availableInstruments = await _traderService.GetAvailableInstruments(user.BrokerID);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private async Task<IEnumerable<InstrumentSelectDTO>> SearchInstruments(string searchText)
        {
            var result = await Task.FromResult(_availableInstruments.Where(x => x.InstrumentName.ToLower().Contains(searchText.ToLower())).ToList());
            return result;
        }
    }
}