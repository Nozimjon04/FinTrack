using FinTrack.Api.Helpers;
using FinTrack.Api.Service.DTOs.ExpenseCategories;
using FinTrack.Api.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[Authorize]
public class ExpenseCategoriesController(IExpenseCategoryService expenseCategoryService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseCategoryService.RetrieveAllAsync(cancellationToken)
        });

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseCategoryService.RetrieveByIdAsync(id, cancellationToken)
        });
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] ExpenseCategoryForCreationDto dto, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseCategoryService.AddAsync(dto, cancellationToken)
        });
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseCategoryService.DeleteAsync(id, cancellationToken)
        });
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] ExpenseCategoryForUpdateDto dto, CancellationToken cancellationToken = default)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await expenseCategoryService.UpdateAsync(id, dto, cancellationToken)
        });

}
