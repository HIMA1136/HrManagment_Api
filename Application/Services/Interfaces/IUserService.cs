using Application.Model.Common;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<ResponseResult> GetUserByUserId(string UserId);
    Task<ResponseResult> DeleteUser(string UserName, string Passward);
    Task<ResponseResult> Login(LoginDto model);
    Task<ResponseResult> Register(RegisterDto registerDTO);

}
