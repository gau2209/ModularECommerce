using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Models
{
    public sealed class ErrorResponse
    {
        public string TraceId { get; init; } = string.Empty;

        public int StatusCode { get; init; }

        public string Message { get; init; } = string.Empty;

        public object? Errors { get; init; }
    }
}
