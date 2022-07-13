using System.Collections.Generic;

namespace WebsiteBlazor.Models
{
    public class AppConfig
    {
        /// <summary>
        ///
        /// </summary>
        public AppConfig()
        {
            UserGroups = new Dictionary<string, string>();
        }

        public string DataSericeURL { get; set; }
        public string RmApiKey { get; set; }
        public string Redis { get; set; }
        public Dictionary<string, string> UserGroups { get; set; }
    }
}