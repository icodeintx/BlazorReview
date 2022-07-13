using AmaraCode.RainMaker.DataServiceWrapper;
using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using WebsiteBlazor.Models;

namespace WebsiteBlazor.Services
{
    public class TraderService : BaseService
    {
        private readonly UserService _userService;
        private TraderWrapper _traderWrapper;
        private WaveRiderWrapper _waveRiderWrapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="redis"></param>
        /// <param name="appConfig"></param>
        /// <param name="userService"></param>
        public TraderService(AuthenticationStateProvider auth, ICacheService redis, AppConfig appConfig, UserService userService) : base(auth, redis, appConfig)
        {
            _userService = userService;

            Initialize().Wait();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        public async Task CreateNewWaverider(WaveRiderDisplayModel model)
        {
            WaveRiderCreateEdit newModel = new()
            {
                //AccountNumber = model.AccountNumber,
                AccountUsageAmount = model.AccountUsageAmount,
                BrokerName = model.BrokerName,
                CompanyName = model.CompanyName,
                Instrument = model.Instrument,
                Name = model.Name,
                ScrapeAmount = model.ScrapeAmount,
                Shutdown = model.Shutdown,
                SignificantDigit = model.SignificantDigit,
                CompanyBrokerID = model.CompanyBrokerID
            };

            await _waveRiderWrapper.CreateWaveRider(newModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<InstrumentSelectDTO>> GetAvailableInstruments(Guid brokerID)
        {
            var results = await _traderWrapper.GetAvailableInstruments(brokerID);
            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<ClosedTradeDTO>> GetClosedTradesByDate(Guid companyBrokerID, DateTime date)
        {
            //create dateonly object to send to wrapper
            DateOnly dateOnly = DateOnly.FromDateTime(date);

            return await _traderWrapper.GetClosedTradesByDate(companyBrokerID, dateOnly);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<ClosedTradeDTO>> GetClosedTradesByMonthYear(Guid companyBrokerID, int month, int year)
        {
            return await _traderWrapper.GetClosedTradesForMonthYear(companyBrokerID, month, year);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <param name="startTradeID"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        public async Task<List<ClosedTradeDTO>> GetClosedTradesByPage(Guid companyBrokerID, int startTradeID, int takeCount)
        {
            return await _traderWrapper.GetClosedTradesByPage(companyBrokerID, startTradeID, takeCount);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <param name="year"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public async Task<List<ClosedTradeDTO>> GetClosedTradesByTradeWeek(Guid companyBrokerID, int year, int week)
        {
            return await _traderWrapper.GetClosedTradesForTradeWeek(companyBrokerID, year, week);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waveRiderID"></param>
        /// <returns></returns>
        public async Task<WaveRiderTraderConfig> GetWaveRiderConfig(Guid waveRiderID)
        {
            var model = await _waveRiderWrapper.GetWaveRiderConfig(waveRiderID); ;
            return model;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<WaveRiderTraderConfig> GetWaveRiderConfig(string name)
        {
            var model = await _waveRiderWrapper.GetWaveRiderConfig(name); ;
            return model;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waveRiderName"></param>
        /// <returns></returns>
        public async Task<WaveRiderDisplayModel> GetWaveRiderEdit(string waveRiderName)
        {
            WaveRiderDisplayModel newModel = null;
            var model = await _waveRiderWrapper.GetWaveRiderEngine(waveRiderName);
            if (model != null)
            {
                newModel = new WaveRiderDisplayModel
                {
                    //AccountNumber = model.AccountNumber,
                    AccountUsageAmount = model.AccountUsageAmount,
                    BrokerName = model.BrokerName,
                    CompanyName = model.CompanyName,
                    WaveRiderID = model.WaveRiderID,
                    Name = model.Name,
                    ScrapeAmount = model.ScrapeAmount,
                    Shutdown = model.Shutdown,
                    Active = model.Active,
                    CompanyBrokerID = model.CompanyBrokerID
                };
            }

            return newModel;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public async Task<WaveRiderHourDTO> GetWaveRiderHour(string name, int hour)
        {
            var model = await _waveRiderWrapper.GetWaveRiderHour(name, hour);

            return model;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        public async Task SaveWaveRider(WaveRiderDisplayModel model)
        {
            var newModel = new WaveRiderCreateEdit
            {
                //AccountNumber = model.AccountNumber,
                AccountUsageAmount = model.AccountUsageAmount,
                BrokerName = model.BrokerName,
                CompanyName = model.CompanyName,
                WaveRiderID = model.WaveRiderID,
                Name = model.Name,
                ScrapeAmount = model.ScrapeAmount,
                Shutdown = model.Shutdown,
                Active = model.Active,
                CompanyBrokerID = model.CompanyBrokerID
            };
            await _waveRiderWrapper.UpdateWaveRider(newModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="waveRiderName"></param>
        /// <param name="hourInfo"></param>
        public async Task SaveWaveRiderHour(string waveRiderName, WaveRiderHourDTO hourInfo)
        {
            //set the WaveRider name in the hour object before sending
            //to the api
            //hourInfo.Name = waveRiderName;

            await _waveRiderWrapper.UpdateWaveRiderHour(hourInfo, waveRiderName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private async Task Initialize()
        {
            var userName = await _userService.GetCurrentUserName();

            _waveRiderWrapper = new WaveRiderWrapper(userName, _appConfig.DataSericeURL, _appConfig.RmApiKey);
            _traderWrapper = new TraderWrapper(userName, _appConfig.DataSericeURL, _appConfig.RmApiKey);
        }
    }
}