namespace api_prueba_ecop.src.Infraestructure.Entities;
public class Cliente
{
    public int CodCliente { get; set; }
    public string? NumCliente { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public int CodTipoDocumento { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? NumeroTelefono { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; }
    public DateTime? FecGra { get; set; }
}
