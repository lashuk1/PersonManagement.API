namespace PersonManagement.Application.DTOs;

public class PersonListDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime BirthDate { get; set; }
}
