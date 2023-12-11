using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class PlaceRepository : BaseRepository<Place>
    {
        public PlaceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

