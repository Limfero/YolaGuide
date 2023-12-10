using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class UserRepository : BaseRepository<User>, IBaseRepository<User>
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public User GetUserById(long id)
        {
            return _dbContext.Set<User>().FirstOrDefault(user => user.Id == id);
        }
    }
}
