namespace PersonManagement.Application.DTOs;

public class PersonAssociationReportDto
{
    public int PersonId { get; set; }
    public string FullName { get; set; }
    public Dictionary<ConnectionType, int> AssociatedPersonCountByType { get; set; }
}