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

        public override async Task<Place> CreateAsync(Place entity)
        {
            var categories = entity.Categories;
            foreach (var category in categories)
            {
                category.Users = new();
                category.Subcategories = new();
                category.Places = new();
                category.Subcategory = null;
            }


            entity.Categories = new();
            entity.Users = new();
            entity.Routes = new();

            _dbContext.Plases.Add(entity);
            await _dbContext.SaveChangesAsync();

            entity.Categories.AddRange(categories);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public override async Task<Place> RemoveAsync(Place entity)
        {
            entity.Users = new();
            entity.Routes = new();
            entity.Categories = new();

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
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
                .Where(place => place.Name.Contains(name))
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
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
            var places = _dbContext.Plases
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
                .ToList();

            List<Place> search = new();

            foreach (var place in places)
                if (place.Name.ToLower().Contains(userInput.ToLower().Trim()))
                    search.Add(place);

            return search;
        }
    }
}

