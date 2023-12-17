using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service
{
    public class RouteService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<IBaseResponse<Route>> CreateRouteAsync(RouteViewModel model)
        {
            try
            {
                var route = new Route()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Cost = model.Cost,
                    Telephone = model.Telephone
                };

                await _routeRepository.CreateAsync(route);

                return new BaseResponse<Route>()
                {
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Route>()
                {
                    Description = $"[RouteService.CreateRouteAsync] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Route>> RemoveRouteAsync(Route route)
        {
            try
            {
                await _routeRepository.RemoveAsync(route);

                return new BaseResponse<Route>()
                {
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Route>()
                {
                    Description = $"[RouteService.RemoveRouteAsync] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Route>> GetAllRoutes()
        {
            try
            {
                var routes = _routeRepository.GetAll().ToList();

                return new BaseResponse<List<Route>>()
                {
                    Data = routes,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Route>>()
                {
                    Description = $"[RouteService.GetAllRoutes] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<Route> GetRouteByName(string name)
        {
            try
            {
                var routes = _routeRepository.GetRouteByName(name);


                return new BaseResponse<Route>()
                {
                    Data = routes,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Route>()
                {
                    Description = $"[RouteService.GetRouteByName] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
