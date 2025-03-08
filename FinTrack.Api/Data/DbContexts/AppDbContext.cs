using FinTrack.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExpenseCategory>()
            .HasMany(e => e.Expenses)
            .WithOne(e => e.ExpenseCategory)
            .HasForeignKey(e => e.ExpenseCategoryId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.ExpenseCategories)
            .WithOne(ec => ec.User)
            .HasForeignKey(ec => ec.UserId);
    }
}
