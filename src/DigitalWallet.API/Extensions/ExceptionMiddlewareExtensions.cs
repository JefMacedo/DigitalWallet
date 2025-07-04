namespace DigitalWallet.API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Middlewares.ExceptionHandlingMiddleware>();
    }
}
