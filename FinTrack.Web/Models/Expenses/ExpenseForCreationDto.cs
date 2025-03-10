namespace FinTrack.Web.Models.Expenses;

public class ExpenseForCreationDto
{
    public long ExpenseCategoryId { get; set; }
    public  decimal Amount { get; set; }
    public string Description { get; set; }
}
