using AmaraCode.RainMaker.DataService.Domain.Models;
using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Company
{
    public partial class CompanyBroker : ComponentBase
    {
        [Inject]
        public AlertService AlertService { get; set; }

        [Inject]
        public BrokerService BrokerService { get; set; }

        public List<CompanyBrokerDTO> CompanyBrokers { get; set; } = new();

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        private UserInfo User { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void EditCompanyBroker(Guid companyBrokerID)
        {
            navigationManager.NavigateTo($"/companybroker/edit/{companyBrokerID}");
        }

        public void NavigateToCreateCompanyBroker()
        {
            navigationManager.NavigateTo($"/companybroker/create/{User.CompanyName}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <returns></returns>
        public async Task ToggleActive(Guid companyBrokerID)
        {
            await CompanyService.ToggleCompanyBrokerActive(companyBrokerID);

            AlertService.AddMessage("Active has been toggled");

            //repopulate the list
            CompanyBrokers = await GetCompanyBrokers();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            CompanyBrokers = await Task.Run(() => GetCompanyBrokers());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyBrokerID"></param>
        /// <returns></returns>
        protected async Task RemoveCompanyBroker(Guid companyBrokerID)
        {
            await CompanyService.RemoveBrokerFromCompany(companyBrokerID);

            AlertService.AddMessage("Broker assignment has been removed.");

            //repopulate the list
            CompanyBrokers = await GetCompanyBrokers();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private async Task<List<CompanyBrokerDTO>> GetCompanyBrokers()
        {
            User = await UserService.GetUserInfo();

            var results = await CompanyService.GetCompanyBrokers(User.CompanyName, User.BrokerName);
            return results;
        }
    }
}