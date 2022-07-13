using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;

namespace WebsiteBlazor.Pages.WaveRider
{
    public partial class WaveRiderCreate : ComponentBase
    {
        protected List<CompanyBrokerDTO> companyBrokers = new List<CompanyBrokerDTO>();

        protected WaveRiderDisplayModel newWaveRider = new WaveRiderDisplayModel();

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected string CompanyName { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task CreateNewWaveRider()
        {
            try
            {
                //set the company name
                newWaveRider.CompanyName = CompanyName;

                await _traderService.CreateNewWaverider(newWaveRider);

                Toast.DisplaySuccessToast("WaveRider has been created");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            NavigateBack();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task GetCompanyBrokers()
        {
            var user = await _userService.GetUserInfo();
            companyBrokers = await _companyService.GetCompanyBrokers(user.CompanyName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetCompanyName()
        {
            var user = await _userService.GetUserInfo();
            var companyName = await _companyService.GetCompanyName(user.CompanyID);

            return companyName;
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
        }
    }
}