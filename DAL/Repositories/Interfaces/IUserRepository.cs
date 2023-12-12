
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetUserById(long id);
    }
}
