using YolaGuide.DAL;
using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service;

namespace YolaGuide.Controllers
{
    public static class UserController
    {
        private static readonly UserService _userService = new(new UserRepository(new ApplicationDbContext(new())));

        public static async Task CreateUser(UserViewModel model)
        {
            var response = await _userService.CreateUserAsync(model);

            if(response.StatusCode != Domain.Enums.StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static async Task UpdateUser(User user) 
        {
            var response = await _userService.Update(user);

            if (response.StatusCode != Domain.Enums.StatusCode.OK)
                throw new Exception(response.Description);
        }

        public static List<User> GetAllUsers()
        {
            var response = _userService.GetUsers();

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public static User GetUserById(long id)
        {
            var response = _userService.GetUserById(id);

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }
    }
}
