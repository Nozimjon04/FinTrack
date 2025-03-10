using AutoMapper;
using FinTrack.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using FinTrack.Api.Service.Exceptions;
using FinTrack.Api.Data.IRepositories;
using FinTrack.Api.Service.Interfaces;
using FinTrack.Api.Service.DTOs.Expenses;
using FinTrack.Api.Helpers;
using FinTrack.Api.Domain.Configurations;
using FinTrack.Api.Service.Extensions;

namespace FinTrack.Api.Service.Services;

public class ExpenseService : IExpenseService
{
    private readonly IMapper mapper;
    private readonly IRepository<Expense> expenseRepository;
    private readonly IRepository<ExpenseCategory> expenseCategoryRepository;

    public ExpenseService(IMapper mapper, IRepository<Expense> expenseRepository, IRepository<ExpenseCategory> expenseCategoryRepository)
    {
        this.mapper = mapper;
        this.expenseRepository = expenseRepository;
        this.expenseCategoryRepository = expenseCategoryRepository;
    }

    public async Task<bool> AddAsync(ExpenseForCreationDto dto, CancellationToken cancellationToken = default)
    {
        var category = await this.expenseCategoryRepository.SelectAll()
            .Where(c => c.Id == dto.ExpenseCategoryId)
            .AsNoTracking()
            .FirstOrDefaultAsync(ec => ec.Id == dto.ExpenseCategoryId, cancellationToken);
        if (category is null)
            throw new CustomException(404, $"Expense Category with {dto.ExpenseCategoryId} not found");

        var mappedEntity = this.mapper.Map<Expense>(dto);
        mappedEntity.CreatedAt = DateTime.UtcNow;
        mappedEntity.UserId = HttpContextHelper.UserId.Value;
        await this.expenseRepository.InsertAsync(mappedEntity, cancellationToken);

        return await this.expenseRepository.SaveChangeAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await this.expenseRepository.SelectAll()
            .Where(e => e.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new CustomException(404, $"Expense with {id} not found");

        await this.expenseRepository.DeleteAsync(id, cancellationToken);
        return await this.expenseRepository.SaveChangeAsync(cancellationToken);
    }

    public async Task<IEnumerable<ExpenseForResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default)
    {
        var entities = await this.expenseRepository.SelectAll()
            .Where(ec => ec.UserId == HttpContextHelper.UserId.Value)
            .Include(e => e.ExpenseCategory)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<ExpenseForResultDto>>(entities);
    }

    public async Task<IEnumerable<ExpenseForResultDto>> RetrieveByCategoryIdAsync(long categoryId, PaginationParams @params, CancellationToken cancellationToken = default)
    {
        var entities = await this.expenseRepository.SelectAll()
            .Where(e => e.ExpenseCategoryId == categoryId && e.UserId == HttpContextHelper.UserId.Value)
            .Include(e => e.ExpenseCategory)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<ExpenseForResultDto>>(entities);
    }

    public async Task<ExpenseForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await this.expenseRepository.SelectAll()
            .Where(e => e.Id == id)
            .Include(e => e.ExpenseCategory)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new CustomException(404, $"Expense with {id} not found");

        return this.mapper.Map<ExpenseForResultDto>(entity);
    }

    public async Task<IEnumerable<ExpenseForStatisticsDto>> RetrieveMonthlyStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var entities = await this.expenseRepository.SelectAll()
            .Where(e => e.UserId == HttpContextHelper.UserId.Value)
            .AsNoTracking()
            .GroupBy(e => new { e.CreatedAt.Year, e.CreatedAt.Month })
            .Select(g => new ExpenseForStatisticsDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalAmount = g.Sum(e => e.Amount)
            })
            .OrderByDescending(e => e.Year)
            .ThenByDescending(e => e.Month)
            .ToListAsync();

        return entities;
    }

    public async Task<IEnumerable<ExpenseForResultDto>> SearchByNameAsync(string name, PaginationParams @params,CancellationToken cancellationToken = default)
    {
        var entities = await this.expenseRepository.SelectAll()
            .Where(e => e.Description.ToLower().Contains(name.ToLower()) && e.UserId == HttpContextHelper.UserId.Value)
            .Include(e => e.ExpenseCategory)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<ExpenseForResultDto>>(entities);
    }

    public async Task<bool> UpdateAsync(long id, ExpenseForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await this.expenseRepository.SelectAll()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new CustomException(404, $"Expense with {id} not found");

        var category = await this.expenseCategoryRepository.SelectAll()
            .Where(c => c.Id == dto.ExpenseCategoryId)
            .AsNoTracking()
            .FirstOrDefaultAsync(ec => ec.Id == dto.ExpenseCategoryId, cancellationToken);
        if (category is null)
            throw new CustomException(404, $"Expense Category with {dto.ExpenseCategoryId} not found");

        var mappedEntity = this.mapper.Map(dto, entity);
        mappedEntity.UpdatedAt = DateTime.UtcNow;

        return await this.expenseRepository.SaveChangeAsync(cancellationToken);
    }
}
