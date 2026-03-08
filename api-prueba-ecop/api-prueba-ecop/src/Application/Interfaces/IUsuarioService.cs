using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface IUsuarioService
{
    Task<LoginResponse> Login(LoginRequest request, CancellationToken token);
}
