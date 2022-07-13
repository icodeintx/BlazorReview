using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Companies
{
    public partial class CompanyList : ComponentBase
    {
        protected List<CompanyDTO> Companies = new();

        [Inject]
        protected AlertService AlertService { get; set; }

        [Inject]
        protected AppConfig AppConfig { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<List<CompanyDTO>> GetCompanies()
        {
            var CurrentUser = await UserService.GetCurrentUserName();
            AmaraCode.RainMaker.DataServiceWrapper.CompanyWrapper coWrapper = new(CurrentUser, AppConfig.DataSericeURL, AppConfig.RmApiKey);
            return await coWrapper.GetCompanies();
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToCreateCompany()
        {
            navigationManager.NavigateTo("/admin/company/create");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="companyID"></param>
        protected void NavigateToEditCompany(string companyID)
        {
            navigationManager.NavigateTo($"admin/company/edit/{companyID}");
        }

        protected override async Task OnInitializedAsync()
        {
            Companies = await GetCompanies();
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