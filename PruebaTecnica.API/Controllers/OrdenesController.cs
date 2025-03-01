using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Shared.Models;
using PruebaTecnica.Shared.Models.DTOs;
using PruebaTecnica.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    /// <summary>
    /// Controller for managing orders
    /// </summary>
    [Route("api/ordenes")]
    public class OrdenesController : BaseApiController
    {
        private readonly IOrdenRepository _ordenRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IDetalleOrdenRepository _detalleOrdenRepository;
        private readonly IProductoRepository _productoRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdenesController"/> class.
        /// </summary>
        /// <param name="ordenRepository">The orden repository.</param>
        /// <param name="clienteRepository">The cliente repository.</param>
        /// <param name="detalleOrdenRepository">The detalle orden repository.</param>
        /// <param name="productoRepository">The producto repository.</param>
        public OrdenesController(
            IOrdenRepository ordenRepository,
            IClienteRepository clienteRepository,
            IDetalleOrdenRepository detalleOrdenRepository,
            IProductoRepository productoRepository)
        {
            _ordenRepository = ordenRepository;
            _clienteRepository = clienteRepository;
            _detalleOrdenRepository = detalleOrdenRepository;
            _productoRepository = productoRepository;
        }

        /// <summary>
        /// Creates a new order with its details
        /// </summary>
        /// <param name="ordenDto">The order creation DTO</param>
        /// <returns>The created order</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/ordenes/create
        ///     {
        ///        "ordenId": 0,
        ///        "clienteId": 1,
        ///        "detalle": [
        ///           {
        ///              "productoId": 1,
        ///              "cantidad": 2
        ///           },
        ///           {
        ///              "productoId": 2,
        ///              "cantidad": 1
        ///           }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the created order</response>
        /// <response code="400">If the order data is invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<OrdenResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<OrdenResponseDto>), 400)]
        [ProducesResponseType(typeof(ApiResponse<OrdenResponseDto>), 500)]
        public async Task<IActionResult> CreateOrden([FromBody] OrdenCreateDto ordenDto)
        {
            try
            {
                // Validation 1: OrdenId must be 0
                if (ordenDto.OrdenId != 0)
                {
                    return BadRequest<OrdenResponseDto>("OrdenId must be 0 for new orders");
                }

                // Validation 2: ClienteId must exist
                var cliente = await _clienteRepository.GetByIdAsync(ordenDto.ClienteId);
                if (cliente == null)
                {
                    return BadRequest<OrdenResponseDto>($"Cliente with ID {ordenDto.ClienteId} not found");
                }

                // Create the order with initial values
                var orden = new Orden
                {
                    ClienteId = ordenDto.ClienteId,
                    Impuesto = 0,
                    Subtotal = 0,
                    Total = 0,
                    FechaCreacion = DateTime.Now
                };

                // Save the order to get the generated ID
                var createdOrden = await _ordenRepository.AddAsync(orden);

                // Process order details
                var detallesResponse = new List<DetalleOrdenResponseDto>();
                decimal totalImpuesto = 0;
                decimal totalSubtotal = 0;
                decimal totalTotal = 0;

                foreach (var detalleDto in ordenDto.Detalle)
                {
                    // Validation: ProductoId must exist
                    var producto = await _productoRepository.GetByIdAsync(detalleDto.ProductoId);
                    if (producto == null)
                    {
                        return BadRequest<OrdenResponseDto>($"Producto with ID {detalleDto.ProductoId} not found");
                    }

                    // Bonus validation: Check if there's enough stock
                    if (detalleDto.Cantidad > producto.Existencia)
                    {
                        return BadRequest<OrdenResponseDto>($"Producto '{producto.Nombre}' does not have enough stock. Available: {producto.Existencia}, Requested: {detalleDto.Cantidad}");
                    }

                    // Calculate values
                    decimal subtotal = detalleDto.Cantidad * producto.Precio;
                    decimal impuesto = subtotal * 0.15m; // 15% tax
                    decimal total = subtotal + impuesto;

                    // Create the order detail
                    var detalle = new DetalleOrden
                    {
                        OrdenId = createdOrden.OrdenId,
                        ProductoId = detalleDto.ProductoId,
                        Cantidad = detalleDto.Cantidad,
                        Impuesto = impuesto,
                        Subtotal = subtotal,
                        Total = total
                    };

                    // Save the order detail
                    var createdDetalle = await _detalleOrdenRepository.AddAsync(detalle);

                    // Update product stock
                    await _productoRepository.UpdateExistencia(producto.ProductoId, producto.Existencia - (long)detalleDto.Cantidad);

                    // Add to totals
                    totalImpuesto += impuesto;
                    totalSubtotal += subtotal;
                    totalTotal += total;

                    // Add to response
                    detallesResponse.Add(new DetalleOrdenResponseDto
                    {
                        DetalleOrdenId = createdDetalle.DetalleOrdenId,
                        OrdenId = createdDetalle.OrdenId,
                        ProductoId = createdDetalle.ProductoId,
                        NombreProducto = producto.Nombre,
                        Cantidad = createdDetalle.Cantidad,
                        PrecioUnitario = producto.Precio,
                        Impuesto = createdDetalle.Impuesto,
                        Subtotal = createdDetalle.Subtotal,
                        Total = createdDetalle.Total
                    });
                }

                // Update order totals
                createdOrden.Impuesto = totalImpuesto;
                createdOrden.Subtotal = totalSubtotal;
                createdOrden.Total = totalTotal;
                await _ordenRepository.UpdateAsync(createdOrden);

                // Create response
                var response = new OrdenResponseDto
                {
                    OrdenId = createdOrden.OrdenId,
                    ClienteId = createdOrden.ClienteId,
                    Impuesto = createdOrden.Impuesto,
                    Subtotal = createdOrden.Subtotal,
                    Total = createdOrden.Total,
                    FechaCreacion = createdOrden.FechaCreacion,
                    Detalles = detallesResponse
                };

                return Success(response, "Order created successfully");
            }
            catch (Exception ex)
            {
                return ServerError<OrdenResponseDto>("Error creating order", new List<string> { ex.Message });
            }
        }
    }
} 