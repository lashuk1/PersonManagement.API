using AutoMapper;
using PersonManagement.Application.DTOs;
using PersonManagement.Domain.Entities;

namespace PersonManagement.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PersonCreateDto, Person>()
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers))
            .ForMember(dest => dest.AssociatedPersons, opt => opt.Ignore()) 
            .ForMember(dest => dest.PhotoPath, opt => opt.Ignore());

        CreateMap<PhoneNumberDto, PhoneNumber>();
        CreateMap<Person, PersonGetFullDto>().ReverseMap();
        CreateMap<Person, PersonCreateDto>().ReverseMap();
        CreateMap<Person, PersonUpdateDto>().ReverseMap();

        CreateMap<AssociatedPerson, AssociatedPersonDto>().ReverseMap();
        CreateMap<AssociatedPerson, AssociatedPersonCreateDto>().ReverseMap();

        CreateMap<PhoneNumber, PhoneNumberDto>().ReverseMap();
    }
}
