using api_prueba_ecop.src.Application.Exceptions;
using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Entities;
using System.Text.Json;

namespace api_prueba_ecop.src.Application.Services;
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(IClienteRepository repository, ILogger<ClienteService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Cliente>> ObtenerClientesActivos(CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de clientes activos ###");

        var clientes = await _repository.ObtenerClientesActivos(token);

        if (clientes == null || clientes.Count == 0)
        {
            _logger.LogWarning("No se encontraron clientes activos");
            throw new DomainException("No se encontraron clientes activos");
        }

        _logger.LogInformation("### Clientes activos obtenidos correctamente. Total: {Count} ###", clientes.Count);
        return clientes;
    }

    public async Task<Cliente> ObtenerCliente(int codigoCliente, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion del cliente: {@CodigoCliente} ###", codigoCliente);

        var cliente = await _repository.ObtenerCliente(codigoCliente, token);

        if (cliente == null)
        {
            _logger.LogWarning("Cliente no encontrado para el codigo: {CodigoCliente}", codigoCliente);
            throw new DomainException("Cliente no encontrado");
        }

        _logger.LogInformation("### Cliente obtenido correctamente: {@Cliente} ###", cliente);
        return cliente;
    }

    public async Task<List<Cliente>> ObtenerClientesPorFiltro(string? busqueda, int? limite, CancellationToken token)
    {
        var limiteFinal = (limite.HasValue && limite.Value > 0) ? limite.Value : 20;

        _logger.LogInformation("### Iniciando proceso de obtencion de clientes por filtro. Busqueda: {Busqueda}, Limite: {Limite} ###", busqueda, limiteFinal);

        var clientes = await _repository.ObtenerClientesPorFiltro(busqueda, limiteFinal, token);

        _logger.LogInformation("### Clientes obtenidos correctamente por filtro. Total: {Count} ###", clientes.Count);
        return clientes;
    }

    public async Task<Cliente> NuevoCliente(Cliente cliente, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de insercion del nuevo cliente: {@Cliente}", JsonSerializer.Serialize(cliente));

        var nuevoCliente = await _repository.InsertarClienteNuevo(cliente, token);

        if (nuevoCliente == null)
        {
            _logger.LogWarning("Error al insertar el nuevo cliente");
            throw new DomainException("Error al insertar el nuevo cliente");
        }

        _logger.LogInformation("### Cliente insertado correctamente ###");
        return nuevoCliente;
    }

    public async Task<Cliente> ActualizarCliente(int codigoCliente, Cliente cliente, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de actualizacion del cliente: {@Cliente}", JsonSerializer.Serialize(cliente));

        var clienteActualizado = await _repository.ActualizarCliente(codigoCliente, cliente, token);

        if (clienteActualizado == null)
        {
            _logger.LogWarning("Error al actualizar el nuevo cliente");
            throw new DomainException("Error al actualizar el nuevo cliente");
        }

        _logger.LogInformation("### Cliente actualizado correctamente ###");

        return clienteActualizado;
    }

    public async Task EliminarCliente(int codigoCliente, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de eliminacion del cliente: {@CodigoCliente} ###", codigoCliente);

        var operacion = await _repository.EliminarCliente(codigoCliente, token);

        if (!operacion)
        {
            _logger.LogWarning("Error al eliminar el cliente");
            throw new DomainException("Error al eliminar el cliente");
        }

        _logger.LogInformation("### Cliente eliminado correctamente ###");
    }
}
