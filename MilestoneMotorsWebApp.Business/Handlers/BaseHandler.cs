using MediatR;
using MilestoneMotorsWebApp.Common.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers
{
    public abstract class BaseHandler<TRequest, TResponse, TRepository>
        : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TRepository : class
    {
        protected readonly TRepository? _repository;
        protected readonly IMapperService? _mapperService;

        protected BaseHandler(TRepository? repository, IMapperService? mapperService)
        {
            _repository = repository;
            _mapperService = mapperService;
        }

        public abstract Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken
        );
    }
}
