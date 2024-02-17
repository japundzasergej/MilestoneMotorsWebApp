namespace MilestoneMotorsWebApp.Common.Utilities
{
    public static class UriBuilderExtension
    {
        public static UriBuilder ExtendPath(this UriBuilder builder, string suffix)
        {
            builder.Path += suffix;
            return builder;
        }
    }
}
