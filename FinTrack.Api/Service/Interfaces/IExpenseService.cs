using FinTrack.Api.Service.DTOs.Expenses;

namespace FinTrack.Api.Service.Interfaces;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseForResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default);
    Task<ExpenseForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(ExpenseForCreationDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, ExpenseForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
