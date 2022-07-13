using System;
using System.ComponentModel.DataAnnotations;
using WebsiteBlazor.Infrastructure;

namespace WebsiteBlazor.Models
{
    public class WaveRiderDisplayModel
    {
        //public string AccountNumber { get; set; }
        public decimal AccountUsageAmount { get; set; } = 20.0m;

        public bool Active { get; set; } = false;

        public string BrokerName { get; set; }

        [Required]
        [NotEmptyGuid]
        public Guid CompanyBrokerID { get; set; }

        public string CompanyName { get; set; }

        //Marked private to be used as default for all new WaveRiders
        [Required]
        public string Instrument { get; private set; } = "EUR_USD";

        [Required]
        public string Name { get; set; }

        public decimal ScrapeAmount { get; set; } = 10000m;

        //public Guid EngineTypeID { get; set; }
        public bool Shutdown { get; set; }

        //Marked private to be used as default for all new WaveRiders
        [Required]
        public int SignificantDigit { get; private set; } = 4;

        public Guid WaveRiderID { get; set; }
    }
}