namespace api_prueba_ecop.src.Application.Interfaces;
public interface IJwtTokenService
{
    (string token, int expiresIn) GenerateToken(int codUsuario, string numUsuario);
}
