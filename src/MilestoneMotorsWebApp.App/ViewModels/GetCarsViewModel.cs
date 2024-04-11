using MilestoneMotorsWebApp.Business.DTO;
using X.PagedList;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record GetCarsViewModel
    {
        public IPagedList<CarDto> Cars { get; init; }
    }
}
