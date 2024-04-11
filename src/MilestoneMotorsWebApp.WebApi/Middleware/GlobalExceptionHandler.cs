using MilestoneMotorsWebApp.WebApi.Helpers;

namespace MilestoneMotorsWebApp.WebApi.Middleware
{
    public class GlobalExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (InvalidDataException e)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                await context
                    .Response
                    .WriteAsync(
                        SerializeProblem.HandleException(
                            e,
                            "Not Found.",
                            StatusCodes.Status404NotFound
                        )
                    );
            }
            catch (BadHttpRequestException e)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context
                    .Response
                    .WriteAsync(
                        SerializeProblem.HandleException(
                            e,
                            "Bad Request.",
                            StatusCodes.Status400BadRequest
                        )
                    );
            }
            catch (Exception e)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context
                    .Response
                    .WriteAsync(
                        SerializeProblem.HandleException(
                            e,
                            "Internal Server Error",
                            StatusCodes.Status500InternalServerError
                        )
                    );
            }
        }
    }
}
