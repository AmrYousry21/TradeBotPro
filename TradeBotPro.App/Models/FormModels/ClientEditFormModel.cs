using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TradeBotPro.App.Models.DataModels;

namespace TradeBotPro.App.Models.FormModels;

public class ClientEditFormModel : BaseFormModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Status { get; set; }

    public string RegistrationKey { get; set; }

    public IEnumerable<SelectListItem> StatusList
    {
        get
        {
            return Enum.GetNames(typeof(ClientStatusEnum))
                .Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                });
        }
    }

    public static explicit operator ClientEditFormModel(Client client)
    {
        return new ClientEditFormModel
        {
            Id = client.Id,
            Name = client.Name,
            Status = client.Status.ToString(),
            RegistrationKey = client.Registrationkey,
        };
    }
}