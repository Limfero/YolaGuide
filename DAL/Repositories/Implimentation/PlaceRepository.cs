using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        public PlaceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<Place> GetPlacesByCategory(Category category)
        {
            return _dbContext.Plases
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
                .Where(place => place.Categories.Contains(category))
                .ToList();
        }

        public List<Place> GetPlacesByName(string name)
        {
            return _dbContext.Plases
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
                .Where(place => place.Name.Contains(name))
                .ToList();
        }

        public Place GetPlaceById(int id)
        {
            return _dbContext.Plases
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
                .FirstOrDefault(place => place.Id == id);
        }

        public List<Place> Search(string userInput)
        {
            return _dbContext.Plases
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
                .Where(place => EF.Functions.FreeText(place.Name, userInput))
                .ToList();
        }
    }
}

