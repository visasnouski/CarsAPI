using System.Net;
using System.Text.Json;
using CarsAPI.Models;

namespace CarsAPI.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlerMiddleware> _logger;

		public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
		{
			_logger = logger;
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while executing the request: {ErrorMessage}", ex.Message);
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			var errorDetails = new ErrorDetails()
			{
				StatusCode = context.Response.StatusCode,
				Message = exception.Message
			};
			await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
		}
	}
}
