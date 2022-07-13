using AmaraCode.RainMaker.Models;
using System;
using System.Collections.Generic;

namespace WebsiteBlazor.Models
{
    public class UserInfo : ICanJson
    {
        public UserInfo()
        {
        }

        /// <summary>
        /// Added on 12/8/2021 - This account number will get stored
        /// in REDIS so when actions like GetTemperature are called we
        /// will not have to do an additional LookUp to get the AccountNumber.
        /// This is set when Selecting Company
        /// </summary>
        public string AccountNumber { get; set; }

        public Guid BrokerID { get; set; }
        public string BrokerName { get; set; }
        public Guid CompanyBrokerID { get; set; }
        public Guid CompanyID { get; set; }
        public string CompanyName { get; set; }
        public Guid UserId { get; set; }

        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}