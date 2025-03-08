using FinTrack.Api.Domain.Models.Commons;

namespace FinTrack.Api.Domain.Models;

public class User : Auditable
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<ExpenseCategory>? ExpenseCategories { get; set; }
}
