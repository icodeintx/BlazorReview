using Microsoft.AspNetCore.Components;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Pages.Admin
{
    public partial class OandaInstruments : ComponentBase
    {
        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        private SystemService SystemService { get; set; }

        private UserInfo UserInfo { get; set; } = new();

        public async Task UpdateInstruments()
        {
            try
            {
                await SystemService.UpdateInstrments(UserInfo.CompanyBrokerID);

                Toast.DisplaySuccessToast("Instruments have been Updated");
            }
            catch (Exception ex)
            {
                Toast.ProcessError(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            UserInfo = await _userService.GetUserInfo();
        }
    }
}