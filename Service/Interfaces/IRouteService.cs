using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service.Interfaces
{
    public interface IRouteService
    {
        Task<IBaseResponse<Route>> CreateRouteAsync(RouteViewModel model);

        Task<IBaseResponse<Route>> RemoveRouteAsync(Route route);

        IBaseResponse<List<Route>> GetAllRoutes();

        IBaseResponse<Route> GetRouteByName(string name);

        IBaseResponse<Route> GetRouteById(int id);
    }
}
