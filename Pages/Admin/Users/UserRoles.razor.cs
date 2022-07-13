using Microsoft.AspNetCore.Components;

namespace WebsiteBlazor.Pages.Admin
{
    public partial class UserRoles : ComponentBase
    {
        [Parameter]
        public string userName { get; set; }

        public void GetRoles()
        {
        }
    }
}