using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Shared.Models;

namespace PruebaTecnica.API.Controllers
{
    /// <summary>
    /// Base controller that all API controllers should inherit from
    /// Provides standardized response methods
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns a successful response with data
        /// </summary>
        /// <returns>A successful response with data</returns>
        protected IActionResult Success<T>(T data, string message = "")
        {
            return Ok(ApiResponse<T>.CreateSuccess(data, message));
        }

        /// <summary>
        /// Returns a successful response with no data (204 No Content)
        /// </summary>
        protected IActionResult SuccessNoContent(string message = "")
        {
            return NoContent();
        }

        /// <summary>
        /// Returns a created response with data
        /// </summary>
        /// <returns>A created response with data</returns>
        protected IActionResult Created<T>(T data, string message = "")
        {
            return StatusCode(201, ApiResponse<T>.CreateSuccess(data, message));
        }

        /// <summary>
        /// Returns a bad request response with error message
        /// </summary>
        protected IActionResult BadRequest<T>(string message, List<string>? errors = null)
        {
            return base.BadRequest(ApiResponse<T>.Failure(message, errors));
        }

        /// <summary>
        /// Returns a not found response with error message
        /// </summary>
        protected IActionResult NotFound<T>(string message, List<string>? errors = null)
        {
            return base.NotFound(ApiResponse<T>.Failure(message, errors));
        }

        /// <summary>
        /// Returns an unauthorized response with error message
        /// </summary>
        protected IActionResult Unauthorized<T>(string message, List<string>? errors = null)
        {
            return base.Unauthorized(ApiResponse<T>.Failure(message, errors));
        }

        /// <summary>
        /// Returns a forbidden response with error message
        /// </summary>
        protected IActionResult Forbidden<T>(string message, List<string>? errors = null)
        {
            return StatusCode(403, ApiResponse<T>.Failure(message, errors));
        }

        /// <summary>
        /// Returns a conflict response with error message
        /// </summary>
        protected IActionResult Conflict<T>(string message, List<string>? errors = null)
        {
            return StatusCode(409, ApiResponse<T>.Failure(message, errors));
        }

        /// <summary>
        /// Returns a server error response with error message
        /// </summary>
        protected IActionResult ServerError<T>(string message, List<string>? errors = null)
        {
            return StatusCode(500, ApiResponse<T>.Failure(message, errors));
        }
    }
} 