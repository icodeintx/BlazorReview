using AmaraCode.RainMaker.DataServiceWrapper;
using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteBlazor.Models;

namespace WebsiteBlazor.Services
{
    public class CompanyService : BaseService
    {
        private CompanyWrapper _companyWrapper;

        private UserService _userService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="redis"></param>
        /// <param name="appConfig"></param>
        /// <param name="userService"></param>
        public CompanyService(AuthenticationStateProvider auth, ICacheService redis, AppConfig appConfig, UserService userService) : base(auth, redis, appConfig)
        {
            _userService = userService;

            Initialize().Wait();
        }

        public CompanyWrapper Wrapper => _companyWrapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddBrokerToCompany(CompanyBrokerCreateModel model)
        {
            await _companyWrapper.AddBrokerToCompany(model.CompanyName, model.BrokerName, model.ApiKey, model.Name, model.AccountNumber, model.SandBox);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task CreateCompany(CompanyDisplayModel company)
        {
            var companyDTO = ConvertCompanyDisplayModelToCompanyDTO(company);

            await _companyWrapper.CreateCompany(companyDTO);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public async Task<List<BrokerDTO>> GetBrokersByCompanyname(string companyName)
        {
            var brokers = await _companyWrapper.GetBrokersByCompanyName(companyName);
            return brokers;
        }

        /// <summary>
        /// Get list of all companies.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyDTO>> GetCompanies()
        {
            var companies = await _companyWrapper.GetCompanies();

            return companies;
        }

        /// <summary>
        /// Gets list of companies assigned to user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<CompanyDTO>> GetCompaniesByUserName(string userName)
        {
            var companies = await _companyWrapper.GetCompaniesByUser(userName);

            return companies;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <returns></returns>
        public async Task<CompanyBrokerDTO> GetCompanyBroker(Guid companyBrokerID)
        {
            var result = await _companyWrapper.GetCompanyBroker(companyBrokerID);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<CompanyBrokerDTO>> GetCompanyBrokerByUserName(string userName)
        {
            var result = await _companyWrapper.GetCompanyBrokerByUserName(userName);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="brokerName"></param>
        /// <returns></returns>
        public async Task<List<CompanyBrokerDTO>> GetCompanyBrokers(string companyName, string brokerName)
        {
            var result = await _companyWrapper.GetCompanyBrokers(companyName, brokerName);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns>Returns list of CompanyBrokerDTO based on Company.</returns>
        public async Task<List<CompanyBrokerDTO>> GetCompanyBrokers(string companyName)
        {
            var result = await _companyWrapper.GetCompanyBrokers(companyName);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public async Task<CompanyDisplayModel> GetCompanyById(Guid companyID)
        {
            var company = await _companyWrapper.GetCompanyById(companyID);

            if (company is not null)
            {
                return ConvertCompanyDTOToCompanyDisplayModel(company);
            }
            else
            {
                return new CompanyDisplayModel();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public async Task<string> GetCompanyName(Guid companyID)
        {
            var x = await _companyWrapper.GetCompanyName(companyID);

            return x;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<CompanyBrokerDTO>> GetDistinctCompanyBrokerByUserName(string userName)
        {
            var result = await _companyWrapper.GetDistinctCompanyBrokerByUserName(userName);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <returns></returns>
        public async Task RemoveBrokerFromCompany(Guid companyBrokerID)
        {
            await _companyWrapper.RemoveBrokerFromCompany(companyBrokerID);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task SaveCompanyBroker(CompanyBrokerDTO model)
        {
            await _companyWrapper.SaveCompanyBroker(model);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <returns></returns>
        public async Task ToggleCompanyBrokerActive(Guid companyBrokerID)
        {
            await _companyWrapper.ToggleCompanyBrokerActive(companyBrokerID);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task UpdateCompany(CompanyDisplayModel company)
        {
            var companyDTO = ConvertCompanyDisplayModelToCompanyDTO(company);

            await _companyWrapper.UpdateCompany(companyDTO);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="companyNames"></param>
        /// <returns></returns>
        public async Task UpdateCompanyUser(string userName, List<string> companyNames)
        {
            //lookup the user
            var user = await _userService.GetUserByUserName(userName);

            //get list of all companies
            var companies = await GetCompanies();

            //get the current companies for the user
            var userCompanies = await GetCompaniesByUserName(userName);

            //loop throuh the selected (from web-screen) companyNames to assign to user
            foreach (string companyName in companyNames)
            {
                //verify if the company name is already in the assigned companies
                var isassigned = userCompanies.Where(x => x.Name == companyName).Select(c => c.Name).Contains(companyName);
                if (isassigned == false)
                {
                    //the company does not exist in the list of assigned companies
                    //so add company to the UserCompany in database
                    await _companyWrapper.AddUser(companyName, user.UserName);
                }
            }

            //loop through all companies and check if company
            //exist in new list passed into this method
            foreach (var company in companies)
            {
                //if the Company Name is not in the list of selected companies
                if (!companyNames.Contains(company.Name))
                {
                    //and if the user is currently assigned to the company
                    //then remove the user from the company

                    var isAssignedToCompany = userCompanies.Where(x => x.Name == company.Name).Select(c => c.Name).Contains(company.Name);
                    if (isAssignedToCompany == true)
                    {
                        //remove user from company
                        await _companyWrapper.RemoveUser(company.Name, user.UserName);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private CompanyDTO ConvertCompanyDisplayModelToCompanyDTO(CompanyDisplayModel model)
        {
            return new CompanyDTO
            {
                ACPercent = model.ACPercent,
                Active = model.Active,
                Address = model.Address,
                City = model.City,
                Contact = model.Contact,
                Email = model.Email,
                LogoFile = model.LogoFile,
                Name = model.Name,
                Phone = model.Phone,
                CompanyID = model.CompanyID,
                State = model.State,
                TaxPercent = model.TaxPercent,
                Zip = model.Zip
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private CompanyDisplayModel ConvertCompanyDTOToCompanyDisplayModel(CompanyDTO model)
        {
            return new CompanyDisplayModel
            {
                ACPercent = model.ACPercent,
                Active = model.Active,
                Address = model.Address,
                City = model.City,
                Contact = model.Contact,
                Email = model.Email,
                LogoFile = model.LogoFile,
                Name = model.Name,
                Phone = model.Phone,
                CompanyID = model.CompanyID,
                State = model.State,
                TaxPercent = model.TaxPercent,
                Zip = model.Zip
            };
        }

        private async Task Initialize()
        {
            _companyWrapper = new CompanyWrapper((await _userService.GetCurrentUserName()), _appConfig.DataSericeURL, _appConfig.RmApiKey);
        }
    }
}