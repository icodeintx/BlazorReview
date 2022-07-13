using Microsoft.AspNetCore.Authorization;

namespace WebsiteBlazor
{
    public static class Policies
    {
        public const string AccountSpecialist = "AccountSpecialist";
        public const string Administrator = "Administrator";
        public const string CompanyOwner = "CompanyOwner";
        public const string Guest = "Guest";

        
    }
}