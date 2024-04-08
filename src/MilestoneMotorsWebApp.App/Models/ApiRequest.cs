using static MilestoneMotorsWebApp.App.AppConfig.StaticDetails;

namespace MilestoneMotorsWebApp.App.Models
{
    public class ApiRequest
    {
        public string Url { get; set; }
        public MethodType MethodType { get; set; } = MethodType.GET;
        public object Data { get; set; }
        public string? AccessToken { get; set; }
    }
}
