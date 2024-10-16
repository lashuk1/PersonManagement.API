namespace PersonManagement.Domain.Entities;

public class Person : Entity
{
    [MinLength(2), MaxLength(50)]
    public required string FirstName { get; set; }

    [MinLength(2), MaxLength(50)]
    public required string LastName { get; set; }

    public required Gender Gender { get; set; }

    [MinLength(11),MaxLength(11)]
    public required string PersonalNumber { get; set; }

    public required DateTime BirthDate { get; set; }

    public string? PhotoPath { get; set; }

    public int? CityId { get; set; }

    public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    public ICollection<AssociatedPerson> AssociatedPersons { get; set; }
}