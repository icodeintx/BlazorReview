using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WebsiteBlazor.Services;

namespace WebsiteBlazor.Infrastructure;

public class AddRolesClaimsTransformation : IClaimsTransformation
{
    private readonly UserService _userService;

    public AddRolesClaimsTransformation(UserService userService)
    {
        _userService = userService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Clone current identity
        var clone = principal.Clone();
        var newIdentity = (ClaimsIdentity)clone.Identity;

        // Support AD and local accounts
        var nameId = principal.Claims.FirstOrDefault(x => x.Type == "name").Value;

        if (nameId == null)
        {
            return principal;
        }

        // Get user from database
        var user = await _userService.GetUserByUserName(nameId);
        if (user == null)
        {
            return principal;
        }

        // Add role claims to cloned identity
        foreach (var userRole in user.UserRoles)
        {
            var claim = new Claim("role", userRole.RoleName);
            newIdentity.AddClaim(claim);
        }

        return clone;
    }
}