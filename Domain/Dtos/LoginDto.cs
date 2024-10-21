using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "RequiredErrorMessage")]
    public string UserName { get; set; }
    [DataType(DataType.Password), Required(ErrorMessage = "RequiredErrorMessage")]
    public string Password { get; set; } = string.Empty;
}
