using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Roles
{
    public partial class RoleManager : ComponentBase
    {
        public RoleDisplayModel newRole = new();

        public List<RoleDto> roles = new();

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected async Task CreateRole()
        {
            try
            {
                if (!string.IsNullOrEmpty(newRole.Name))
                {
                    _ = UserService.CreateRole(newRole.Name);
                }

                Toast.DisplaySuccessToast($"Role {newRole.Name} Created");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //refresh
            await GetRoles();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleName"></param>
        protected async Task DeleteRole(string roleName)
        {
            try
            {
                if (!string.IsNullOrEmpty(roleName))
                {
                    var result = await UserService.DeleteRole(roleName);
                    if (result > 0)
                    {
                        //refresh
                        await GetRoles();
                    }
                }

                Toast.DisplaySuccessToast($"Role {roleName} Deleted");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task GetRoles()
        {
            roles = await UserService.GetRoles();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            //call to the database and get data
            await GetRoles();
        }
    }
}