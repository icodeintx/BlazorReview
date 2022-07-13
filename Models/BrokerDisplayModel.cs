using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteBlazor.Models
{
    public class BrokerDisplayModel
    {
        public Guid BrokerID {  get; set; }
        [Required]
        public string Name {  get; set; }

        [Required]
        public string Alias {  get; set; }

        [Required]
        public string ApiUrl {  get; set; }
        public string SanboxApiUrl { get; set; }
        public string LogoFile { get; set; }
        public bool Active { get; set; }

    }
}
