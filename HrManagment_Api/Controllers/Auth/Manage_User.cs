using Application.Services.Interfaces;
using Domain.Dtos;
using HrManagment_Api.Helper;
using Microsoft.Win32;

namespace HrManagment_Api.Controllers.Auth;

public class Manage_UserController : IApiInterface
{
      public void RegisterEndPoint(WebApplication app)
{
    app.MapPost("/Login", Login);
    app.MapPost("/Register", Register);
    app.MapGet("/DeleteUser", DeleteUser);


}
private async Task<IResult> Login(LoginDto model, IUserService service) => Results.Ok(await service.Login(model));
private async Task<IResult> Register(RegisterDto model, IUserService service) => Results.Ok(await service.Register(model));

private async Task<IResult> DeleteUser(string password, string UserName, IUserService service) => Results.Ok(await service.DeleteUser(password, UserName));

}
