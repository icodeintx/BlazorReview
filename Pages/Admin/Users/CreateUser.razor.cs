using Microsoft.AspNetCore.Components;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Users
{
    public partial class CreateUser : ComponentBase
    {
        protected UserDto newUser = new();

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected UserService userService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task CreateNewUser()
        {
            try
            {
                await userService.CreateUserAsync(newUser);

                Toast.DisplaySuccessToast("User has been Updated");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //navigationManager.NavigateTo($"/admin/user/list", true);
        }
    }
}