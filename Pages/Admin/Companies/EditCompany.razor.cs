using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Companies
{
    public partial class EditCompany : ComponentBase
    {
        public CompanyDisplayModel Company { get; set; } = new CompanyDisplayModel();

        [ParameterAttribute]
        public string Id { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        public string UserName { get; set; }

        [Inject]
        protected AlertService AlertService { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task GetCompany()
        {
            Company = await CompanyService.GetCompanyById(Guid.Parse(Id));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task UpdateCompany()
        {
            try
            {
                await CompanyService.UpdateCompany(Company);

                Toast.DisplaySuccessToast($"Company {Company.Name} has been updated.");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //navigationManager.NavigateTo($"/admin/company/list", true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => GetUserName());

            await Task.Run(() => GetCompany());
        }

        /// <summary>
        ///
        /// </summary>
        private async Task GetUserName()
        {
            UserName = await UserService.GetCurrentUserName();
        }
    }
}