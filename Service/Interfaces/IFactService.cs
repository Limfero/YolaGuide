using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service.Interfaces
{
    public interface IFactService
    {
        Task<IBaseResponse<Fact>> CreateFactAsync(FactViewModel model);

        IBaseResponse<List<Fact>> GetAllFact();

        IBaseResponse<Fact> GetFactByName(string name);

        Task<IBaseResponse<Fact>> RemoveFactAsync(Fact fact);
    }
}
