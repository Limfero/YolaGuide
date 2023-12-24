using Microsoft.EntityFrameworkCore;
using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class FactRepository : BaseRepository<Fact>, IFactRepository
    {
        public FactRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Fact GetFactByName(string name)
        {
            return _dbContext.Facts
                .FirstOrDefault(fact => fact.Name.Contains(name));
        }
    }
}
