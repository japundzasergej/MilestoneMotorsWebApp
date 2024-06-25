using MilestoneMotorsWebApp.App.Models;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IBaseService
    {
        Task<TBody> SendAsync<TBody>(ApiRequest apiRequest);
    }
}
