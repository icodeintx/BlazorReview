using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Company
{
    public partial class ChangeCompany : ComponentBase
    {
        protected List<CompanyBrokerDTO> companies = new();

        [Parameter]
        public string Returnurl { get; set; }

        //public string userName { get; set; }
        public UserInfo UserInfo { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        /// Get a list of companies the user has access to
        /// </summary>
        protected async Task GetCompanyList(string userName)
        {
            companies = await CompanyService.GetCompanyBrokerByUserName(userName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="apiKey"></param>
        protected string MaskApiKey(string apiKey)
        {
            string result = apiKey;

            if (!string.IsNullOrEmpty(result))
            {
                if (result.Length > 4)
                {
                    result = "********" + result.Substring(result.Length - 5);
                }
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected void Navigate()
        {
            if (Returnurl == "")
            {
                navigationManager.NavigateTo("/");
            }
            else
            {
                navigationManager.NavigateTo(Returnurl, true);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            UserInfo = await UserService.GetUserInfo();

            //userName = _userService.GetCurrentUserName();

            await GetCompanyList(UserInfo.UserName);
        }

        /// <summary>
        /// When user clicks on a company button this method is called
        /// which will update the user's redis then call the navigate method
        /// </summary>
        /// <param name="model"></param>
        protected async void SelectCompany(CompanyBrokerDTO model)
        {
            UserInfo.CompanyBrokerID = model.CompanyBrokerID;
            UserInfo.CompanyID = model.CompanyID;
            UserInfo.CompanyName = model.CompanyName;
            UserInfo.BrokerID = model.BrokerID;
            UserInfo.BrokerName = model.BrokerName;
            UserInfo.AccountNumber = model.AccountNumber;
            await _cacheService.SetCacheValueAsync(UserInfo.UserName, UserInfo.ToJson());

            //setup alert to display
            _alertService.AddMessage($"Company Changed to {model.CompanyName}");

            Navigate();
        }
    }
}