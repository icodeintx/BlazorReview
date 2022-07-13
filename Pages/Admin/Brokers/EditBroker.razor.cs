using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Brokers
{
    public partial class EditBroker : ComponentBase
    {
        public BrokerDisplayModel Broker { get; set; } = new();

        [ParameterAttribute]
        public string Id { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        public string UserName { get; set; }

        [Inject]
        protected AlertService AlertService { get; set; }

        [Inject]
        protected BrokerService BrokerService { get; set; }

        [Inject]
        protected UserService UserService { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task GetBroker()
        {
            Broker = await BrokerService.GetBroker(Guid.Parse(Id));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task UpdateBroker()
        {
            try
            {
                await BrokerService.UpdateBroker(Broker);

                Toast.DisplaySuccessToast($"Broker {Broker.Name} has been Updated");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }

            //navigationManager.NavigateTo($"/admin/broker/list", true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => GetUserName());

            await Task.Run(() => GetBroker());
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