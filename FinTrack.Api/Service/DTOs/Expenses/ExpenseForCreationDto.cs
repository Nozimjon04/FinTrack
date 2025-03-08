namespace FinTrack.Api.Service.DTOs.Expenses;

public class ExpenseForCreationDto
{
    public required long ExpenseCategoryId { get; set; }
    public required decimal Amount { get; set; }
    public string Description { get; set; }
  
}
