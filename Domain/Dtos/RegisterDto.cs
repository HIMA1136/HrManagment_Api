namespace Domain.Dtos;

public class RegisterDto
{
    public string Company_Name { get; set; }
    public int phoneNumber { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
