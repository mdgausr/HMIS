using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AbdmController : ControllerBase
{
    private readonly IAbdmClient _client;
    public AbdmController(IAbdmClient client) => _client = client;

    [HttpPost("verify")]
    public async Task<IActionResult> Verify(VerifyRequest req)
    {
        var result = await _client.VerifyHealthIdAsync(req.HealthId);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result);
    }
}

public record VerifyRequest(string HealthId);
