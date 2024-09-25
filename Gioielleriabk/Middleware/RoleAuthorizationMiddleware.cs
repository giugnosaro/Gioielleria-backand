using Gioielleriabk.Models;
using Microsoft.AspNetCore.Authorization;

namespace Gioielleriabk.Middleware
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Ottieni il ruolo dell'utente (es. da un token JWT o dal database)
            var userRole = context.User.FindFirst("role")?.Value;

            if (string.IsNullOrEmpty(userRole))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Role is missing.");
                return;
            }

            // Verifica il ruolo
            if (!IsAuthorized(context, userRole))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: You do not have permission.");
                return;
            }

            await _next(context);
        }

    private bool IsAuthorized(HttpContext context, string userRole)
    {
        // Ottieni l'endpoint richiesto
        var endpoint = context.GetEndpoint();
        if (endpoint == null) return false;

        // Logica di autorizzazione in base al ruolo
        if (userRole == Role.Admin.ToString()) return true; // Admin ha tutti i permessi
        if (userRole == Role.User.ToString() && endpoint.Metadata.GetMetadata<IAllowAnonymous>() == null)
        {
            // Utente ha permessi limitati
            return endpoint.DisplayName.Contains("BuyJewelry");
        }

        return false;
    }
}
}