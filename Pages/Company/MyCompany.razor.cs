using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Company
{
    public partial class MyCompany : ComponentBase
    {
        public CompanyDisplayModel Company { get; set; } = new CompanyDisplayModel();

        [CascadingParameter]
        public Toast Toast { get; set; }

        public UserInfo UserInfo { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task SaveMyCompany()
        {
            try
            {
                if (UserInfo.UserName.ToLower() == "guest")
                {
                    Toast.DisplaySuccessToast($"Simulated for guest user");
                    return;
                }

                await CompanyService.UpdateCompany(Company);

                Toast.DisplaySuccessToast($"Company {Company.Name} has been Updated");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //navigationManager.NavigateTo("/");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await GetCompany();
        }

        /// <summary>
        ///
        /// </summary>
        private async Task GetCompany()
        {
            Company = await CompanyService.GetCompanyById(UserInfo.CompanyID);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private async Task GetUserInfo()
        {
            UserInfo = await UserService.GetUserInfo();
        }
    }
}