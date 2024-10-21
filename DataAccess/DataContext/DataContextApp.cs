using Domain.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext;

public class DataContextApp : IdentityDbContext<AppUser>
{
    public DataContextApp(DbContextOptions<DataContextApp> options) : base(options)
    {
    }
    public DbSet<Projects> Projects { get; set; }
    public DbSet<Company> Company { get; set; }
}