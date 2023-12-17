using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IBaseResponse<User>> CreateUserAsync(UserViewModel model)
        {
            try 
            {
                var user = new User()
                {
                    Id = model.Id,
                    Username = model.Username,
                    State = State.Start,
                    Substate = Substate.Start,
                };

                var response = await _userRepository.CreateAsync(user);

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

        public IBaseResponse<List<User>> GetUsers()
        {
            try
            {
                var response = _userRepository.GetAll().ToList();

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

        public IBaseResponse<User> GetUserById(long id)
        {
            try
            {
                var response = _userRepository.GetUserById(id);

                return new BaseResponse<User>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = $"[UserService.GetUserById] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> Update(User user)
        {
            try
            {
                await _userRepository.UpdateAsync(user);

                return new BaseResponse<User>()
                {
                    Description = "Пользователь обновлен",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = $"[UserService.Update] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
