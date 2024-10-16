using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonManagement.Application.Common;
using PersonManagement.Application.DTOs;
using PersonManagement.Application.Interfaces;
using PersonManagement.Domain.Entities;
using PersonManagement.Infrastructure.Repositories.Interfaces;

namespace PersonManagement.Application.Services;

public class PersonService(IUnitOfWork unitOfWork, IMapper mapper) : IPersonService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    #region CRUD Operations

    public async Task<Result<int>> AddPersonAsync(PersonCreateDto dto)
    {
        var person = _mapper.Map<Person>(dto);

        await _unitOfWork.PersonRepository.AddAsync(person);
        await _unitOfWork.SaveChangesAsync();

        return Result<int>.Success(person.Id, "Person added successfully.");
    }

    public async Task<Result<PersonGetFullDto>> GetPersonByIdAsync(int id)
    {
        var person = await _unitOfWork.PersonRepository
            .SingleOrDefaultAsync(p => p.Id == id, p => p.PhoneNumbers, p => p.AssociatedPersons);

        if (person == null)
            return Result<PersonGetFullDto>.Failure("Person not found.");

        var personDto = new PersonGetFullDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Gender = person.Gender,
            PersonalNumber = person.PersonalNumber,
            BirthDate = person.BirthDate,
            CityId = person.CityId,
            PhotoPath = person.PhotoPath,
            PhoneNumbers = person.PhoneNumbers.Select(pn => new PhoneNumberDto
            {
                PhoneNumberType = pn.PhoneNumberType,
                Number = pn.Number
            }).ToList(),
            AssociatedPersons = person.AssociatedPersons.Select(ap => new AssociatedPersonDto
            {
                ConnectionType = ap.ConnectionType
            }).ToList()
        };

        return Result<PersonGetFullDto>.Success(personDto);
    }

    public async Task<Result<PersonGetDto>> UpdatePersonAsync(PersonUpdateDto dto)
    {
        var person = await _unitOfWork.PersonRepository.SingleOrDefaultAsync(p => p.Id == dto.Id, include => include.PhoneNumbers);
        if (person == null)
            return Result<PersonGetDto>.Failure("Person not found.");

        person.FirstName = dto.FirstName;
        person.LastName = dto.LastName;
        person.Gender = dto.Gender;
        person.PersonalNumber = dto.PersonalNumber;
        person.BirthDate = dto.BirthDate;
        person.CityId = dto.CityId;

        var phoneNumbersToRemove = person.PhoneNumbers
            .Where(pn => !dto.PhoneNumbers.Any(dtoPn => dtoPn.Number == pn.Number))
            .ToList();

        _unitOfWork.PhoneNumberRepository.RemoveRange(phoneNumbersToRemove);

        foreach (var dtoPhoneNumber in dto.PhoneNumbers)
        {
            var existingPhoneNumber = person.PhoneNumbers
                .FirstOrDefault(pn => pn.Number == dtoPhoneNumber.Number);

            if (existingPhoneNumber != null)
            {
                existingPhoneNumber.PhoneNumberType = dtoPhoneNumber.PhoneNumberType;
            }
            else
            {
                var newPhoneNumber = new PhoneNumber
                {
                    Number = dtoPhoneNumber.Number,
                    PhoneNumberType = dtoPhoneNumber.PhoneNumberType,
                    PersonId = person.Id
                };
                person.PhoneNumbers.Add(newPhoneNumber);
            }
        }

        _unitOfWork.PersonRepository.Update(person);
        await _unitOfWork.SaveChangesAsync();

        var resultDto = new PersonGetDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Gender = person.Gender,
            PersonalNumber = person.PersonalNumber,
            BirthDate = person.BirthDate,
            CityId = person.CityId,
            PhoneNumbers = person.PhoneNumbers.Select(pn => new PhoneNumberDto
            {
                PhoneNumberType = pn.PhoneNumberType,
                Number = pn.Number
            }).ToList()
        };

        return Result<PersonGetDto>.Success(resultDto, "Person updated successfully.");
    }

    public async Task<Result<string>> DeletePersonAsync(int id)
    {
        var person = await _unitOfWork.PersonRepository.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            return Result<string>.Failure("Person not found.");

        _unitOfWork.PersonRepository.Remove(person);
        await _unitOfWork.SaveChangesAsync();

        return Result<string>.Success("Person deleted successfully.");
    }

    #endregion

    #region Association Management

    public async Task<Result<string>> RemoveAssociatedPersonAsync(int personId, int associatedPersonId)
    {
        var person = await _unitOfWork.PersonRepository.SingleOrDefaultAsync(p => p.Id == personId, p => p.AssociatedPersons);
        if (person == null)
            return Result<string>.Failure("Person not found.");

        var associatedPerson = person.AssociatedPersons.FirstOrDefault(ap => ap.AssociatedPersonId == associatedPersonId);
        if (associatedPerson == null)
            return Result<string>.Failure("Associated person not found for this person.");

        person.AssociatedPersons.Remove(associatedPerson);
        await _unitOfWork.SaveChangesAsync();

        return Result<string>.Success("Association removed successfully.");
    }

    public async Task<Result<AssociatedPersonDto>> AddAssociatedPersonAsync(AssociatedPersonCreateDto dto)
    {
        var person = await _unitOfWork.PersonRepository.SingleOrDefaultAsync(p => p.Id == dto.PersonId, p => p.AssociatedPersons);
        if (person == null)
            return Result<AssociatedPersonDto>.Failure("Person not found.");

        var associatedPerson = await _unitOfWork.PersonRepository.GetByIdAsync(dto.AssociatedPersonId);
        if (associatedPerson == null)
            return Result<AssociatedPersonDto>.Failure("Associated person not found.");

        if (person.AssociatedPersons.Any(ap => ap.AssociatedPersonId == dto.AssociatedPersonId))
            return Result<AssociatedPersonDto>.Failure("This association already exists.");

        var newAssociation = new AssociatedPerson
        {
            PersonId = dto.PersonId,
            AssociatedPersonId = dto.AssociatedPersonId,
            ConnectionType = dto.ConnectionType
        };

        person.AssociatedPersons.Add(newAssociation);
        await _unitOfWork.SaveChangesAsync();

        var resultDto = new AssociatedPersonDto
        {
            Id = newAssociation.Id,
            AssociatedPersonId = newAssociation.AssociatedPersonId,
            ConnectionType = newAssociation.ConnectionType
        };

        return Result<AssociatedPersonDto>.Success(resultDto, "Associated person added successfully.");
    }

    #endregion

    #region Photo Management

    public async Task<Result<string>> UpdatePersonPhotoAsync(int personId, IFormFile photoFile)
    {
        var person = await _unitOfWork.PersonRepository.GetByIdAsync(personId);
        if (person == null)
            return Result<string>.Failure("Person not found.");

        var uploadsDirectory = Path.Combine("wwwroot", "uploads", "photos");
        if (!Directory.Exists(uploadsDirectory))
            Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"{Guid.NewGuid()}_{photoFile.FileName}";
        var filePath = Path.Combine(uploadsDirectory, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await photoFile.CopyToAsync(stream);
        }

        person.PhotoPath = fileName;
        _unitOfWork.PersonRepository.Update(person);
        await _unitOfWork.SaveChangesAsync();

        return Result<string>.Success(fileName, "Photo updated successfully.");
    }

    #endregion

    #region Reports

    public async Task<List<PersonAssociationReportDto>> GetPersonAssociationReportAsync()
    {
        var persons = await _unitOfWork.PersonRepository
            .GetAll()
            .Include(p => p.AssociatedPersons)
            .ToListAsync();

        var report = persons.Select(person => new PersonAssociationReportDto
        {
            PersonId = person.Id,
            FullName = $"{person.FirstName} {person.LastName}",
            AssociatedPersonCountByType = person.AssociatedPersons
                .GroupBy(ap => ap.ConnectionType)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                )
        }).ToList();

        return report;
    }

    #endregion
}

