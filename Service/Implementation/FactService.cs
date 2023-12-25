using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;
using YolaGuide.Service.Interfaces;

namespace YolaGuide.Service.Implementation
{
    public class FactService : IFactService
    {
        private readonly IFactRepository _factRepository;

        public FactService(IFactRepository factRepository)
        {
            _factRepository = factRepository;
        }

        public async Task<IBaseResponse<Fact>> CreateFactAsync(FactViewModel model)
        {
            try
            {
                var fact = new Fact()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image
                };

                var response = await _factRepository.CreateAsync(fact);

                return new BaseResponse<Fact>()
                {
                    Data = response,
                    Description = "Факт успешно создан",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Fact>()
                {
                    Description = $"[FactService.CreateFact] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<List<Fact>> GetAllFact()
        {
            try
            {
                var facts = _factRepository.GetAll().ToList();

                return new BaseResponse<List<Fact>>()
                {
                    Data = facts,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Fact>>()
                {
                    Description = $"[FactService.GetAllFact] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<Fact> GetFactByName(string name)
        {
            try
            {
                var fact = _factRepository.GetFactByName(name);

                return new BaseResponse<Fact>()
                {
                    Data = fact,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Fact>()
                {
                    Description = $"[FactService.GetFactByName] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Fact>> RemoveFactAsync(Fact fact)
        {
            try
            {
                await _factRepository.RemoveAsync(fact);

                return new BaseResponse<Fact>()
                {
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Fact>()
                {
                    Description = $"[FactService.RemoveFactAsync] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
