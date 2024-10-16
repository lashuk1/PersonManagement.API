namespace PersonManagement.Domain.Entities;

public class Entity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
}
