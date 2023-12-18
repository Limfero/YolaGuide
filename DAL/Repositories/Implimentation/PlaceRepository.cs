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

        public override async Task<Place> CreateAsync(Place entity)
        {
            var categories = entity.Categories;
            entity.Categories = new();

            _dbContext.Plases.Add(entity);
            await _dbContext.SaveChangesAsync();

            entity.Categories.AddRange(categories);
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
                .Where(place => place.Name == name)
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
            return _dbContext.Plases
                .FromSql($"SELECT * FROM [place] WHERE name LIKE '%{userInput}%'")
                .Include(place => place.Categories)
                .Include(place => place.Routes)
                .AsSplitQuery()
                .ToList();
        }
    }
}

