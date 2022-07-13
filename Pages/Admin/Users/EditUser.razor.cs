using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Users
{
    public partial class EditUser : ComponentBase
    {
        private List<CompanyDTO> companies = new();
        private List<CompanyDTO> userCompany = new();
        private List<CompanyAssigned> userCompanyAssigned = new();
        private List<RoleAssigned> userRoleAssigned = new();
        private List<string> userRoles = new();
        private List<string> roles = new();

        [CascadingParameter]
        public Toast Toast { get; set; }

        public UserDto User { get; set; } = new UserDto();

        [ParameterAttribute]
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
        public async Task UpdateUser()
        {
            try
            {
                //update the user information
                await UserService.UpdateUser(User);

                //update the roles
                await UpdateUserRoles();

                //update the companies
                await UpdateUserCompanies();

                Toast.DisplaySuccessToast("User has been Updated");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //navigationManager.NavigateTo($"/admin/user/list", true);
        }

        /// <summary>
        /// Update Identity database with selected roles.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateUserRoles()
        {
            //build a list of only the selected roles
            var rolesSelected = userRoleAssigned.Where(x => x.IsAssigned == true).Select(n => n.RoleName).ToList();

            //call the UserService to update Identity Database
            await UserService.UpdateUserRoles(User.UserName, rolesSelected);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            //call to the database and get the user information
            await GetUser();
            await GetUserRoles();
            await GetRoles();
            BuildUserRoleAssigned();
            await GetCompanies();
            await GetUserCompanies();
            BuildUserCompanyAssigned();
        }

        /// <summary>
        /// Handle setting the checked values of the checkbox for companies
        /// </summary>
        /// <param name="company"></param>
        /// <param name="selectedValue"></param>
        protected void UserCompanyCheck(string company, bool selectedValue)
        {
            var ra = userCompanyAssigned.Where(x => x.CompanyName == company).FirstOrDefault();
            ra.IsAssigned = selectedValue;
        }

        /// <summary>
        /// Handle setting the checked values of the checkbox for roles
        /// </summary>
        /// <param name="role"></param>
        /// <param name="selectedValue"></param>
        protected void UserRoleCheck(string role, bool selectedValue)
        {
            var ra = userRoleAssigned.Where(x => x.RoleName == role).FirstOrDefault();
            ra.IsAssigned = selectedValue;
        }

        /// <summary>
        /// Builds the userCompanyAssigned List
        /// </summary>
        private void BuildUserCompanyAssigned()
        {
            foreach (var company in companies)
            {
                var item = new CompanyAssigned
                {
                    CompanyName = company.Name,
                    IsAssigned = userCompany.Where(c => c.Name == company.Name).Select(x => x.Name).Contains(company.Name)
                };

                userCompanyAssigned.Add(item);
            }
        }

        /// <summary>
        /// Builds the useRoleAssigned List
        /// </summary>
        private void BuildUserRoleAssigned()
        {
            foreach (string role in roles)
            {
                var item = new RoleAssigned
                {
                    RoleName = role,
                    IsAssigned = userRoles.Contains(role)
                };

                userRoleAssigned.Add(item);
            }
        }

        private async Task GetCompanies()
        {
            companies = await CompanyService.GetCompanies();
        }

        /// <summary>
        ///
        /// </summary>
        private async Task GetRoles()
        {
            var results = await _userService.GetRoles();
            roles = results.Select(x => x.Name).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        private async Task GetUser()
        {
            User = await UserService.GetUserByUserName(UserName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private async Task GetUserRoles()
        {
            //userRoles = await UserService.GetUserRoles(UserName);
            var results = await UserService.GetUserRoles(UserName);
            userRoles = results.Select(x => x.RoleName).ToList();
        }

        private async Task GetUserCompanies()
        {
            userCompany = await CompanyService.GetCompaniesByUserName(User.UserName);
        }

        private async Task UpdateUserCompanies()
        {
            //build a list of only the selected companies
            var companiesSelected = userCompanyAssigned.Where(x => x.IsAssigned == true).Select(u => u.CompanyName).ToList();

            //call the UserService to update rm
            await CompanyService.UpdateCompanyUser(User.UserName, companiesSelected);
        }
    }
}