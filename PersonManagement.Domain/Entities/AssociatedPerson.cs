namespace PersonManagement.Domain.Entities;

public class AssociatedPerson : Entity
{
    public ConnectionType ConnectionType { get; set; }
    public int AssociatedPersonId { get; set; }
    public Person Associated { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }
}