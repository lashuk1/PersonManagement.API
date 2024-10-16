namespace PersonManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService) : ControllerBase
{
    private readonly IPersonService _personService = personService;

    #region CRUD Operations

    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] PersonCreateDto personCreateDto)
    {
        var result = await _personService.AddPersonAsync(personCreateDto);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetPersonById), new { id = result.Data }, result);
        }

        return BadRequest(result.Message);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPersonById(int id)
    {
        var result = await _personService.GetPersonByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound(result.Message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto personUpdateDto)
    {
        if (id != personUpdateDto.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var result = await _personService.UpdatePersonAsync(personUpdateDto);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound(result.Message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var result = await _personService.DeletePersonAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Message);
        }

        return NotFound(result.Message);
    }

    #endregion

    #region Association Management

    [HttpPost("{personId}/associate")]
    public async Task<IActionResult> AddAssociatedPerson(int personId, [FromBody] AssociatedPersonCreateDto associatedPersonCreateDto)
    {
        if (personId != associatedPersonCreateDto.PersonId)
        {
            return BadRequest("Person ID mismatch.");
        }

        var result = await _personService.AddAssociatedPersonAsync(associatedPersonCreateDto);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.Message);
    }

    [HttpDelete("{personId}/associate/{associatedPersonId}")]
    public async Task<IActionResult> RemoveAssociatedPerson(int personId, int associatedPersonId)
    {
        var result = await _personService.RemoveAssociatedPersonAsync(personId, associatedPersonId);
        if (result.IsSuccess)
        {
            return Ok(result.Message);
        }

        return NotFound(result.Message);
    }

    #endregion

    #region Photo Management

    [HttpPost("{personId}/photo")]
    public async Task<IActionResult> UpdatePersonPhoto(int personId, IFormFile photoFile)
    {
        if (photoFile == null || photoFile.Length == 0)
        {
            return BadRequest("Photo file is required.");
        }

        var result = await _personService.UpdatePersonPhotoAsync(personId, photoFile);
        if (result.IsSuccess)
        {
            return Ok(new { FilePath = result.Data });
        }

        return NotFound(result.Message);
    }

    #endregion

    #region Reports

    [HttpGet("report/associations")]
    public async Task<IActionResult> GetPersonAssociationReport()
    {
        var report = await _personService.GetPersonAssociationReportAsync();
        return Ok(report);
    }

    #endregion
}