namespace CRUD_1st.Middleware
{
	using Azure;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	public class ApiMiddleware
	{
		private readonly RequestDelegate _next;
		private const string API_KEY_HEADER_NAME = "X_API_KEY";

		public ApiMiddleware(RequestDelegate next)
		{
			_next = next;
		}


		public async Task Invoke(HttpContext context, IConfiguration configuration)
		{
			if (context.Request.Method == HttpMethods.Options)
			{
				// Allow preflight OPTIONS request
				context.Response.StatusCode = 204; // No Content
				return;
			}
			if (context.Request.Path.StartsWithSegments("/api"))
			{

				if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
				{
					context.Response.StatusCode = 401;
					await context.Response.WriteAsync("API Key is missing");
					return;
				}

				var key = configuration.GetValue<string>("ApiKey");
				if (!extractedApiKey.Equals(key))
				{
					context.Response.StatusCode = 401;
					await context.Response.WriteAsync("Unauthorized Client");
					return;
				}
			}

			await _next(context);
		}
	}
}
