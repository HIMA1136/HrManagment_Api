namespace Domain.Entites;

public class Company:BaseEntity
{
    public ICollection<Projects> projects { get; set; }
}
