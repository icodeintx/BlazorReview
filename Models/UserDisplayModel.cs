using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBlazor.Models
{
    public class UserDisplayModel : IdentityUser
    {
        [Required]
        public string Address { get; set; }

        public string City { get; set; }

        public string CreateUserPassword { get; set; }

        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string State { get; set; }

        public Guid userId { get; set; }

        [Required]
        public string Zip { get; set; }
    }
}