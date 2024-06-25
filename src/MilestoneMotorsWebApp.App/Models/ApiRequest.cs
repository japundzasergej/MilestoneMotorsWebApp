using static MilestoneMotorsWebApp.App.AppConfig.StaticDetails;

namespace MilestoneMotorsWebApp.App.Models
{
    public class ApiRequest
    {
        public string Url { get; init; }
        public MethodType MethodType { get; init; } = MethodType.GET;
        public object Data { get; init; }
        public string? AccessToken { get; init; }
    }
}
