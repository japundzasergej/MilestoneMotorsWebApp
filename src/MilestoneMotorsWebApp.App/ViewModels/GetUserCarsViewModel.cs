using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record GetUserCarsViewModel
    {
        public List<CarDto> Cars { get; init; }
    }
}
