using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    [Route("api/clientes")]
    public class ClientesController : BaseApiController
    {
        private readonly IClienteRepository _clienteRepository;

        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetClientes()
        {
            try
            {
                var clientes = await _clienteRepository.GetAllAsync();
                return Success(clientes, "Clientes retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<IEnumerable<Cliente>>("Error retrieving clientes", new List<string> { ex.Message });
            }
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetCliente(long id)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);

                if (cliente == null)
                {
                    return NotFound<Cliente>($"Cliente with ID {id} not found");
                }

                return Success(cliente, "Cliente retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Cliente>("Error retrieving cliente", new List<string> { ex.Message });
            }
        }

        [HttpGet("{id}/ordenes")]
        public async Task<IActionResult> GetClienteWithOrdenes(long id)
        {
            try
            {
                var cliente = await _clienteRepository.GetClienteWithOrdenes(id);

                if (cliente == null)
                {
                    return NotFound<Cliente>($"Cliente with ID {id} not found");
                }

                return Success(cliente, "Cliente with orders retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Cliente>("Error retrieving cliente with orders", new List<string> { ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCliente(Cliente cliente)
        {
            try
            {
                // Validate ClienteId is 0
                if (cliente.ClienteId != 0)
                {
                    return BadRequest<Cliente>("ClienteId must be 0 for new clients");
                }

                // Check if Identidad is unique
                var existingCliente = await _clienteRepository.GetClienteByIdentidad(cliente.Identidad);
                if (existingCliente != null)
                {
                    return Conflict<Cliente>($"A client with Identidad '{cliente.Identidad}' already exists", 
                        new List<string> { "Identidad must be unique" });
                }

                await _clienteRepository.AddAsync(cliente);
                return Created(cliente, "Cliente created successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Cliente>("Error creating cliente", new List<string> { ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCliente(Cliente cliente)
        {
            try
            {
                // Check if client exists
                var existingCliente = await _clienteRepository.GetByIdAsync(cliente.ClienteId);
                if (existingCliente == null)
                {
                    return NotFound<Cliente>($"Cliente with ID {cliente.ClienteId} not found");
                }

                // Check if Identidad is unique (excluding the current client)
                var clienteWithSameIdentidad = await _clienteRepository.GetClienteByIdentidad(cliente.Identidad);
                if (clienteWithSameIdentidad != null && clienteWithSameIdentidad.ClienteId != cliente.ClienteId)
                {
                    return Conflict<Cliente>($"A client with Identidad '{cliente.Identidad}' already exists", 
                        new List<string> { "Identidad must be unique" });
                }

                await _clienteRepository.UpdateAsync(cliente);
                return Success(cliente, "Cliente updated successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Cliente>("Error updating cliente", new List<string> { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound<Cliente>($"Cliente with ID {id} not found");
                }

                await _clienteRepository.DeleteAsync(cliente);
                return Success(true, "Cliente deleted successfully");
            }
            catch (Exception ex)
            {
                return ServerError<bool>("Error deleting cliente", new List<string> { ex.Message });
            }
        }
    }
} 