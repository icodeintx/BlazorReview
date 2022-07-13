using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebsiteBlazor.Pages.WaveRider
{
    public partial class WaveRiderHours : ComponentBase
    {
        public List<WaveRiderHourDTO> Hours { get; set; } = new();

        [ParameterAttribute]
        public string Name { get; set; }

        [Inject]
        protected AppConfig AppConfig { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected async Task GetHours()
        {
            //TODO Why is this calling the wrapper?  It should be using a WaveRiderService
            var CurrentUser = await _userService.GetCurrentUserName();
            AmaraCode.RainMaker.DataServiceWrapper.WaveRiderWrapper wrWrapper = new(CurrentUser, AppConfig.DataSericeURL, AppConfig.RmApiKey);
            Hours = await wrWrapper.GetWaveRiderHours(Name);
        }

        /// <summary>
        /// Used to toggle "class" (background color) in razor
        /// </summary>
        /// <param name="currentHour"></param>
        /// <param name="utcHour"></param>
        /// <returns></returns>
        protected string IsActiveHour(int currentHour, int utcHour)
        {
            if (currentHour == utcHour)
            {
                return "row-highlight";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void NavigateBack()
        {
            navigationManager.NavigateTo("/waverider/list");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hour"></param>
        protected void NavigateToEditHour(string name, int hour)
        {
            //waverider/edit/name/{name}/hour/{hour}
            navigationManager.NavigateTo($"/waverider/edit/name/{name}/hour/{hour}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => GetHours());
        }

        /// <summary>
        /// Include this method if the screen should be refreshed
        /// like when coming back from db insert (cqrs)
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldRender()
        {
            var renderUI = true;

            return renderUI;
        }
    }
}