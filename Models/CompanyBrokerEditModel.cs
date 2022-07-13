using System;

namespace WebsiteBlazor.Models
{
    public class CompanyBrokerEditModel
    {
        public string AccountNumber { get; set; }
        public string ApiKey { get; set; }
        public Guid CompanyBrokerID { get; set; }
        public string Name { get; set; }
        public bool SandBox { get; set; }
    }
}