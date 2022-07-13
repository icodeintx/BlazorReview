using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Companies
{
    public partial class CreateCompany : ComponentBase
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
        public async Task Create()
        {
            try
            {
                await CompanyService.CreateCompany(Company);

                Toast.DisplaySuccessToast($"Company {Company.Name} created.");
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