using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.DAL;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service
{
    public static class PlaceService
    {
        private static readonly PlaceRepository _placeRepository = new(new ApplicationDbContext(new()));

        public static async Task<IBaseResponse<Place>> AddPlace(PlaceViewModel model)
        {
            try
            {
                var place = new Place() 
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image,
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
