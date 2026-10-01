using Microsoft.AspNetCore.Identity;

namespace Web2_API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}