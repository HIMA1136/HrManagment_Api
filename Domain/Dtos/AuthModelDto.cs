namespace Domain.Dtos;

public class AuthModelDto
{
    public string Token { get; set; }
    public bool IsAuthencated { get; set; }
    public DateTime ExpiresOn { get; set; }
    public UserDataDto User { get; set; }
}
