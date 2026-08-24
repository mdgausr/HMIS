using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IPatientRepository _repo;
    public PatientController(IPatientRepository repo) => _repo = repo;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePatientDto dto)
    {
        var id = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }
}

public record CreatePatientDto(string FirstName, string LastName, string Gender, DateTime Dob);
