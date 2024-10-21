using Domain.Entites;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.DataContext.Seed;

public class DataSeed
{
    public async Task SeedAsync(DataContextApp dataContext, UserManager<AppUser> _userManager, int retry = 0)
    {
        int retryForAvailability = retry;

        try
        {
            dataContext.Database.EnsureCreated();
            await SeedUsersAsync(dataContext, _userManager);
            //await SeedFinancialSettingAsync(dataContext);
        }
        catch(Exception ex) 
        {
            //if (retryForAvailability < 10)
            //{
            //    retryForAvailability++;
            //    await SeedAsync(dataContext, _userManager, retryForAvailability);
            //}
            throw;
        }
    }
    private static async Task SeedUsersAsync(DataContextApp DataContext, UserManager<AppUser> _userManager)
    {
        
        if (DataContext.Users.Any())
            return;

        var user = new AppUser
        {
            Email = "admin@admin.com",
            UserName = "Admin",
            FullName = "Admin",
            TypeAccountText = "مسئول",
            TypeAccountValue = 1,

        };
       var ss=  await _userManager.CreateAsync(user, "Admin");
        await DataContext.SaveChangesAsync();
    }
}
