using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    public class ProductosController : BaseApiController
    {
        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
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

        [HttpPost]
        public async Task<IActionResult> CreateProducto(Producto producto)
        {
            try
            {
                await _productoRepository.AddAsync(producto);
                return Created(producto, "Producto created successfully");
            }
            catch (Exception ex)
            {
                return ServerError<Producto>("Error creating producto", new List<string> { ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(long id, Producto producto)
        {
            try
            {
                if (id != producto.ProductoId)
                {
                    return BadRequest<Producto>("ID in URL does not match ID in request body");
                }

                var existingProducto = await _productoRepository.GetByIdAsync(id);
                if (existingProducto == null)
                {
                    return NotFound<Producto>($"Producto with ID {id} not found");
                }

                await _productoRepository.UpdateAsync(producto);
                return Success(producto, "Producto updated successfully");
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