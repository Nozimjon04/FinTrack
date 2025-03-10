using FinTrack.Web.Models.ExpenseCategories;

namespace FinTrack.Web.Models.Expenses;

public class ExpenseForResultDto
{
    public long Id { get; set; }
    public long ExpenseCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public ExpenseCategoryForResultDto ExpenseCategory { get; set; }
}
