namespace api_prueba_ecop.src.Application.Models.Responses;
public class ErrorResponse
{
    public int ErrorCode { get; set; }
    public string ErrorDescription { get; set; } = string.Empty;
}
