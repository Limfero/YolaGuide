using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Interfaces
{
    public interface IPlaceRepository : IBaseRepository<Place>
    {
        List<Place> GetPlaceByCategory(Category category);
    }
}
