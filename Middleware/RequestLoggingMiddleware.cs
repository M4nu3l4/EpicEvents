using Serilog;
using System.Diagnostics;
using System.Security.Claims;


namespace EpicEvents.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            await _next(context);
            stopwatch.Stop();

            var user = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirst(ClaimTypes.Email)?.Value
                : "Anonymous";

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "N/A";

            Log.Information("Richiesta {Metodo} {Percorso} da {Utente} (IP: {IP}) → {StatusCode} in {Tempo}ms",
                request.Method,
                request.Path,
                user,
                ip,
                context.Response?.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}
