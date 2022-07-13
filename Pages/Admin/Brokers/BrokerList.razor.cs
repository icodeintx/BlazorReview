using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Brokers
{
    public partial class BrokerList : ComponentBase
    {
        protected List<BrokerDTO> Brokers = new();

        [Inject]
        protected AlertService AlertService { get; set; }

        [Inject]
        protected AppConfig AppConfig { get; set; }

        [Inject]
        protected BrokerService BrokerService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<List<BrokerDTO>> GetBrokers()
        {
            var CurrentUser = await UserService.GetCurrentUserName();
            AmaraCode.RainMaker.DataServiceWrapper.BrokerWrapper brokerWrapper = new(CurrentUser, AppConfig.DataSericeURL, AppConfig.RmApiKey);
            return await brokerWrapper.GetBrokers();
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToCreateCompany()
        {
            navigationManager.NavigateTo("/admin/broker/create");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="brokerID"></param>
        protected void NavigateToEditCompany(string brokerID)
        {
            navigationManager.NavigateTo($"admin/broker/edit/{brokerID}");
        }

        protected override async Task OnInitializedAsync()
        {
            Brokers = await Task.Run(() => GetBrokers());
        }

        /// <summary>
        /// Include this method if the screen should be refreshed
        /// like when coming back from db insert (cqrs)
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldRender()
        {
            var renderUI = true;

            return renderUI;
        }
    }
}