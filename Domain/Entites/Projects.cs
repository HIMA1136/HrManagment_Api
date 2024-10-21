namespace Domain.Entites;

public class Projects:BaseEntity
{
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
