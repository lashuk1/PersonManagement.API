namespace PersonManagement.Application.DTOs;

public class PhoneNumberDto
{
    public PhoneNumberType PhoneNumberType { get; set; }
    public string Number { get; set; }
}

public class PhoneNumberDtoValidator : AbstractValidator<PhoneNumberDto>
{
    public PhoneNumberDtoValidator()
    {
        RuleFor(p => p.Number)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d+$").WithMessage("Phone number must contain only digits and can start with a plus sign.");
    }
}
