﻿namespace MilestoneMotorsWebApp.Common.Interfaces
{
    public interface IMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
