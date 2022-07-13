using AmaraCode.RainMaker.Models;
using System;
using System.Text;

namespace WebsiteBlazor.Models
{
    public class WaveRiderHourDisplayModel : ICanJson
    {
        public int ChannelWidth { get; set; }
        public int CurveIntervals { get; set; }
        public int EngineDelay { get; set; }
        public int Hour { get; set; }
        public string Instrument { get; set; } = "EUR_USD";

        public decimal LocalHour
        {
            get
            {
                return GetLocalHour();
            }
        }

        public decimal LongZone { get; set; }
        public decimal MaxChannelHeight { get; set; }
        public decimal MaxSpread { get; set; }
        public int MinChannelHeight { get; set; }
        public string Name { get; set; }
        public decimal ProfitStrategy { get; set; }
        public decimal ShortZone { get; set; }
        public int SignificantDigit { get; set; }
        public decimal StopLossMultiplier { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var model = (WaveRiderHourDTO)obj;
                return
                    (ChannelWidth == model.ChannelWidth) &&
                    (CurveIntervals == model.CurveIntervals) &&
                    (EngineDelay == model.EngineDelay) &&
                    (Hour == model.Hour) &&
                    (Instrument == model.Instrument) &&
                    (LongZone == model.LongZone) &&
                    (MaxChannelHeight == model.MaxChannelHeight) &&
                    (MaxSpread == model.MaxSpread) &&
                    (MinChannelHeight == model.MinChannelHeight) &&
                    (ProfitStrategy == model.ProfitStrategy) &&
                    (ShortZone == model.ShortZone) &&
                    (StopLossMultiplier == model.StopLossMultiplier);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ChannelWidth.GetHashCode() ^
                CurveIntervals.GetHashCode() ^
                EngineDelay.GetHashCode() ^
                Hour.GetHashCode() ^
                Instrument.GetHashCode() ^
                LongZone.GetHashCode() ^
                MaxChannelHeight.GetHashCode() ^
                MaxSpread.GetHashCode() ^
                MinChannelHeight.GetHashCode() ^
                ProfitStrategy.GetHashCode() ^
                ShortZone.GetHashCode() ^
                StopLossMultiplier.GetHashCode();

            //return base.GetHashCode();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder line = new StringBuilder();

            line.AppendLine($"ChannelWidth = {ChannelWidth}");
            line.AppendLine($"CurveIntervals = {CurveIntervals}");
            line.AppendLine($"EngineDelay = {EngineDelay}");
            line.AppendLine($"Hour = {Hour}");
            line.AppendLine($"Instrument = {Instrument}");
            line.AppendLine($"SignificantDigit = {SignificantDigit}");
            line.AppendLine($"LongZone = {LongZone}");
            line.AppendLine($"ShortZone = {ShortZone}");
            line.AppendLine($"MaxChannelHeight = {MaxChannelHeight}");
            line.AppendLine($"MinChannelHeight = {MinChannelHeight}");
            line.AppendLine($"MaxSpread = {MaxSpread}");
            line.AppendLine($"ProfitStrategy = {ProfitStrategy}");
            line.AppendLine($"StopLossMultiplier = {StopLossMultiplier}");

            return line.ToString();
        }

        private int GetLocalHour()
        {
            var y = DateTime.UtcNow;
            var x = y.LocalHourFromUTC(this.Hour);
            return x;
        }
    }
}