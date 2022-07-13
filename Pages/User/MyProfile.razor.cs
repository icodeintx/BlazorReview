using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.User
{
    public partial class MyProfile : ComponentBase
    {
        [CascadingParameter]
        public Toast Toast { get; set; }

        public UserDto User { get; set; } = new();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task UpdateUser()
        {
            try
            {
                var userName = await _userService.GetCurrentUserName();

                if (userName.ToLower() == "guest")
                {
                    Toast.DisplaySuccessToast("Simulated for Guest User");
                }
                else
                {
                    await _userService.UpdateUser(User);

                    Toast.DisplaySuccessToast("User has been Updated");
                }
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
        protected override async Task OnInitializedAsync()
        {
            //call to the database and get the user information

            await GetUser();
        }

        /// <summary>
        ///
        /// </summary>
        private async Task GetUser()
        {
            //Error.DisplaySuccessToast("you did it");
            var userName = await _userService.GetCurrentUserName();
            User = await _userService.GetUserByUserName(userName);
        }
    }
}