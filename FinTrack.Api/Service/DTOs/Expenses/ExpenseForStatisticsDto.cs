namespace FinTrack.Api.Service.DTOs.Expenses;

public class ExpenseForStatisticsDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalAmount { get; set; }
}
