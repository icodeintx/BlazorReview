using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Company
{
    public partial class CompanyBrokerAdd : ComponentBase
    {
        public List<BrokerDTO> Brokers { get; set; } = new List<BrokerDTO>();

        [Inject]
        public BrokerService BrokerService { get; set; }

        [Parameter]
        public string CompanyName { get; set; }

        public CompanyBrokerCreateModel CreateModel { get; set; } = new CompanyBrokerCreateModel();

        /// <summary>
        /// The selected item in the dropdown for the Broker selection
        /// </summary>
        public string SelectedBroker { get; set; } = "";

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void CancelCompanyBrokerEdit()
        {
            navigationManager.NavigateTo("/companybroker");
        }

        //private UserInfo User { get; set; }
        /// <summary>
        ///
        /// </summary>
        protected async void AddCompanyBroker()
        {
            try
            {
                if ((await _userService.GetCurrentUserName()).ToLower() == "guest")
                {
                    Toast.DisplaySuccessToast("Simulated for guest user");
                    return;
                }

                if (!string.IsNullOrEmpty(SelectedBroker) &&
                    !string.IsNullOrEmpty(CreateModel.ApiKey) &&
                    !string.IsNullOrEmpty(CreateModel.Name))
                {
                    CreateModel.CompanyName = CompanyName;
                    CreateModel.BrokerName = SelectedBroker;

                    await CompanyService.AddBrokerToCompany(CreateModel);

                    CancelCompanyBrokerEdit();
                }

                Toast.DisplaySuccessToast("Company-Broker has been added.");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => GetBrokers());
        }

        /// <summary>
        ///
        /// </summary>
        private async Task GetBrokers()
        {
            Brokers = await BrokerService.GetBrokers();
        }
    }
}