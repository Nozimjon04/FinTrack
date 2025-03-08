using FinTrack.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Api.Service.DTOs.Expenses;
using FinTrack.Api.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FinTrack.Api.Controllers;

[Authorize]
public class ExpensesController(IExpenseService expenseService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseService.RetrieveAllAsync(cancellationToken)
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

}
