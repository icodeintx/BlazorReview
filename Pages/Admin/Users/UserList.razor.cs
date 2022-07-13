using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Users
{
    public partial class UserList : ComponentBase
    {
        protected UserDisplayModel newUser = new();

        protected List<UserDto> users = new();

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected AlertService AlertService { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        protected string CurrentUser { get; set; }

        //[CascadingParameter]
        protected UserInfo UserInfo { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected async Task<List<AmaraCode.RainMaker.Models.UserDto>> GetUsers()
        {
            CurrentUser = await UserService.GetCurrentUserName();

            var result = await UserService.GetUsers();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateToCreateUser()
        {
            navigationManager.NavigateTo("/admin/user/create");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        protected void NavigateToEditUser(string userId)
        {
            navigationManager.NavigateTo($"admin/user/edit/{userId}");
        }

        protected override async Task OnInitializedAsync()
        {
            users = await Task.Run(() => GetUsers());
        }

        /// <summary>
        ///
        /// </summary>
        protected async Task ReadFromRedis()
        {
            var userName = await UserService.GetCurrentUserName();

            UserInfo = await UserService.GetUserInfoFromCache(userName);
        }
    }
}