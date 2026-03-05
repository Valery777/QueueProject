using Microsoft.AspNetCore.Mvc;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Infrastructure.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthController(IJwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // TODO: validate user from DB
        if (request.Username != "admin" || request.Password != "password")
            return Unauthorized();

        var token = _tokenGenerator.GenerateToken("1", request.Username);

        return Ok(new { token });
    }
}

