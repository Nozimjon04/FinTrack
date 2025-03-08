using FinTrack.Api.Domain.Models.Commons;

namespace FinTrack.Api.Domain.Models;

public class ExpenseCategory : Auditable
{
    public required string Name { get; set; }
    public required long UserId { get; set; }
    public required User User { get; set; }
    public ICollection<Expense>? Expenses { get; set; }
}
