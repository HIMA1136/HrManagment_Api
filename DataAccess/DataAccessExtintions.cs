using DataAccess.DataContext;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Config;
public static class DataAccessExtensions
{
    public static void ConfigureDataAccess(this IServiceCollection services, string connectionString)
    {

        services.AddDbContext<DataContextApp>(options =>
                    options.UseSqlServer(connectionString));

        services.AddIdentityCore<AppUser>(a =>
        {
            a.Password.RequireNonAlphanumeric = false;
            a.Password.RequireUppercase = false;
            a.Password.RequireDigit = false;
            a.Password.RequireLowercase = false;
            a.Password.RequiredLength = 5;
        }).AddEntityFrameworkStores<DataContextApp>();
    }
}
