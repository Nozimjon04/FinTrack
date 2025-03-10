using FinTrack.Web.Models.Expenses;

namespace FinTrack.Web.Models.ExpenseCategories;

public class ExpenseCategoryForResultDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<ExpenseForResultDto> Expenses { get; set; }
}
