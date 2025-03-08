using FinTrack.Api.Domain.Models.Commons;

namespace FinTrack.Api.Domain.Models;

public class Expense : Auditable
{
    public required long ExpenseCategoryId { get; set; }
    public required ExpenseCategory ExpenseCategory { get; set; }
    public required decimal Amount { get; set; }
    public string Description { get; set; }
    public required long UserId { get; set; }
    public required User User { get; set; }
}
