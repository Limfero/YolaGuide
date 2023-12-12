﻿using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service
{
    internal class FactService
    {
        private readonly IFactRepository _factRepository;

        public FactService(IFactRepository factRepository)
        {
            _factRepository = factRepository;
        }

        public async Task<IBaseResponse<Fact>> CreateFact(FactViewModel model)
        {
            try
            {
                var fact = new Fact()
                {
                    Name = model.Name,
                    Description = model.Description,
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
    }
}