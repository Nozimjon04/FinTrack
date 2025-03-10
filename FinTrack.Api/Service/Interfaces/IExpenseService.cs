using FinTrack.Api.Domain.Configurations;
using FinTrack.Api.Service.DTOs.Expenses;

namespace FinTrack.Api.Service.Interfaces;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseForResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default);
    Task<ExpenseForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(ExpenseForCreationDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, ExpenseForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExpenseForResultDto>> RetrieveByCategoryIdAsync(long categoryId, PaginationParams @params, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExpenseForResultDto>> SearchByNameAsync(string name, PaginationParams @params, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExpenseForStatisticsDto>> RetrieveMonthlyStatisticsAsync(CancellationToken cancellationToken = default);
}
