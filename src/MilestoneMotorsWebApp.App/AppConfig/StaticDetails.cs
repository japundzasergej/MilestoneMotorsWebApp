namespace MilestoneMotorsWebApp.App.AppConfig
{
    public static class StaticDetails
    {
        public static string ApiBase { get; set; }

        public enum MethodType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
