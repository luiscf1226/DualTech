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
    public class OrdenesController : BaseApiController
    {
        private readonly IOrdenRepository _ordenRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IDetalleOrdenRepository _detalleOrdenRepository;
        private readonly IProductoRepository _productoRepository;

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

        [HttpGet]
        public async Task<IActionResult> GetOrdenes()
        {
            try
            {
                var ordenes = await _ordenRepository.GetAllAsync();
                return Success(ordenes, "Ordenes retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<IEnumerable<Orden>>("Error retrieving ordenes", new List<string> { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrden(long id)
        {
            try
            {
                var orden = await _ordenRepository.GetByIdAsync(id);

                if (orden == null)
                {
                    return NotFound<Orden>($"Orden with ID {id} not found");
                }

                return Success(orden, "Orden retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Orden>("Error retrieving orden", new List<string> { ex.Message });
            }
        }

        [HttpGet("{id}/detalles")]
        public async Task<IActionResult> GetOrdenWithDetalles(long id)
        {
            try
            {
                var orden = await _ordenRepository.GetOrdenWithDetalles(id);

                if (orden == null)
                {
                    return NotFound<Orden>($"Orden with ID {id} not found");
                }

                return Success(orden, "Orden with details retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Orden>("Error retrieving orden with details", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Creates a new order with its details
        /// </summary>
        /// <param name="ordenDto">The order creation DTO</param>
        /// <returns>The created order</returns>
        [HttpPost("create")]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrden(long id, Orden orden)
        {
            try
            {
                if (id != orden.OrdenId)
                {
                    return BadRequest<Orden>("ID in URL does not match ID in request body");
                }

                var existingOrden = await _ordenRepository.GetByIdAsync(id);
                if (existingOrden == null)
                {
                    return NotFound<Orden>($"Orden with ID {id} not found");
                }

                // Verify cliente exists
                var cliente = await _clienteRepository.GetByIdAsync(orden.ClienteId);
                if (cliente == null)
                {
                    return BadRequest<Orden>($"Cliente with ID {orden.ClienteId} not found");
                }

                await _ordenRepository.UpdateAsync(orden);
                return Success(orden, "Orden updated successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Orden>("Error updating orden", new List<string> { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(long id)
        {
            try
            {
                var orden = await _ordenRepository.GetByIdAsync(id);
                if (orden == null)
                {
                    return NotFound<Orden>($"Orden with ID {id} not found");
                }

                // Delete associated detalles
                try
                {
                    var detalles = await _detalleOrdenRepository.GetDetallesByOrden(id);
                    foreach (var detalle in detalles)
                    {
                        await _detalleOrdenRepository.DeleteAsync(detalle);
                    }
                }
                catch (Exception ex)
                {
                    return ServerError<bool>($"Error deleting detalles for orden {id}", new List<string> { ex.Message });
                }

                await _ordenRepository.DeleteAsync(orden);
                return Success(true, "Orden and its details deleted successfully");
            }
            catch (Exception ex)
            {
                return ServerError<bool>("Error deleting orden", new List<string> { ex.Message });
            }
        }
    }
} 