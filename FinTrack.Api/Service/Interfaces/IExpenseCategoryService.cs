using FinTrack.Api.Service.DTOs.ExpenseCategories;

namespace FinTrack.Api.Service.Interfaces;

public interface IExpenseCategoryService
{
    Task<IEnumerable<ExpenseCategoryForResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
    Task<ExpenseCategoryForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(ExpenseCategoryForCreationDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, ExpenseCategoryForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);

}
