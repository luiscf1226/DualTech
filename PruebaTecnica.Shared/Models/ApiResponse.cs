using System.Text.Json.Serialization;

namespace PruebaTecnica.Shared.Models
{
    /// <summary>
    /// Standardized API response structure for all endpoints
    /// </summary>
    /// <typeparam name="T">Type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        /// <example>true</example>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Optional message for the client
        /// </summary>
        /// <example>Operation completed successfully</example>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// List of errors that occurred during processing
        /// </summary>
        /// <example>[]</example>
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// The data payload of the response
        /// </summary>
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        /// <param name="data">The data to include in the response</param>
        /// <param name="message">Optional message for the client</param>
        /// <returns>A successful API response</returns>
        public static ApiResponse<T> CreateSuccess(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a failed response with error messages
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="errors">List of detailed error messages</param>
        /// <returns>A failed API response</returns>
        public static ApiResponse<T> Failure(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        /// <summary>
        /// Creates a failed response with a single error message
        /// </summary>
        /// <param name="message">Message describing the error</param>
        /// <param name="error">Detailed error message</param>
        /// <returns>A failed API response</returns>
        public static ApiResponse<T> Failure(string message, string error)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }

        /// <summary>
        /// Creates a failed response from an exception
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        /// <param name="message">Message describing the error</param>
        /// <returns>A failed API response</returns>
        public static ApiResponse<T> FromException(Exception exception, string message = "An error occurred while processing your request")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { exception.Message }
            };
        }
    }
} 