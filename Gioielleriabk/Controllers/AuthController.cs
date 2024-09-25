
using Gioielleriabk.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Gioielleriabk.Models;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public AuthController(UserService userService)
    {
        _userService = userService;
        _tokenService = new TokenService();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!Enum.TryParse(request.Role, out Role role))
        {
            return BadRequest("Il ruolo specificato non è valido.");
        }

        // Passa il ruolo a RegisterUserAsync
        var success = await _userService.RegisterUserAsync(request.Nome, request.Email, request.Password, role);
        if (!success)
        {
            return BadRequest(new { message = "Email già registrata" });
        }

        return Ok(new { message = "Registrazione avvenuta con successo" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.AuthenticateUserAsync(request.Email, request.Password);

        if (user == null)
        {
            return BadRequest(new { message = "Email o password non validi" });
        }

        var token = _tokenService.GenerateToken(user);

        Console.WriteLine($"User Role: {user.Role}");

        return Ok(new
        {
            user = new
            {
                nome = user.Nome,
                email = user.Email,
                role = user.Role
            },
            token = token
        });
    }
}
