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

        public override async Task<Route> CreateAsync(Route entity)
        {
            var places = entity.Places;
            entity.Places = new();
            entity.Users = new();

            _dbContext.Routes.Add(entity);
            await _dbContext.SaveChangesAsync();

            foreach (var place in places)
            {
                place.Categories = null;
                place.Users = null;
            }

            entity.Places.AddRange(places);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public override IQueryable<Route> GetAll()
        {
            return _dbContext.Routes
                .Include(route => route.Places)
                .Include(route => route.Users)
                .AsSplitQuery();
        }

        public Route GetRouteByName(string name)
        {
            var routes = _dbContext.Routes
                .Include(route => route.Places)
                .Include(route => route.Users)
                .AsSplitQuery()
                .ToList();

            return routes.FirstOrDefault(route => route.Name.Contains(name));
        }

        public Route GetRouteById(int id)
        {
            return _dbContext.Routes
                .Include(route => route.Places)
                .Include(route => route.Users)
                .AsSplitQuery()
                .FirstOrDefault(route => route.Id == id);
        }
    }
}
