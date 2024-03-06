using TradeBotPro.App.Models.DataModels;
using TradeBotPro.App.Services.Interfaces;

namespace TradeBotPro.App.Models;

public class DataSeeder
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        // Ge Services
        var dbContext = serviceProvider.GetService<DatabaseContext>();
        var authenticationService = serviceProvider.GetService<IAuthenticationService>();

        var sysAdminExists = dbContext.Users.Any(x => x.Email == "essam.yousry1996@gmail.com");
        if (sysAdminExists)
            return;

        // Add User (System Admin)
        var systemAdmin = dbContext.Users.Add(new User
        {
            FirstName = "Essam",
            LastName = "Yousry",
            Email = "essam.yousry1996@gmail.com",
            Password = authenticationService.HashPassword("GUagj_24dfsl@"),
            UserRole = UserRoleEnum.Admin,
            Status = UserStatusEnum.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        dbContext.SaveChanges();
    }
}