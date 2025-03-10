using FinTrack.Api.Service.DTOs.ExpenseCategories;

namespace FinTrack.Api.Service.DTOs.Expenses;

public class ExpenseForResultDto
{
    public long Id { get; set; }
    public ExpenseCategoryForResultDto ExpenseCategory { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
