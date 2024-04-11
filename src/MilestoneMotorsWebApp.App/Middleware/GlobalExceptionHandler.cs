namespace MilestoneMotorsWebApp.App.Middleware
{
    public class GlobalExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/html";

                switch (e)
                {
                    case BadHttpRequestException:
                        context.Response.Redirect("/Home/Error?statuscode=400");
                        break;
                    case UnauthorizedAccessException:
                        context.Response.Redirect("/Home/Error?statuscode=401");
                        break;
                    case InvalidDataException:
                        context.Response.Redirect("/Home/Error?statuscode=404");
                        break;
                    default:
                        context.Response.Redirect("/Home/Error?statuscode=500");
                        break;
                }
            }
        }
    }
}
