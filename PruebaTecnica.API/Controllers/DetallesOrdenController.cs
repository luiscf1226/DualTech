using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PruebaTecnica.API.Controllers
{
    public class DetallesOrdenController : BaseApiController
    {
        private readonly IDetalleOrdenRepository _detalleOrdenRepository;
        private readonly IOrdenRepository _ordenRepository;
        private readonly IProductoRepository _productoRepository;

        public DetallesOrdenController(
            IDetalleOrdenRepository detalleOrdenRepository,
            IOrdenRepository ordenRepository,
            IProductoRepository productoRepository)
        {
            _detalleOrdenRepository = detalleOrdenRepository;
            _ordenRepository = ordenRepository;
            _productoRepository = productoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetallesOrden()
        {
            try
            {
                var detalles = await _detalleOrdenRepository.GetAllAsync();
                return Success(detalles, "Detalles de orden retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<IEnumerable<DetalleOrden>>("Error retrieving detalles de orden", new List<string> { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetalleOrden(long id)
        {
            try
            {
                var detalle = await _detalleOrdenRepository.GetByIdAsync(id);

                if (detalle == null)
                {
                    return NotFound<DetalleOrden>($"Detalle de orden with ID {id} not found");
                }

                return Success(detalle, "Detalle de orden retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<DetalleOrden>("Error retrieving detalle de orden", new List<string> { ex.Message });
            }
        }

        [HttpGet("orden/{ordenId}")]
        public async Task<IActionResult> GetDetallesByOrdenId(long ordenId)
        {
            try
            {
                var detalles = await _detalleOrdenRepository.GetDetallesByOrden(ordenId);
                
                if (detalles == null || !detalles.Any())
                {
                    return NotFound<IEnumerable<DetalleOrden>>($"No se encontraron detalles para la orden con ID {ordenId}");
                }

                return Success(detalles, $"Detalles for orden {ordenId} retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<IEnumerable<DetalleOrden>>("Error al obtener los detalles de la orden", new List<string> { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDetalleOrden(DetalleOrden detalleOrden)
        {
            try
            {
                // Verify orden exists
                var orden = await _ordenRepository.GetByIdAsync(detalleOrden.OrdenId);
                if (orden == null)
                {
                    return BadRequest<DetalleOrden>($"Orden with ID {detalleOrden.OrdenId} not found");
                }

                // Verify producto exists
                var producto = await _productoRepository.GetByIdAsync(detalleOrden.ProductoId);
                if (producto == null)
                {
                    return BadRequest<DetalleOrden>($"Producto with ID {detalleOrden.ProductoId} not found");
                }

                // Calculate subtotal if not provided
                if (detalleOrden.Subtotal <= 0)
                {
                    detalleOrden.Subtotal = detalleOrden.Cantidad * producto.Precio;
                }

                await _detalleOrdenRepository.AddAsync(detalleOrden);
                return Created(detalleOrden, "Detalle de orden created successfully");
            }
            catch (Exception ex)
            {
                return ServerError<DetalleOrden>("Error creating detalle de orden", new List<string> { ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetalleOrden(long id, DetalleOrden detalleOrden)
        {
            try
            {
                if (id != detalleOrden.DetalleOrdenId)
                {
                    return BadRequest<DetalleOrden>("ID in URL does not match ID in request body");
                }

                var existingDetalle = await _detalleOrdenRepository.GetByIdAsync(id);
                if (existingDetalle == null)
                {
                    return NotFound<DetalleOrden>($"Detalle de orden with ID {id} not found");
                }

                // Verify orden exists
                var orden = await _ordenRepository.GetByIdAsync(detalleOrden.OrdenId);
                if (orden == null)
                {
                    return BadRequest<DetalleOrden>($"Orden with ID {detalleOrden.OrdenId} not found");
                }

                // Verify producto exists
                var producto = await _productoRepository.GetByIdAsync(detalleOrden.ProductoId);
                if (producto == null)
                {
                    return BadRequest<DetalleOrden>($"Producto with ID {detalleOrden.ProductoId} not found");
                }

                // Recalculate subtotal if cantidad changed
                if (detalleOrden.Cantidad != existingDetalle.Cantidad)
                {
                    detalleOrden.Subtotal = detalleOrden.Cantidad * producto.Precio;
                }

                await _detalleOrdenRepository.UpdateAsync(detalleOrden);
                return Success(detalleOrden, "Detalle de orden updated successfully");
            }
            catch (Exception ex)
            {
                return ServerError<DetalleOrden>("Error updating detalle de orden", new List<string> { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleOrden(long id)
        {
            try
            {
                var detalle = await _detalleOrdenRepository.GetByIdAsync(id);
                if (detalle == null)
                {
                    return NotFound<DetalleOrden>($"Detalle de orden with ID {id} not found");
                }

                await _detalleOrdenRepository.DeleteAsync(detalle);
                return Success(true, "Detalle de orden deleted successfully");
            }
            catch (Exception ex)
            {
                return ServerError<bool>("Error deleting detalle de orden", new List<string> { ex.Message });
            }
        }
    }
} 