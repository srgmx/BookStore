using BookStore.API.ResponseModels;
using BookStore.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
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
                //Log.Error(
                //    "Exception Type: {exceptionType}. Exception message: {message}. Stacktrace: {stackTrace}", 
                //    e.GetType().ToString(), 
                //    e.Message,
                //    e.StackTrace.ToString()
                //);
                ApiBaseResponse response;

                if (e is RecordNotFoundException)
                {
                    response = new ApiBaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = e.Message
                    };
                    await Respond(response, context);

                    return;
                }

                if (e is InvalidAuthorsException || e is ExistingAuthorException)
                {
                    response = new ApiBaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = e.Message
                    };
                    await Respond(response, context);

                    return;
                }

                if (_env.IsDevelopment())
                {
                    response = new ApiExceptionResponse()
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = e.Message,
                        ExceptionType = e.GetType().ToString(),
                        ExceptionStackTrace = e.StackTrace.ToString()
                    };
                    await Respond(response, context);

                    return;
                }
                else
                {
                    response = new ApiBaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = "Internal Server Error."
                    };
                    await Respond(response, context);

                    return;
                }
            }
        }

        private async Task Respond(ApiBaseResponse response, HttpContext context)
        {
            var serializedResponse = GetSerializedResponse(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(serializedResponse);
        }

        private string GetSerializedResponse(object response)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(response, options);
        }
    }
}
