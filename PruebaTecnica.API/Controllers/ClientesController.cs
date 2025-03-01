using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    /// <summary>
    /// Controller for managing clients
    /// </summary>
    [Route("api/clientes")]
    public class ClientesController : BaseApiController
    {
        private readonly IClienteRepository _clienteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientesController"/> class.
        /// </summary>
        /// <param name="clienteRepository">The cliente repository.</param>
        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns>A list of all clients</returns>
        /// <response code="200">Returns the list of clients</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Cliente>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Cliente>>), 500)]
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

        /// <summary>
        /// Gets a client by ID
        /// </summary>
        /// <param name="id">The client ID</param>
        /// <returns>The client with the specified ID</returns>
        /// <response code="200">Returns the client</response>
        /// <response code="404">If the client was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("getById/{id}")]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 500)]
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

        /// <summary>
        /// Gets a client with their orders
        /// </summary>
        /// <param name="id">The client ID</param>
        /// <returns>The client with their orders</returns>
        /// <response code="200">Returns the client with their orders</response>
        /// <response code="404">If the client was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}/ordenes")]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 500)]
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

        /// <summary>
        /// Creates a new client
        /// </summary>
        /// <param name="cliente">The client to create</param>
        /// <returns>The created client</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/clientes/create
        ///     {
        ///        "clienteId": 0,
        ///        "nombre": "John Doe",
        ///        "identidad": "1234567890"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created client</response>
        /// <response code="400">If the client data is invalid</response>
        /// <response code="409">If a client with the same Identidad already exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 201)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 400)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 409)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 500)]
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

        /// <summary>
        /// Updates an existing client
        /// </summary>
        /// <param name="cliente">The client to update</param>
        /// <returns>The updated client</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/clientes/update
        ///     {
        ///        "clienteId": 1,
        ///        "nombre": "John Doe",
        ///        "identidad": "1234567890"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the updated client</response>
        /// <response code="404">If the client was not found</response>
        /// <response code="409">If a client with the same Identidad already exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 409)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), 500)]
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

        /// <summary>
        /// Deletes a client
        /// </summary>
        /// <param name="id">The client ID</param>
        /// <returns>A success message</returns>
        /// <response code="200">If the client was successfully deleted</response>
        /// <response code="404">If the client was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 500)]
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