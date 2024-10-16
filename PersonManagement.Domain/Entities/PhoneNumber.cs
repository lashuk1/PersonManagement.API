namespace PersonManagement.Domain.Entities;

public class PhoneNumber : Entity
{
    public PhoneNumberType PhoneNumberType { get; set; } 
    public string Number { get; set; } 
    public int PersonId { get; set; } 
    public Person Person { get; set; }
}