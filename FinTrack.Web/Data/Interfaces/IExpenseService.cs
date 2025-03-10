using FinTrack.Web.Helpers;
using FinTrack.Web.Models.Expenses;

namespace FinTrack.Web.Data.Interfaces;

public interface IExpenseService
{
    Task<ExpenseForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Response> AddAsync(ExpenseForCreationDto dto, CancellationToken cancellationToken = default);
    Task<Response> UpdateAsync(long id, ExpenseForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<Response> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResponse<IEnumerable<ExpenseForResultDto>>> RetrieveByCategoryIdAsync(long categoryId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResponse<IEnumerable<ExpenseForResultDto>>> SearchByNameAsync(string name, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResponse<IEnumerable<ExpenseForResultDto>>> RetrieveAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
