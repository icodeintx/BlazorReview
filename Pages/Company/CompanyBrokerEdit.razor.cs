using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Company
{
    public partial class CompanyBrokerEdit : ComponentBase
    {
        /// <summary>
        /// Should be set when editing an entry.
        /// </summary>
        [ParameterAttribute]
        public string EditCompanyBrokerID { get; set; }

        public CompanyBrokerEditModel EditModel { get; set; } = new CompanyBrokerEditModel();

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        /// <summary>
        /// This model is used to populate and send back to the Wrapper service for
        /// saving to the database.
        /// </summary>
        private CompanyBrokerDTO CompanyBrokerToSave { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public void CancelCompanyBrokerEdit()
        {
            navigationManager.NavigateTo("/companybroker");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task EditCompanyBroker()
        {
            //get the companybroker and populate the form.
            var result = await CompanyService.GetCompanyBroker(Guid.Parse(EditCompanyBrokerID));

            EditModel.ApiKey = result.BrokerApiKey;
            EditModel.Name = result.Name;
            EditModel.CompanyBrokerID = result.CompanyBrokerID;
            EditModel.AccountNumber = result.AccountNumber;
            EditModel.SandBox = result.SandBox;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task SaveCompanyBroker()
        {
            try
            {
                if ((await _userService.GetCurrentUserName()).ToLower() == "guest")
                {
                    Toast.DisplaySuccessToast("Simulated for guest user");
                    return;
                }

                //get the latest info from the db.
                CompanyBrokerToSave = await CompanyService.GetCompanyBroker(EditModel.CompanyBrokerID);

                CompanyBrokerToSave.Name = EditModel.Name;
                CompanyBrokerToSave.BrokerApiKey = EditModel.ApiKey;
                CompanyBrokerToSave.AccountNumber = EditModel.AccountNumber;
                CompanyBrokerToSave.SandBox = EditModel.SandBox;

                //Send the model to the company servic that will call the wrapper
                await CompanyService.SaveCompanyBroker(CompanyBrokerToSave);

                Toast.DisplaySuccessToast("Company-Broker has been updated.");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //call the cancel method so we do not duplicate code
            CancelCompanyBrokerEdit();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await EditCompanyBroker();
        }
    }
}