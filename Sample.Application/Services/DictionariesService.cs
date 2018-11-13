using Sample.Application.Models;
using Sample.Core.Api;
using Sample.Core.Services;
using Sample.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Services
{
    public class DictionariesService: BaseService
    {
        public async Task<ServiceResult<PaginationViewModel<string>>> GetDictionaryKeys(ApiParameters apiParameters)
        {
            if (apiParameters == null)
            {
                apiParameters = new ApiParameters();
            }

            var serviceResult = new ServiceResult<PaginationViewModel<string>>();

            serviceResult.Data = (await UnitOfWork
                .RepositoryAsync<Dictionary>()
                .Query()
                .OrderBy(q => q.OrderBy(dictionary => dictionary.Key))
                .SelectPageAsync(apiParameters.Page, apiParameters.Limit, true))
                .Mutate(q => q.Key);

            return serviceResult;
        }

        public async Task<ServiceResult<string>> GetDictionaryValue(string key)
        {
            var serviceResult = new ServiceResult<string>();

            var foundDictionaryItem = await UnitOfWork
                .RepositoryAsync<Dictionary>()
                .Query(q => q.Key == key)
                .FirstOrDefaultAsync();

            if (foundDictionaryItem == null)
            {
                return serviceResult.SetStatus(HttpStatusCode.NotFound);
            }

            serviceResult.Data = foundDictionaryItem.Value;

            return serviceResult;
        }
    }
}
