using AmaraCode.RainMaker.Services.Broker;
using AmaraCode.RainMaker.DataServiceWrapper;
using AmaraCode.RainMaker.DataServiceWrapper.Abstract;
using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteBlazor.Models;
using WebsiteBlazor.Abstract;

namespace WebsiteBlazor.Services
{
    /// <summary>
    /// Broker Service for the Blazor Website.  Not to be confused with the BrokerService library.
    /// </summary>
    public class BrokerService : BaseService
    {
        private BrokerWrapper _brokerWrapper;
        private CompanyService _companyService;
        private SystemService _systemService;
        private UserService _userService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="redis"></param>
        /// <param name="appConfig"></param>
        /// <param name="userService"></param>
        /// <param name="companyService"></param>
        public BrokerService(AuthenticationStateProvider auth, ICacheService redis,
            AppConfig appConfig, UserService userService,
            SystemService systemService, CompanyService companyService) : base(auth, redis, appConfig)
        {
            _systemService = systemService;
            _companyService = companyService;
            _userService = userService;

            Initialize().Wait();
        }

        public BrokerWrapper Wrapper => _brokerWrapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="broker"></param>
        public async Task CreateBroker(BrokerDisplayModel broker)
        {
            var brokerDTO = ConvertBrokerDisplayModelToBrokerDTO(broker);

            await _brokerWrapper.CreateBroker(brokerDTO);
        }

        /// <summary>
        ///  Get a Broker object that is the actual BROKER from the BrokerService
        /// </summary>
        /// <returns></returns>
        public async Task<IBrokerService> GetBroker()
        {
            //get userInfo for CompanyBrokerID
            var userinfo = await _userService.GetUserInfo();

            var broker = await _brokerWrapper.GetBroker(userinfo.BrokerID);
            var companyBroker = await _companyService.GetCompanyBroker(userinfo.CompanyBrokerID);

            var brokerInfo = BrokerInfo.CreateNew(broker.Alias, companyBroker.BrokerApiKey, companyBroker.AccountNumber, companyBroker.SandBox);
            IBrokerService ibroker = AmaraCode.RainMaker.Services.Broker.Broker.CreateNew(brokerInfo);
            return ibroker;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerID"></param>
        /// <returns></returns>
        public async Task<BrokerDisplayModel> GetBroker(Guid brokerID)
        {
            var result = await _brokerWrapper.GetBroker(brokerID);
            if (result is not null)
            {
                return ConvertBrokerDTOToBrokerDisplayModel(result);
            }
            else
            {
                return new BrokerDisplayModel();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<List<BrokerDTO>> GetBrokers()
        {
            var brokers = await _brokerWrapper.GetBrokers();
            return brokers;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>Returns list of Instruments in JaylenDB for Broker</returns>
        public async Task<List<InstrumentDTO>> GetInstruments(string brokerName)
        {
            var result = await _brokerWrapper.GetInstruments(brokerName);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="broker"></param>
        public async Task UpdateBroker(BrokerDisplayModel broker)
        {
            var brokerDTO = ConvertBrokerDisplayModelToBrokerDTO(broker);

            await _brokerWrapper.UpdateBroker(brokerDTO);
        }

        /// <summary>
        ///  This methods needs to be refactored because it will duplicate your
        ///  instruments.  Consider moving all the logic inside the dataservice to avoid
        ///  api call for each add instrument. OR create new method in api that will accept a
        ///  list of instruments and will Add to database or set inactive for missing instruments.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public async Task UpdateInstrments(Guid companyBrokerID)
        {
            await _systemService.UpdateInstrments(companyBrokerID);
            /*
            //get companyname and brokername
            var userinfo = await _userService.GetUserInfo();

            //if we don't have userinfo then we cannot continue
            if (userinfo == null)
            {
                throw new DataNotFoundException("Cannot find UserInfo from UserService.GetUserInfo");
            }

            //get companybroker object to get ApiKey
            var cb = await _companyService.GetCompanyBroker(userinfo.CompanyBrokerID);

            //create brokerinfo object
            //note we pass in the BrokerName as the alias for simplicity
            var brokerInfo = BrokerInfo.CreateNew(cb.BrokerName, cb.BrokerApiKey, accountNumber);

            //get instruments
            var brokerService = Broker.CreateNew(brokerInfo);

            //add instruments to companyservice.
            var instruments = await brokerService.GetInstruments(accountNumber);

            foreach (var instrument in instruments)
            {
                var dto = new InstrumentDTO
                {
                    Active = true,
                    BrokerID = userinfo.BrokerID,
                    Name = instrument.Name,
                    SignificantDigit = System.Math.Abs(instrument.PipLocation),
                    DisplayName = instrument.Name,
                    DisplayPrecision = instrument.DisplayPrecision,
                    MarginRate = instrument.MarginRate,
                    MaximumOrderUnits = instrument.MaximumOrderUnits,
                    MaximumPositionSize = instrument.MaximumPositionSize,
                    MaximumTrailingStopDistance = instrument.MaximumTrailingStopDistance,
                    MinimumTradeSize = instrument.MinimumTradeSize,
                    MinimumTrailingStopDistance = instrument.MinimumTrailingStopDistance,
                    PipLocation = instrument.PipLocation,
                    TradeUnitsPrecision = instrument.TradeUnitsPrecision,
                    Type = instrument.Type
                };

                await _brokerWrapper.CreateInstrument(dto);
            }*/
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private BrokerDTO ConvertBrokerDisplayModelToBrokerDTO(BrokerDisplayModel model)
        {
            return new BrokerDTO
            {
                Active= model.Active,
                Alias=model.Alias,
                ApiUrl=model.ApiUrl,
                BrokerID=model.BrokerID,
                LogoFile=model.LogoFile,
                Name=model.Name,
                SandboxApiUrl=model.SanboxApiUrl
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private BrokerDisplayModel ConvertBrokerDTOToBrokerDisplayModel(BrokerDTO model)
        {
            return new BrokerDisplayModel
            {
                Active = model.Active,
                Alias = model.Alias,
                ApiUrl = model.ApiUrl,
                BrokerID = model.BrokerID,
                LogoFile = model.LogoFile,
                Name = model.Name,
                SanboxApiUrl = model.SandboxApiUrl
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private async Task Initialize()
        {
            var userName = await _userService.GetCurrentUserName();

            _brokerWrapper = new BrokerWrapper(userName, _appConfig.DataSericeURL, _appConfig.RmApiKey);
        }
    }
}