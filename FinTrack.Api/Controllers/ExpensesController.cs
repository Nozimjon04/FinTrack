using FinTrack.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Api.Service.DTOs.Expenses;
using FinTrack.Api.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using FinTrack.Api.Domain.Configurations;

namespace FinTrack.Api.Controllers;

[Authorize]
public class ExpensesController(IExpenseService expenseService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.RetrieveAllAsync(@params,cancellationToken)
        });
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.RetrieveByIdAsync(id, cancellationToken)
        });
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] ExpenseForCreationDto dto, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.AddAsync(dto, cancellationToken)
        });
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.DeleteAsync(id, cancellationToken)
        });
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] ExpenseForUpdateDto dto, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.UpdateAsync(id, dto, cancellationToken)
        });
    [HttpGet("category/{categoryId:long}")]
    public async Task<IActionResult> GetByCategoryIdAsync(long categoryId, [FromQuery] PaginationParams @params, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.RetrieveByCategoryIdAsync(categoryId, @params, cancellationToken)
        });
    [HttpGet("search")]
    public async Task<IActionResult> SearchByNameAsync([FromQuery] string name, [FromQuery] PaginationParams @params, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.SearchByNameAsync(name, @params, cancellationToken)
        });
    [HttpGet("statistics")]
    public async Task<IActionResult> GetMonthlyStatisticsAsync(CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.RetrieveMonthlyStatisticsAsync(cancellationToken)
        });
}
