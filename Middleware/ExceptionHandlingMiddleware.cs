using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Text.Json;

namespace EpicEvents.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    Log.Warning("❌ Accesso non autorizzato: {Path}", context.Request.Path);
                }
                else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    Log.Warning("⛔ Accesso vietato a: {Path}", context.Request.Path);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "💥 Errore non gestito in {Path}", context.Request.Path);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Errore interno del server",
                    details = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}

