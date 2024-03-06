using TradeBotPro.App.Models.DataModels;
namespace TradeBotPro.App.Models.ViewModels;

public class ClientViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ClientStatusEnum Status { get; set; }
    public string RegistrationKey { get; set; }
    public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();

    public static explicit operator ClientViewModel(Client client)
    {
        var clientViewModel = new ClientViewModel
        {
            Id = client.Id,
            Name = client.Name,
            RegistrationKey = client.Registrationkey,
            Status = client.Status
        };

        foreach (var user in client.Users)
        {
            clientViewModel.Users.Add((UserViewModel)user);
        }

        return clientViewModel;
    }
}