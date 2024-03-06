namespace TradeBotPro.App.Services.Interfaces;

public interface IAuthenticationService
{
    string HashPassword(string password);
    bool IsPasswordVerified(string hash, string password);
}
