using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<User>> CreateUserAsync(UserViewModel model);

        IBaseResponse<List<User>> GetUsers();

        IBaseResponse<User> GetUserById(long id);

        Task<IBaseResponse<User>> Update(User user);
    }
}
