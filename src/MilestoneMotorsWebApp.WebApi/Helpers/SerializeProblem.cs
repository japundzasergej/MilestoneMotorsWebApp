using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MilestoneMotorsWebApp.WebApi.Helpers
{
    public static class SerializeProblem
    {
        public static string HandleException(Exception e, string title, int statusCode)
        {

            var problem = new ProblemDetails
            {
                Title = title,
                Detail = e.Message,
                Status = statusCode
            };

            return JsonSerializer.Serialize(problem);
        }
    }
}
