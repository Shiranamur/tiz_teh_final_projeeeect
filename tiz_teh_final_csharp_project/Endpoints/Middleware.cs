using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using tiz_teh_final_csharp_project.Repositories;

namespace tiz_teh_final_csharp_project.Endpoints;

public class Middleware
{
    private readonly RequestDelegate _next;
    private readonly IUserRepository _userRepository;

    public Middleware(RequestDelegate next, IUserRepository userRepository)
    {
        _next = next;
        _userRepository = userRepository;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // before the NEXT api call
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
            authHeader.ToString().StartsWith("Bearer "))
        {
            string token = authHeader.ToString().Substring("Bearer ".Length).Trim();
            var userId = await _userRepository.GetUserIdByTokenAsync(token);
            if (userId != null)
            {
                context.Items["UserId"] = userId; 
            }
            else
            {
                Console.WriteLine("[debug] Bearer of the token not found");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }


            await _next(context);
        }
        else
        {
            Console.WriteLine("[debug] No token");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}