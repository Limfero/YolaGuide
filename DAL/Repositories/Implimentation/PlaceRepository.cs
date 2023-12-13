using Microsoft.EntityFrameworkCore;
using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        public PlaceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<Place> GetPlaceByCategory(Category category)
        {
            return _dbContext.Plases
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
                .Where(place => place.Categories.Contains(category)).ToList();
        }
    }
}

