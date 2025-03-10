using FinTrack.Web.Helpers;
using FinTrack.Web.Models.ExpenseCategories;

namespace FinTrack.Web.Data.Interfaces;

public interface IExpenseCategoryService
{
    Task<IEnumerable<ExpenseCategoryForResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
    Task<ExpenseCategoryForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Response> AddAsync(ExpenseCategoryForCreationDto dto, CancellationToken cancellationToken = default);
    Task<Response> UpdateAsync(long id, ExpenseCategoryForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<Response> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
