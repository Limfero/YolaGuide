using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Interfaces
{
    public interface IRouteRepository : IBaseRepository<Route>
    {
        Route GetRouteByName(string name);
    }
}
