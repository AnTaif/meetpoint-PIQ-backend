using AccountService.Models;

namespace AccountService.Providers;

public interface ITokenProvider
{ 
    string GenerateToken(User user, IEnumerable<string> roles);
}