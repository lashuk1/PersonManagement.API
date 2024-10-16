namespace PersonManagement.Application.DTOs;

public class AssociatedPersonDto
{
    public int Id { get; set; }
    public int AssociatedPersonId { get; set; }
    public ConnectionType ConnectionType { get; set; }
}
