using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Application.Wrappers
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
            Errors = new List<string>();
            Succeeded = false;
            Message = string.Empty;
            Data = default;
        }
        public ApiResponse(T data, string message = "")
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
            Data = default;
            Errors = new List<string>();

        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T? Data { get; set; }

        public async static Task<ApiResponse<T>> SuccessAsync(T data, string message)
        {
            return await Task.FromResult(new ApiResponse<T>(data, message));
        }

        public async static Task<ApiResponse<T>> FailAsync(string message)
        {
            return await Task.FromResult(new ApiResponse<T>(message));
        }

    }

}
