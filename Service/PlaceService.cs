using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;
using YolaGuide.DAL.Repositories.Implimentation;

namespace YolaGuide.Service
{
    public class PlaceService
    {
        private readonly PlaceRepository _placeRepository;

        public PlaceService(PlaceRepository placeRepository)
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
                    Adress = model.Adress,
                    YIdOrganization = model.YIdOrganization,
                    Coordinates = model.Coordinates,
                    Categories = model.Categories
                };

                var response = await _placeRepository.CreateAsync(place);

                return new BaseResponse<Place>()
                {
                    Data = response,
                    Description = "Место успешно создано",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Place>()
                {
                    Description = $"[PlaceService.AddPlace] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

    }
}
