using AmaraCode.RainMaker.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace WebsiteBlazor.Pages.WaveRider
{
    public partial class WaveRiderConfig : ComponentBase
    {
        protected string configResult;

        [ParameterAttribute]
        public string Name { get; set; } = "Svetlana";

        protected async Task<string> GetConfig()
        {
            var result = await _traderService.GetWaveRiderConfig(Name);

            return result.ToJson();
        }

        protected override async Task OnInitializedAsync()
        {
            configResult = await Task.Run(() => GetConfig());
        }
    }
}