using api_prueba_ecop.src.Application.Exceptions;
using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using api_prueba_ecop.src.Infraestructure.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_prueba_ecop.src.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repository;
    private readonly ILogger<PedidoService> _logger;
    private readonly IMapper _mapper;

    public PedidoService(IPedidoRepository repository, ILogger<PedidoService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    #region "Obtener Pedidos"

    public async Task<PedidoResponse> Obtener(int codigoPedido, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion del pedido: {@CodigoPedido} ###", codigoPedido);

        var pedido = await ObtenerPedidoDesdeRepositorio(codigoPedido, token);
        var pedidoValidado = ValidarPedidoExiste(pedido, codigoPedido);
        var response = MapearPedidoAResponse(pedidoValidado);

        _logger.LogInformation("### Pedido obtenido correctamente ###");
        return response;
    }

    public async Task<PaginatedResponse<PedidoResponse>> ObtenerPorFiltro(string? busqueda, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de obtencion de pedidos por filtro. Busqueda: {Busqueda}, FechaDesde: {FechaDesde}, FechaHasta: {FechaHasta}, Pagina: {PageNumber}, Tamaño: {PageSize} ###", busqueda, fechaDesde, fechaHasta, pageNumber, pageSize);

        NormalizarParametrosPaginacion(ref pageNumber, ref pageSize);
        var (pedidos, totalCount) = await ConsultarPedidosPorFiltro(busqueda, fechaDesde, fechaHasta, pageNumber, pageSize, token);

        if (ListaPedidosEstaVacia(pedidos))
        {
            return CrearRespuestaPaginadaVacia(pageNumber, pageSize);
        }

        var response = ConstruirRespuestaPaginada(pedidos, totalCount, pageNumber, pageSize);

        _logger.LogInformation("### Pedidos obtenidos correctamente. Total: {Count}, Paginas: {TotalPages} ###", totalCount, response.TotalPages);
        return response;
    }

    private async Task<Pedido?> ObtenerPedidoDesdeRepositorio(int codigoPedido, CancellationToken token)
    {
        return await _repository.ObtenerPedido(codigoPedido, token);
    }

    private Pedido ValidarPedidoExiste(Pedido? pedido, int codigoPedido)
    {
        if (pedido == null)
        {
            _logger.LogWarning("Pedido no encontrado para el codigo: {CodigoPedido}", codigoPedido);
            throw new DomainException("Pedido no encontrado");
        }
        return pedido;
    }

    private PedidoResponse MapearPedidoAResponse(Pedido pedido)
    {
        return _mapper.Map<PedidoResponse>(pedido);
    }

    private void NormalizarParametrosPaginacion(ref int pageNumber, ref int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
    }

    private async Task<(List<Pedido> pedidos, int totalCount)> ConsultarPedidosPorFiltro(string? busqueda, DateTime? fechaDesde, DateTime? fechaHasta, int pageNumber, int pageSize, CancellationToken token)
    {
        return await _repository.ObtenerPedidosPorFiltro(busqueda, fechaDesde, fechaHasta, pageNumber, pageSize, token);
    }

    private bool ListaPedidosEstaVacia(List<Pedido>? pedidos)
    {
        if (pedidos == null || pedidos.Count == 0)
        {
            _logger.LogWarning("No se encontraron pedidos con los filtros aplicados");
            return true;
        }
        return false;
    }

    private PaginatedResponse<PedidoResponse> CrearRespuestaPaginadaVacia(int pageNumber, int pageSize)
    {
        return new PaginatedResponse<PedidoResponse>
        {
            Items = new List<PedidoResponse>(),
            TotalItems = 0,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = 0
        };
    }

    private PaginatedResponse<PedidoResponse> ConstruirRespuestaPaginada(List<Pedido> pedidos, int totalCount, int pageNumber, int pageSize)
    {
        var pedidosResponse = MapearListaPedidosAResponse(pedidos);
        var totalPages = CalcularTotalPaginas(totalCount, pageSize);

        return new PaginatedResponse<PedidoResponse>
        {
            Items = pedidosResponse,
            TotalItems = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }

    private List<PedidoResponse> MapearListaPedidosAResponse(List<Pedido> pedidos)
    {
        return _mapper.Map<List<PedidoResponse>>(pedidos);
    }

    private int CalcularTotalPaginas(int totalCount, int pageSize)
    {
        return (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    #endregion

    #region "Insertar Pedido Nuevo"

    public async Task<PedidoResponse> Nuevo(PedidoRequest request, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de creacion de pedido: {@Request} ###", request);

        var pedido = PrepararPedidoParaInsercion(request, token);
        var pedidoCreado = await EjecutarInsercionConTransaccion(pedido, token);
        var response = MapearPedidoAResponse(pedidoCreado);

        _logger.LogInformation("### Pedido creado correctamente. Codigo: {CodPedido} ###", pedidoCreado.CodPedido);
        return response;
    }

    private Pedido PrepararPedidoParaInsercion(PedidoRequest request, CancellationToken token)
    {
        var pedido = MapearRequestAPedido(request);
        var subtotal = CalcularSubtotalPedido(pedido.Detalles);
        pedido.Total = CalcularTotalPedido(subtotal, pedido.Iva ?? 0);
        pedido.FecGra = DateTime.Now;
        return pedido;
    }

    private Pedido MapearRequestAPedido(PedidoRequest request)
    {
        return _mapper.Map<Pedido>(request);
    }

    private decimal CalcularSubtotalPedido(List<PedidoDetalle> detalles)
    {
        return detalles.Sum(d => (d.Cantidad ?? 0) * (d.PrecioUnitario ?? 0));
    }

    private decimal CalcularTotalPedido(decimal subtotal, decimal iva)
    {
        return subtotal + iva;
    }

    private async Task<Pedido> EjecutarInsercionConTransaccion(Pedido pedido, CancellationToken token)
    {
        using var transaction = await IniciarTransaccion(token);

        try
        {
            var pedidoCreado = await InsertarPedidoCabecera(pedido, token);
            await InsertarDetallesPedido(pedidoCreado.CodPedido, pedido.Detalles, token);
            await ConfirmarTransaccion(transaction, token);

            return await ObtenerPedidoCompleto(pedidoCreado.CodPedido, token);
        }
        catch
        {
            await RevertirTransaccion(transaction, token);
            throw new DomainException("Error al crear el pedido");
        }
    }

    private async Task<IDbContextTransaction> IniciarTransaccion(CancellationToken token)
    {
        return await _repository.IniciarTransaccion(token);
    }

    private async Task<Pedido> InsertarPedidoCabecera(Pedido pedido, CancellationToken token)
    {
        _logger.LogInformation("Insertando cabecera del pedido");
        return await _repository.InsertarPedido(pedido, token);
    }

    private async Task InsertarDetallesPedido(int codigoPedido, List<PedidoDetalle> detalles, CancellationToken token)
    {
        _logger.LogInformation("Insertando {Count} detalles del pedido", detalles.Count);
        PrepararDetallesParaInsercion(codigoPedido, detalles);
        await _repository.InsertarDetalles(detalles, token);
    }

    private void PrepararDetallesParaInsercion(int codigoPedido, List<PedidoDetalle> detalles)
    {
        foreach (var detalle in detalles)
        {
            detalle.CodPedidoDetalle = 0;
            detalle.CodPedido = codigoPedido;
            detalle.FecGra = DateTime.Now;
        }
    }

    private async Task ConfirmarTransaccion(IDbContextTransaction transaction, CancellationToken token)
    {
        await transaction.CommitAsync(token);
    }

    private async Task RevertirTransaccion(IDbContextTransaction transaction, CancellationToken token)
    {
        await transaction.RollbackAsync(token);
        _logger.LogError("Error al insertar el pedido. Se realizó rollback de la transacción");
    }

    private async Task<Pedido> ObtenerPedidoCompleto(int codigoPedido, CancellationToken token)
    {
        var pedidoCompleto = await _repository.ObtenerPedido(codigoPedido, token);
        if (pedidoCompleto == null)
        {
            throw new DomainException("Error al obtener el pedido creado");
        }
        return pedidoCompleto;
    }

    #endregion

    #region "Anular Pedido"

    public async Task<PedidoResponse> Anulacion(int codigoPedido, AnularPedidoRequest request, CancellationToken token)
    {
        _logger.LogInformation("### Iniciando proceso de anulacion del pedido: {CodigoPedido}, Motivo: {Motivo} ###", codigoPedido, request.MotivoAnulacion);

        var pedido = await ObtenerYValidarPedidoParaAnulacion(codigoPedido, token);
        var pedidoAnulado = await EjecutarAnulacionDePedido(pedido, request.MotivoAnulacion, token);
        var response = MapearPedidoAResponse(pedidoAnulado);

        _logger.LogInformation("### Pedido anulado correctamente ###");
        return response;
    }

    private async Task<Pedido> ObtenerYValidarPedidoParaAnulacion(int codigoPedido, CancellationToken token)
    {
        _logger.LogInformation("Validando existencia y estado del pedido: {CodigoPedido}", codigoPedido);

        var pedido = await ObtenerPedidoDesdeRepositorio(codigoPedido, token);
        ValidarPedidoExisteParaAnulacion(pedido, codigoPedido);
        ValidarPedidoEstaActivo(pedido!);

        return pedido!;
    }

    private void ValidarPedidoExisteParaAnulacion(Pedido? pedido, int codigoPedido)
    {
        if (pedido == null)
        {
            _logger.LogWarning("El pedido con codigo {CodigoPedido} no existe", codigoPedido);
            throw new DomainException("El pedido no existe");
        }
    }

    private void ValidarPedidoEstaActivo(Pedido pedido)
    {
        if (!pedido.Activo)
        {
            _logger.LogWarning("El pedido {CodigoPedido} ya está anulado", pedido.CodPedido);
            throw new DomainException("El pedido ya está anulado");
        }
    }

    private async Task<Pedido> EjecutarAnulacionDePedido(Pedido pedido, string motivoAnulacion, CancellationToken token)
    {
        _logger.LogInformation("Anulando pedido: {CodigoPedido}, Motivo: {Motivo}", pedido.CodPedido, motivoAnulacion);

        MarcarPedidoComoAnulado(pedido, motivoAnulacion);
        var pedidoActualizado = await ActualizarPedidoEnBaseDatos(pedido, token);

        return await ObtenerPedidoCompleto(pedidoActualizado.CodPedido, token);
    }

    private void MarcarPedidoComoAnulado(Pedido pedido, string motivoAnulacion)
    {
        pedido.Activo = false;
        pedido.MotivoAnulacion = motivoAnulacion;
    }

    private async Task<Pedido> ActualizarPedidoEnBaseDatos(Pedido pedido, CancellationToken token)
    {
        var pedidoActualizado = await _repository.ActualizarPedido(pedido, token);
        if (pedidoActualizado == null)
        {
            _logger.LogError("Error al actualizar el pedido en la base de datos");
            throw new DomainException("Error al anular el pedido");
        }
        return pedidoActualizado;
    }

    #endregion
}