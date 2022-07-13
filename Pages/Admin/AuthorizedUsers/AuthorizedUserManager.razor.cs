using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.AuthorizedUsers
{
    public partial class AuthorizedUserManager : ComponentBase
    {
        public AuthorizedUserDisplayModel newAuthorizedUser = new();

        public List<AuthorizedUserDto> AuthorizedUsers { get; set; } = new();

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected async Task CreateAuthorizedUser()
        {
            try
            {
                if (!string.IsNullOrEmpty(newAuthorizedUser.EmailAddress))
                {
                    _ = await UserService.CreateAuthorizedUser(newAuthorizedUser.EmailAddress);
                }

                Toast.DisplaySuccessToast($"Authorized User {newAuthorizedUser.EmailAddress} Created");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //refresh
            await GetAuthorizedUser();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleName"></param>
        protected async Task RemoveAuthorizedUser(string emailAddress)
        {
            try
            {
                if (!string.IsNullOrEmpty(emailAddress))
                {
                    var result = await UserService.RemoveAuthorizedUser(emailAddress);
                    if (result > 0)
                    {
                        //refresh
                        await GetAuthorizedUser();
                    }
                }

                Toast.DisplaySuccessToast($"AuthorizedUser {emailAddress} Deleted");
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
        protected async Task GetAuthorizedUser()
        {
            AuthorizedUsers = await UserService.GetAuthorizedUsers();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            //call to the database and get data
            await GetAuthorizedUser();
        }
    }
}