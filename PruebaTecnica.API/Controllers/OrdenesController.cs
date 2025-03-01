using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    public class OrdenesController : BaseApiController
    {
        private readonly IOrdenRepository _ordenRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IDetalleOrdenRepository _detalleOrdenRepository;

        public OrdenesController(
            IOrdenRepository ordenRepository,
            IClienteRepository clienteRepository,
            IDetalleOrdenRepository detalleOrdenRepository)
        {
            _ordenRepository = ordenRepository;
            _clienteRepository = clienteRepository;
            _detalleOrdenRepository = detalleOrdenRepository;
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

        [HttpPost]
        public async Task<IActionResult> CreateOrden(Orden orden)
        {
            try
            {
                // Verify cliente exists
                var cliente = await _clienteRepository.GetByIdAsync(orden.ClienteId);
                if (cliente == null)
                {
                    return BadRequest<Orden>($"Cliente with ID {orden.ClienteId} not found");
                }

                // Validate date
                if (orden.FechaCreacion > DateTime.Now)
                {
                    return BadRequest<Orden>("La fecha de la orden no puede ser futura", new List<string> { "La fecha de la orden debe ser menor o igual a la fecha actual" });
                }

                // Set date to current if not provided
                if (orden.FechaCreacion == default)
                {
                    orden.FechaCreacion = DateTime.Now;
                }

                await _ordenRepository.AddAsync(orden);
                return Created(orden, "Orden created successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Orden>("Error creating orden", new List<string> { ex.Message });
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