using BookStore.API.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionsMiddleware(
            RequestDelegate next,
            IHostEnvironment env
        )
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var response = GetApiResponse(e);
                Log.Error(
                    "Exception Type: {exceptionType}. Exception message: {message}. Stacktrace: {stackTrace}", 
                    e.GetType().ToString(), 
                    e.Message,
                    e.StackTrace.ToString()
                );
                var serializedRepsonse = GetSerializedApiResponse(response);
                SetContextResponse(context);
                await context.Response.WriteAsync(serializedRepsonse);
            }
        }

        private ApiBaseResponse GetApiResponse(Exception e)
        {
            if (_env.IsDevelopment())
            {
                return new ApiExceptionResponse()
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    Message = e.Message,
                    ExceptionType = e.GetType().ToString(),
                    ExceptionStackTrace = e.StackTrace.ToString()
                };
            }

            return new ApiBaseResponse()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Internal Server Error"
            };
        }

        private string GetSerializedApiResponse(object response)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(response, options);
        }

        private void SetContextResponse(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}