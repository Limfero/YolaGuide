using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;
using YolaGuide.DAL.Repositories.Interfaces;

namespace YolaGuide.Service
{
    public class PlaceService
    {
        private readonly IPlaceRepository _placeRepository;

        public PlaceService(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public async Task<IBaseResponse<Place>> CreatePlace(PlaceViewModel model)
        {
            try
            {
                var place = new Place() 
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image,
                    ContactInformation = model.ContactInformation,
                    YIdOrganization = model.YIdOrganization,
                    Coordinates = model.Coordinates,
                    Categories = model.Categories
                };

                await _placeRepository.CreateAsync(place);

                return new BaseResponse<Place>()
                {
                    Description = "Место успешно создано",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Place>()
                {
                    Description = $"[PlaceService.CreatePlace] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Place>> GetPlacesByName(string name)
        {
            try
            {
                var places = _placeRepository.GetPlacesByName(name);


                return new BaseResponse<List<Place>>()
                {
                    Data = places,
                    Description = "Места найдены!",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Place>>()
                {
                    Description = $"[PlaceService.GetAdressesPlace] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Place>> GetPlacesByCategory(Category category)
        {
            try
            {
                var places = _placeRepository.GetPlacesByCategory(category);

                return new BaseResponse<List<Place>>()
                {
                    Data = places,
                    Description = "Места найдены!",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Place>>()
                {
                    Description = $"[PlaceService.GetPlacesByCategory] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<Place> GetPlaceById(int id)
        {
            try
            {
                var places = _placeRepository.GetPlaceById(id);

                return new BaseResponse<Place>()
                {
                    Data = places,
                    Description = "Место найдены!",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Place>()
                {
                    Description = $"[PlaceService.GetPlaceById] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Place>> GetAllPlace()
        {
            try
            {
                var places = _placeRepository.GetAll().ToList();

                return new BaseResponse<List<Place>>()
                {
                    Data = places,
                    Description = "Места найдены!",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Place>>()
                {
                    Description = $"[PlaceService.GetAllPlace] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Place>> SearchPlace(string userInput)
        {
            try
            {
                var places = _placeRepository.Search(userInput);

                return new BaseResponse<List<Place>>()
                {
                    Data = places,
                    Description = "Места найдены!",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Place>>()
                {
                    Description = $"[PlaceService.SearchPlace] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Place>> RemovePlaceAsync(Place place)
        {
            try
            {
                await _placeRepository.RemoveAsync(place);

                return new BaseResponse<Place>()
                {
                    Description = "Место удалено!",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Place>()
                {
                    Description = $"[PlaceService.RemovePlaceAsync] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

    }
}
