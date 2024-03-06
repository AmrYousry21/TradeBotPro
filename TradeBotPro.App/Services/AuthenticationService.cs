using TradeBotPro.App.Services.Interfaces;

namespace TradeBotPro.App.Services;

public class AuthenticationService : IAuthenticationService
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool IsPasswordVerified(string hash, string password) => BCrypt.Net.BCrypt.Verify(password, hash);
}