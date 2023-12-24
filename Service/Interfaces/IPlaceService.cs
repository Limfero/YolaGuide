using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service.Interfaces
{
    public interface IPlaceService
    {
        Task<IBaseResponse<Place>> CreatePlace(PlaceViewModel model);

        IBaseResponse<List<Place>> GetPlacesByName(string name);

        IBaseResponse<List<Place>> Search(string userInput);

        IBaseResponse<List<Place>> GetPlacesByCategory(Category category);

        IBaseResponse<Place> GetPlaceById(int id);

        IBaseResponse<List<Place>> GetAllPlace();

        Task<IBaseResponse<Place>> RemovePlaceAsync(Place place);
    }
}
