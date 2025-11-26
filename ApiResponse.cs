using System.Collections.Generic;
using System.Linq;

namespace construtivaBack
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static ApiResponse<T> CreateSuccess(T data, string message = null)
        {
            return new ApiResponse<T> { Success = true, Data = data, Message = message };
        }

        public static ApiResponse<T> CreateError(string message, IEnumerable<string> errors = null)
        {
            return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
        }
    }
}
