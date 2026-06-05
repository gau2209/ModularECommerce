using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Models
{
    public sealed class APIResponse<T>
    {
        public bool Success { get; init; }

        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }

        public static APIResponse<T> Ok (T data, string message = "Success")
        {
            return new APIResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static APIResponse<T> Fail (string message)
        {
            return new APIResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
