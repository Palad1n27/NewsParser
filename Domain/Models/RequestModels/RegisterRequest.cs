using Domain.Models.DbModels;

namespace Domain.Models.RequestModels;

public class RegisterRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}