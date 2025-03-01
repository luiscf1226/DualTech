using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    /// <summary>
    /// Controller for managing products
    /// </summary>
    [Route("api/productos")]
    public class ProductosController : BaseApiController
    {
        private readonly IProductoRepository _productoRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductosController"/> class.
        /// </summary>
        /// <param name="productoRepository">The producto repository.</param>
        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>A list of all products</returns>
        /// <response code="200">Returns the list of products</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Producto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Producto>>), 500)]
        public async Task<IActionResult> GetProductos()
        {
            try
            {
                var productos = await _productoRepository.GetAllAsync();
                return Success(productos, "Productos retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<IEnumerable<Producto>>("Error retrieving productos", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Gets a product by ID
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>The product with the specified ID</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="404">If the product was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("getById/{id}")]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 500)]
        public async Task<IActionResult> GetProducto(long id)
        {
            try
            {
                var producto = await _productoRepository.GetByIdAsync(id);

                if (producto == null)
                {
                    return NotFound<Producto>($"Producto with ID {id} not found");
                }

                return Success(producto, "Producto retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Producto>("Error retrieving producto", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="producto">The product to create</param>
        /// <returns>The created product</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/productos/create
        ///     {
        ///        "productoId": 0,
        ///        "nombre": "Product Name",
        ///        "descripcion": "Product Description",
        ///        "precio": 100.00,
        ///        "existencia": 10
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the product data is invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 400)]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 500)]
        public async Task<IActionResult> CreateProducto(Producto producto)
        {
            try
            {
                // Validate ProductoId is 0
                if (producto.ProductoId != 0)
                {
                    return BadRequest<Producto>("ProductoId must be 0 for new products");
                }

                await _productoRepository.AddAsync(producto);
                return Created(producto, "Producto created successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Producto>("Error creating producto", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="producto">The product to update</param>
        /// <returns>The updated product</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/productos/update
        ///     {
        ///        "productoId": 1,
        ///        "nombre": "Updated Product Name",
        ///        "descripcion": "Updated Product Description",
        ///        "precio": 150.00,
        ///        "existencia": 20
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the updated product</response>
        /// <response code="404">If the product was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Producto>), 500)]
        public async Task<IActionResult> UpdateProducto(Producto producto)
        {
            try
            {
                // Check if product exists
                var existingProducto = await _productoRepository.GetByIdAsync(producto.ProductoId);
                if (existingProducto == null)
                {
                    return NotFound<Producto>($"Producto with ID {producto.ProductoId} not found");
                }

                // Update the existing product's properties
                existingProducto.Nombre = producto.Nombre;
                existingProducto.Descripcion = producto.Descripcion;
                existingProducto.Precio = producto.Precio;
                existingProducto.Existencia = producto.Existencia;
                
                // Save the changes to the existing entity
                await _productoRepository.UpdateAsync(existingProducto);
                
                return Success(existingProducto, "Producto updated successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Producto>("Error updating producto", new List<string> { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(long id)
        {
            try
            {
                var producto = await _productoRepository.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound<Producto>($"Producto with ID {id} not found");
                }

                await _productoRepository.DeleteAsync(producto);
                return Success(true, "Producto deleted successfully");
            }
            catch (Exception ex)
            {
                return ServerError<bool>("Error deleting producto", new List<string> { ex.Message });
            }
        }
    }
} 