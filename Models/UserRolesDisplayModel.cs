using System.Collections.Generic;
using System.Security.Claims;

namespace WebsiteBlazor.Models
{
    public class UserRolesDisplayModel
    {
        public List<string> Roles { get; set; }
        public List<Claim> UserClaims { get; set; }
        public string UserName { get; set; }
    }
}