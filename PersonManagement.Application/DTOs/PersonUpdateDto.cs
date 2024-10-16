using System.ComponentModel.DataAnnotations;

namespace PersonManagement.Application.DTOs;

public class PersonUpdateDto
{
    public int Id { get; set; } 

    [MinLength(2), MaxLength(50)]
    public required string FirstName { get; set; }

    [MinLength(2), MaxLength(50)]
    public required string LastName { get; set; }

    public required Gender Gender { get; set; }

    [MinLength(11), MaxLength(11)]
    public required string PersonalNumber { get; set; }

    public required DateTime BirthDate { get; set; }

    public int? CityId { get; set; }

    public ICollection<PhoneNumberDto> PhoneNumbers { get; set; }
}

public class PersonUpdateDtoValidator : AbstractValidator<PersonUpdateDto>
{
    public PersonUpdateDtoValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage("Person ID is required and must be greater than zero.");

        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("First name must be at most 50 characters.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("Last name must be at most 50 characters.");

        RuleFor(p => p.Gender)
            .IsInEnum().WithMessage("Gender must be a valid enum value.");

        RuleFor(p => p.PersonalNumber)
            .NotEmpty().WithMessage("Personal number is required.")
            .Length(11).WithMessage("Personal number must be exactly 11 characters.");

        RuleFor(p => p.BirthDate)
           .NotEmpty().WithMessage("Birth date is required.")
           .LessThan(DateTime.Now).WithMessage("Birth date must be in the past.")
           .Must(BeAtLeast18YearsOld).WithMessage("You must be at least 18 years old.");

        RuleForEach(p => p.PhoneNumbers).SetValidator(new PhoneNumberDtoValidator());
    }

    private bool BeAtLeast18YearsOld(DateTime birthDate)
    {
        return birthDate <= DateTime.Now.AddYears(-18);
    }
}