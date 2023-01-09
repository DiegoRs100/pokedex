using Newtonsoft.Json;

namespace Pokedex.Api.Meddlewares
{
    public class ExceptionMeddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMeddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHostEnvironment enviroment)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, enviroment, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, IHostEnvironment enviroment, Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            if (!enviroment.IsProduction())
            {
                context.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(new
                {
                    Error = exception.Message,
                    Details = exception.InnerException?.Message
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}