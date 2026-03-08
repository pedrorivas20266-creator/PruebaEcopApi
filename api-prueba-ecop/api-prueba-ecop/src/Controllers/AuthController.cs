using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace api_prueba_ecop.src.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _service;

    public AuthController(IUsuarioService service)
    {
        _service = service;
    }

    /// <summary>
    /// Endpoint para autenticar a un usuario y obtener un token de acceso.
    /// </summary>
    /// <param name="request">Datos para la autenticacion del usuario</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken token)
    {
        var response = await _service.Login(request, token);
        return Ok(response);
    }
}