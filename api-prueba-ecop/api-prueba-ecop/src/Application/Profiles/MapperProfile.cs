using api_prueba_ecop.src.Application.Models.Requests;
using api_prueba_ecop.src.Application.Models.Responses;
using api_prueba_ecop.src.Infraestructure.Entities;
using AutoMapper;

namespace api_prueba_ecop.src.Application.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        #region "Usuario"

        CreateMap<Usuario, UsuarioResponse>()
            .ForMember(dest => dest.CodUsuario, opt => opt.MapFrom(src => src.CodUsuario))
            .ForMember(dest => dest.NumUsuario, opt => opt.MapFrom(src => src.NumUsuario))
            .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
            .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos));

        #endregion

        #region "Pedido"
        CreateMap<PedidoRequest, Pedido>()
            .ForMember(dest => dest.CodPedido, opt => opt.Ignore())
            .ForMember(dest => dest.NumPedido, opt => opt.MapFrom(src => src.NumPedido))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha ?? DateTime.Now))
            .ForMember(dest => dest.CodUsuario, opt => opt.MapFrom(src => src.CodUsuario))
            .ForMember(dest => dest.CodCliente, opt => opt.MapFrom(src => src.CodCliente))
            .ForMember(dest => dest.CodMoneda, opt => opt.MapFrom(src => src.CodMoneda))
            .ForMember(dest => dest.Iva, opt => opt.Ignore())
            .ForMember(dest => dest.Total, opt => opt.Ignore())
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.MotivoAnulacion, opt => opt.Ignore())
            .ForMember(dest => dest.FecGra, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));

        CreateMap<PedidoDetalleRequest, PedidoDetalle>()
            .ForMember(dest => dest.CodPedidoDetalle, opt => opt.Ignore())
            .ForMember(dest => dest.CodPedido, opt => opt.Ignore())
            .ForMember(dest => dest.CodProducto, opt => opt.MapFrom(src => src.CodProducto))
            .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
            .ForMember(dest => dest.PrecioUnitario, opt => opt.MapFrom(src => src.PrecioUnitario))
            .ForMember(dest => dest.LineaNumero, opt => opt.MapFrom(src => src.LineaNumero))
            .ForMember(dest => dest.FecGra, opt => opt.Ignore())
            .ForMember(dest => dest.Producto, opt => opt.Ignore());

        CreateMap<Pedido, PedidoResponse>()
            .ForMember(dest => dest.CodPedido, opt => opt.MapFrom(src => src.CodPedido))
            .ForMember(dest => dest.NumPedido, opt => opt.MapFrom(src => src.NumPedido))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
            .ForMember(dest => dest.CodUsuario, opt => opt.MapFrom(src => src.CodUsuario))
            .ForMember(dest => dest.CodCliente, opt => opt.MapFrom(src => src.CodCliente))
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombres : null))
            .ForMember(dest => dest.ApellidoCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Apellidos : null))
            .ForMember(dest => dest.DocumentoCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.NumeroDocumento : null))
            .ForMember(dest => dest.CodMoneda, opt => opt.MapFrom(src => src.CodMoneda))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
            .ForMember(dest => dest.Iva, opt => opt.MapFrom(src => src.Iva))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
            .ForMember(dest => dest.MotivoAnulacion, opt => opt.MapFrom(src => src.MotivoAnulacion))
            .ForMember(dest => dest.FecGra, opt => opt.MapFrom(src => src.FecGra))
            .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));

        CreateMap<PedidoDetalle, PedidoDetalleResponse>()
            .ForMember(dest => dest.CodPedidoDetalle, opt => opt.MapFrom(src => src.CodPedidoDetalle))
            .ForMember(dest => dest.CodProducto, opt => opt.MapFrom(src => src.CodProducto))
            .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto != null ? src.Producto.DesProducto : null))
            .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
            .ForMember(dest => dest.PrecioUnitario, opt => opt.MapFrom(src => src.PrecioUnitario))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => (src.Cantidad ?? 0) * (src.PrecioUnitario ?? 0)))
            .ForMember(dest => dest.LineaNumero, opt => opt.MapFrom(src => src.LineaNumero));
        #endregion
    }
}
