namespace FinTrack.Web.Models.Expenses;

public class ExpenseForUpdateDto
{
    public required long ExpenseCategoryId { get; set; }
    public required decimal Amount { get; set; }
    public string Description { get; set; }
}
