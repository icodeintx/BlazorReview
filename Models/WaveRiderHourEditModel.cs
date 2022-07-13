
namespace WebsiteBlazor.Models
{
    public class WaveRiderHourEditModel
    {
        public int ChannelWidth { get; set; }
        public int CurveIntervals { get; set; }
        public int EngineDelay { get; set; }
        public string Instrument { get; set; } = "EUR_USD";
        public decimal LongZone { get; set; }
        public decimal MaxChannelHeight { get; set; }
        public decimal MaxSpread { get; set; }
        public int MinChannelHeight { get; set; }
        public decimal ProfitStrategy { get; set; }
        public decimal ShortZone { get; set; }
        public decimal StopLossMultiplier { get; set; }
        public int TradeExpirationMinutes { get; set; }
        public string Name { get; set; }
        public int Hour { get; set;  }

    }
}