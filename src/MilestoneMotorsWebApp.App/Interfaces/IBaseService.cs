using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDTO> SendAsync(ApiRequest apiRequest);
    }
}
