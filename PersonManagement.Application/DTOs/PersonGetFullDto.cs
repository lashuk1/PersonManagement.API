﻿namespace PersonManagement.Application.DTOs;

public class PersonGetFullDto
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Gender Gender { get; set; }

    public string PersonalNumber { get; set; }

    public DateTime BirthDate { get; set; }

    public int? CityId { get; set; }

    public string PhotoPath { get; set; }

    public ICollection<PhoneNumberDto> PhoneNumbers { get; set; }

    public ICollection<AssociatedPersonDto> AssociatedPersons { get; set; }
}
