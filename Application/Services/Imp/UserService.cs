using Application.Helpers.JwtConfingrations;
using Application.Model.Common;
using Application.Services.Interfaces;
using AutoMapper;
using DataAccess.DataContext;
using Domain.Dtos;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Services.Imp;

public class UserService:IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtUtils _jwtUtil;
    private readonly IMapper _mapper;
    private readonly DataContextApp _context;
    public UserService( UserManager<AppUser> userManager, DataContextApp context, IJwtUtils jwtUtil, IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _jwtUtil = jwtUtil;
        _mapper = mapper;
    }
    public async Task<ResponseResult> Login(LoginDto model)
    {

        var user = await _userManager.FindByNameAsync(model.UserName);
        user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == model.UserName);

        if (user == null)
            return new ResponseResult { IsSuccssed = false, Message = "البيانات المدخلة غير صحيحة" };

        if (user.IsDeleted == true)
            return new ResponseResult { IsSuccssed = false, Message = "لا يوجد مستخدم بهذا الاسم " };

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return new ResponseResult { IsSuccssed = false, Message = "البيانات المدخلة غير صحيحة" };
        var userData = _mapper.Map<UserDataDto>(user);

        var jwtSecurityToken = await _jwtUtil.CreateJwtToken(user);
        var auth = new AuthModelDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthencated = true,
            User = userData,
        };


        return new ResponseResult { IsSuccssed = true, Message = "تم تسجيل الدخول بنجاح", Obj = auth };
    }

    public async Task<ResponseResult> GetUserByUserId(string UserId)
    {

        var user = await _userManager.FindByIdAsync(UserId);

        if (user == null)
            return new ResponseResult { IsSuccssed = false, Message = "الحساب غير موجود" };

        var userData = _mapper.Map<UserDataDto>(user);

        return new ResponseResult { IsSuccssed = true, Message = "تم تسجيل الدخول بنجاح", Obj = userData };
    }


    public async Task<ResponseResult> Register(RegisterDto registerDTO)
    {


        if (await _userManager.FindByNameAsync(registerDTO.UserName) != null)
            return new ResponseResult { IsSuccssed = false, Message = "اسم المستخدم مأخوذ من قبل" };




        var user = new AppUser
        {
            UserName = registerDTO.UserName,
            FullName = registerDTO.UserName,
            TypeAccountText = registerDTO.UserName,


        };
        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
        {
            var error = string.Empty;
            foreach (var item in result.Errors)
            {
                error += $"{item.Description},";
            }
            return new ResponseResult { IsSuccssed = false, Message = error };
        }
        var jwtSecurityToken = await _jwtUtil.CreateJwtToken(user);
        var auth = new AuthModelDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthencated = true,

        };

        return new ResponseResult { IsSuccssed = true, Message = "تم الإضافة بنجاح", Obj=auth };

    }

    public async Task<ResponseResult> DeleteUser(string UserName, string Passward)
    {
        var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName== UserName);
        if (await _userManager.CheckPasswordAsync(user, Passward))
        {
            user.IsDeleted = true;
        }
        else
        {
            return new ResponseResult { IsSuccssed = false, Message = "البيانات المدخلة غير صحيحة" };
        }
        _context.SaveChanges();

        return new ResponseResult { IsSuccssed = true, Message = "تم حذف الحساب بنجاح" };
    }

}
