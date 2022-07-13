using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebsiteBlazor.Models;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin.Brokers
{
    public partial class CreateBroker : ComponentBase
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
        public async Task Create()
        {
            try
            {
                await BrokerService.CreateBroker(Broker);

                Toast.DisplaySuccessToast($"Broker {Broker.Name} has been created.");
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