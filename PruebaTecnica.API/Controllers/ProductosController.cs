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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductoResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductoResponseDto>>), 500)]
        public async Task<IActionResult> GetProductos()
        {
            try
            {
                var productos = await _productoRepository.GetAllAsync();
                var productosResponse = productos.Select(p => new ProductoResponseDto
                {
                    ProductoId = p.ProductoId,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Existencia = p.Existencia
                }).ToList();

                return Success(productosResponse, "Productos retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<IEnumerable<ProductoResponseDto>>("Error retrieving productos", new List<string> { ex.Message });
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
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 404)]
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 500)]
        public async Task<IActionResult> GetProducto(long id)
        {
            try
            {
                var producto = await _productoRepository.GetByIdAsync(id);

                if (producto == null)
                {
                    return NotFound<ProductoResponseDto>($"Producto with ID {id} not found");
                }

                var productoResponse = new ProductoResponseDto
                {
                    ProductoId = producto.ProductoId,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Existencia = producto.Existencia
                };

                return Success(productoResponse, "Producto retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServerError<ProductoResponseDto>("Error retrieving producto", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="productoDto">The product to create</param>
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
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 400)]
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 500)]
        public async Task<IActionResult> CreateProducto(ProductoDto productoDto)
        {
            try
            {
                // Validate ProductoId is 0
                if (productoDto.ProductoId != 0)
                {
                    return BadRequest<ProductoResponseDto>("ProductoId must be 0 for new products");
                }

                // Map DTO to entity
                var producto = new Producto
                {
                    ProductoId = 0, // Ensure it's 0
                    Nombre = productoDto.Nombre,
                    Descripcion = productoDto.Descripcion,
                    Precio = productoDto.Precio,
                    Existencia = productoDto.Existencia
                };

                // Save to database
                var createdProducto = await _productoRepository.AddAsync(producto);

                // Map entity to response DTO
                var productoResponse = new ProductoResponseDto
                {
                    ProductoId = createdProducto.ProductoId,
                    Nombre = createdProducto.Nombre,
                    Descripcion = createdProducto.Descripcion,
                    Precio = createdProducto.Precio,
                    Existencia = createdProducto.Existencia
                };

                return Created(productoResponse, "Producto created successfully");
            }
            catch (Exception ex)
            {
                return ServerError<ProductoResponseDto>("Error creating producto", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="productoDto">The product to update</param>
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
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 404)]
        [ProducesResponseType(typeof(ApiResponse<ProductoResponseDto>), 500)]
        public async Task<IActionResult> UpdateProducto(ProductoDto productoDto)
        {
            try
            {
                // Check if product exists
                var existingProducto = await _productoRepository.GetByIdAsync(productoDto.ProductoId);
                if (existingProducto == null)
                {
                    return NotFound<ProductoResponseDto>($"Producto with ID {productoDto.ProductoId} not found");
                }

                // Update the existing product's properties
                existingProducto.Nombre = productoDto.Nombre;
                existingProducto.Descripcion = productoDto.Descripcion;
                existingProducto.Precio = productoDto.Precio;
                existingProducto.Existencia = productoDto.Existencia;
                
                // Save the changes to the existing entity
                await _productoRepository.UpdateAsync(existingProducto);
                
                // Map entity to response DTO
                var productoResponse = new ProductoResponseDto
                {
                    ProductoId = existingProducto.ProductoId,
                    Nombre = existingProducto.Nombre,
                    Descripcion = existingProducto.Descripcion,
                    Precio = existingProducto.Precio,
                    Existencia = existingProducto.Existencia
                };
                
                return Success(productoResponse, "Producto updated successfully");
            }
            catch (Exception ex)
            {
                return ServerError<ProductoResponseDto>("Error updating producto", new List<string> { ex.Message });
            }
        }
    }
} 