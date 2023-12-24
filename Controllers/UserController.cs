using YolaGuide.Domain.Entity;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service.Interfaces;

namespace YolaGuide.Controllers
{
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task CreateUser(UserViewModel model)
        {
            var response = await _userService.CreateUserAsync(model);

            if(response.StatusCode != Domain.Enums.StatusCode.OK)
                throw new Exception(response.Description);
        }

        public async Task UpdateUser(User user)  
        {
            var response = await _userService.Update(user);

            if (response.StatusCode != Domain.Enums.StatusCode.OK)
                throw new Exception(response.Description);
        }

        public List<User> GetAllUsers()
        {
            var response = _userService.GetUsers();

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }

        public User GetUserById(long id)
        {
            var response = _userService.GetUserById(id);

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
                return response.Data;

            throw new Exception(response.Description);
        }
    }
}
