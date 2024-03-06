namespace MilestoneMotorsWebApp.Business.Interfaces
{
    public interface IMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
