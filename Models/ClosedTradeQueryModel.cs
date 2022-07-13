namespace WebsiteBlazor.Models
{
    public class ClosedTradeQueryModel
    {
        public DateTime EndDate { get; set; }
        public int Month { get; set; }
        public int TakeCount { get; set; }
        public int TradeID { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
    }
}