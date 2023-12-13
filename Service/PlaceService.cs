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

        public IBaseResponse<List<Place>> GetPlaceByCategory(Category category)
        {
            try
            {
                var places = _placeRepository.GetPlaceByCategory(category);

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
                    Description = $"[PlaceService.GetPlaceByCategory] - {ex.Message}",
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
                    Description = $"[PlaceService.GetPlaceByCategory] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

    }
}
