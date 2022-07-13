using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.WaveRider
{
    public partial class WaveRiderEdit : ComponentBase
    {
        protected List<CompanyBrokerDTO> companyBrokers = new();
        protected WaveRiderDisplayModel editWaveRider = new();

        [ParameterAttribute]
        public string Name { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected string CompanyName { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected TraderService TraderService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        [Inject]
        private EngineService EngineService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task GetCompanyBrokers()
        {
            var user = await UserService.GetUserInfo();
            companyBrokers = await CompanyService.GetCompanyBrokers(user.CompanyName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetCompanyName()
        {
            var user = await UserService.GetUserInfo();
            var companyName = await CompanyService.GetCompanyName(user.CompanyID);

            return companyName;
        }

        /// <summary>
        ///
        /// </summary>
        protected async Task GetWaveRiderEdit()
        {
            editWaveRider = await TraderService.GetWaveRiderEdit(Name);
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateBack()
        {
            navigationManager.NavigateTo("/waverider/list");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            CompanyName = await GetCompanyName();
            await GetCompanyBrokers();
            await GetWaveRiderEdit();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task SaveWaveRider()
        {
            try
            {
                if ((await _userService.GetCurrentUserName()).ToLower() == "guest")
                {
                    Toast.DisplaySuccessToast("Simulated for guest user");
                    return;
                }

                //set the company name
                editWaveRider.CompanyName = CompanyName;

                await TraderService.SaveWaveRider(editWaveRider);

                //send Signalr message to Update Config
                EngineService.NotifyTraderToUpdateConfig(editWaveRider.Name);

                Toast.DisplaySuccessToast("WaveRider has been Updated");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            NavigateBack();
        }
    }
}