using Microsoft.EntityFrameworkCore;
using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class RouteRepository : BaseRepository<Route>, IRouteRepository
    {
        public RouteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<Route> GetAll()
        {
            return _dbContext.Routs
                .Include(route => route.Places)
                .Include(route => route.Users)
                .AsSplitQuery();
        }

        public Route GetRouteByName(string name)
        {
            return _dbContext.Routs
                .Include(route => route.Places)
                .Include(route => route.Users)
                .AsSplitQuery()
                .FirstOrDefault(route => route.Name.Contains(name));
        }
    }
}
