namespace PersonManagement.Application.Interfaces;

public interface IPersonService
{
    #region CRUD Operations

    Task<Result<int>> AddPersonAsync(PersonCreateDto dto);
    Task<Result<PersonGetFullDto>> GetPersonByIdAsync(int id);
    Task<Result<PersonGetDto>> UpdatePersonAsync(PersonUpdateDto dto);
    Task<Result<string>> DeletePersonAsync(int id);

    #endregion

    #region Association Management

    Task<Result<AssociatedPersonDto>> AddAssociatedPersonAsync(AssociatedPersonCreateDto dto);
    Task<Result<string>> RemoveAssociatedPersonAsync(int personId, int associatedPersonId);

    #endregion

    #region Photo Management

    Task<Result<string>> UpdatePersonPhotoAsync(int personId, IFormFile photoFile);

    #endregion

    #region Reports

    Task<List<PersonAssociationReportDto>> GetPersonAssociationReportAsync();

    #endregion
}