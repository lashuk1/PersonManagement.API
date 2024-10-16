namespace PersonManagement.Application.DTOs;

public class AssociatedPersonCreateDto
{
    public int PersonId { get; set; }
    public int AssociatedPersonId { get; set; }
    public ConnectionType ConnectionType { get; set; }
}
