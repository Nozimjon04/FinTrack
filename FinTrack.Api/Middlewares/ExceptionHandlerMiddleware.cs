using FinTrack.Api.Helpers;
using FinTrack.Api.Service.Exceptions;

namespace FinTrack.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CustomException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new Response
            {
                Code = ex.StatusCode,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new Response
            {
                Code = 500,
                Message = ex.Message
            });
        }
    }
}
