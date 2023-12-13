using Microsoft.EntityFrameworkCore;
using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public User GetUserById(long id)
        {
            return _dbContext.Users
                .Include(user => user.Categories)
                .Include(user => user.Routes)
                .Include(user => user.Places)
                .AsSplitQuery()
                .FirstOrDefault(user => user.Id == id);
        }
    }
}
