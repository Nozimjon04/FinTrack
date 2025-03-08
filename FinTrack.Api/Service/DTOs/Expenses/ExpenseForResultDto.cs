namespace FinTrack.Api.Service.DTOs.Expenses;

public class ExpenseForResultDto
{
    public long Id { get; set; }
    public long ExpenseCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
