using api_prueba_ecop.src.Application.Exceptions;
using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using api_prueba_ecop.src.Infraestructure.Entities;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace api_prueba_ecop.src.Application.Services;
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly ILogger<UsuarioService> _logger;
    private readonly IJwtTokenService _jwtService;
    private readonly IMapper _mapper;

    public UsuarioService(IUsuarioRepository repository, ILogger<UsuarioService> logger, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _jwtService = jwtTokenService;
        _mapper = mapper;
    }

    #region"Validacion de Inicio de Sesion"

    public async Task<LoginResponse> Login(LoginRequest request, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de validacion de inicio de sesion del usuario: {Usuario} ###", request.NombreUsuario);

        var usuario = await ValidarLogin(request.NombreUsuario, ConvertirClave(request.ClaveUsuario), token);

        var (tokenJwt, expiresIn) = GenerarToken(usuario);

        _logger.LogInformation("### Proceso de validacion de inicio de sesion completado para el usuario: {Usuario} ###", request.NombreUsuario);

        return new LoginResponse
        {
            Usuario = _mapper.Map<UsuarioResponse>(usuario),
            AccessToken = tokenJwt,
            TokenType = TokenType.Bearer,
            ExpiresIn = expiresIn
        };
    }

    private static byte[] ConvertirClave(string clave)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(clave));
        }
    }

    private async Task<Usuario> ValidarLogin(string nombreUsuario, byte[] claveUsuario, CancellationToken token)
    {
        _logger.LogInformation("Validando login para el usuario: {NombreUsuario}", nombreUsuario);

        var usuario = await _repository.ValidarLogin(nombreUsuario, claveUsuario, token);

        if (usuario == null)
        {
            _logger.LogWarning("Login fallido para el usuario: {NombreUsuario}", nombreUsuario);
            throw new DomainException("Credenciales invalidas");
        }

        _logger.LogInformation("Login exitoso para el usuario: {NombreUsuario}", nombreUsuario);
        return usuario;
    }

    private (string token, int expiresIn) GenerarToken(Usuario usuario)
    {
        _logger.LogInformation("Generando token para el usuario: {NombreUsuario}", usuario.NumUsuario);
        return _jwtService.GenerateToken(usuario.CodUsuario, usuario.NumUsuario);
    }

    #endregion
}
