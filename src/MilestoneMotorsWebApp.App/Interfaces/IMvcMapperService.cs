namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IMvcMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
