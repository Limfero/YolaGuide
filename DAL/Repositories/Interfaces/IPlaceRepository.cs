using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Interfaces
{
    public interface IPlaceRepository : IBaseRepository<Place>
    {
        List<Place> GetPlacesByCategory(Category category);

        List<Place> GetPlacesByName(string name);

        Place GetPlaceById(int id);
    }
}
