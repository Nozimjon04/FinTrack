using FinTrack.Api.Domain.Models;
using FinTrack.Api.Service.DTOs.Expenses;

namespace FinTrack.Api.Service.DTOs.ExpenseCategories;

public class ExpenseCategoryForResultDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<ExpenseForResultDto>? Expenses { get; set; }
}
