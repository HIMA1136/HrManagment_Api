using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Domain.Entites;

public class AppUser:IdentityUser
{

    public string FullName { get; set; }
    public string CreatedById { get; set; } = "server";
    public string CreatedByName { get; set; } = "server";
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public int TypeAccountValue { get; set; }
    public string TypeAccountText { get; set; }
    public int EmployeeId { get; set; }
    public bool IsDeleted { get; set; }

    public int RoleGroupsId { get; set; }


}
