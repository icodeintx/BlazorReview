using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBlazor.Models
{
    public class CompanyDisplayModel
    {
        [Required]
        public decimal ACPercent { get; set; } = 10.0m;

        public bool Active { get; set; } = true;
        public string Address { get; set; }
        public string City { get; set; }

        [Required]
        public Guid CompanyID { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string LogoFile { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }
        public string State { get; set; }

        [Required]
        public decimal TaxPercent { get; set; } = 18.0m;

        public string Zip { get; set; }
    }
}