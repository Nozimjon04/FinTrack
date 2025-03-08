using AutoMapper;
using FinTrack.Api.Domain.Models;
using FinTrack.Api.Data.IRepositories;
using FinTrack.Api.Service.Exceptions;
using FinTrack.Api.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using FinTrack.Api.Service.DTOs.ExpenseCategories;
using FinTrack.Api.Helpers;

namespace FinTrack.Api.Service.Services;

public class ExpenseCategoryService : IExpenseCategoryService
{
    private readonly IMapper mapper;
    private readonly IRepository<ExpenseCategory> expenseCategory;

    public ExpenseCategoryService(IMapper mapper, IRepository<ExpenseCategory> expenseCategory)
    {
        this.mapper = mapper;
        this.expenseCategory = expenseCategory;
    }

    public async Task<bool> AddAsync(ExpenseCategoryForCreationDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await this.expenseCategory.SelectAll()
            .Where(ec =>ec.Name.ToLower() == dto.Name.ToLower())
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is not null)
            throw new CustomException(409, $"{dto.Name} is already exists");

        var mappedEntity = this.mapper.Map<ExpenseCategory>(dto);
        mappedEntity.CreatedAt = DateTime.UtcNow;
        mappedEntity.UserId = HttpContextHelper.UserId.Value;
        await this.expenseCategory.InsertAsync(mappedEntity, cancellationToken);

        return await this.expenseCategory.SaveChangeAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await this.expenseCategory.SelectAll()
            .Where(ec => ec.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new CustomException(404, $"Expense Category with {id} not found");

        await this.expenseCategory.DeleteAsync(id, cancellationToken);
        return await this.expenseCategory.SaveChangeAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExpenseCategoryForResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await this.expenseCategory.SelectAll()
            .Where(ec => ec.UserId == HttpContextHelper.UserId.Value)
            .Include(ec => ec.Expenses)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return this.mapper.Map<IEnumerable<ExpenseCategoryForResultDto>>(entities);
    }

    public async Task<ExpenseCategoryForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await this.expenseCategory.SelectAll()
            .Where(ec => ec.Id == id)
            .Include(ec => ec.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new CustomException(404, $"Expense Category with {id} not found");

        return this.mapper.Map<ExpenseCategoryForResultDto>(entity);
    }

    public async Task<bool> UpdateAsync(long id, ExpenseCategoryForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await this.expenseCategory.SelectAll()
            .Where(ec => ec.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new CustomException(404, $"Expense Category with {id} not found");

        var mappedEntity = this.mapper.Map(dto, entity);
        mappedEntity.UpdatedAt = DateTime.UtcNow;

        return await this.expenseCategory.SaveChangeAsync(cancellationToken);
    }
}
