using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service
{
    public static class UserService
    {
        public static async Task<IBaseResponse<User>> CreateUser(UserViewModel model, UserRepository userRepository)
        {
            try 
            {
                var user = new User()
                {
                    Id = model.Id,
                    Username = model.Username
                };

                var response = await userRepository.CreateAsync(user);

                return new BaseResponse<User>()
                {
                    Data = response,
                    Description = "Пользователь был создан",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = $"[UserService.CreateUser] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public static IBaseResponse<List<User>> GetUsers(UserRepository userRepository)
        {
            try
            {
                var response = userRepository.GetAll().ToList();

                return new BaseResponse<List<User>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<User>>()
                {
                    Description = $"[UserService.GetUsers] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
