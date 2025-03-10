using Blazored.LocalStorage;
using FinTrack.Web.Data.Interfaces;
using FinTrack.Web.Helpers;
using FinTrack.Web.Models.ExpenseCategories;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FinTrack.Web.Data;

public class ExpenseCategoryService : IExpenseCategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;


    public ExpenseCategoryService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    public async Task<Response> AddAsync(ExpenseCategoryForCreationDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("/api/expense-categories", dto);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the expense category. Please try again.", ex);
        }
    }

    public async Task<Response> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"/api/expense-categories/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();

           return JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
           {
                PropertyNameCaseInsensitive = true
           });

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the expense category. Please try again.", ex);
        }
    }

    public async Task<IEnumerable<ExpenseCategoryForResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.GetFromJsonAsync<Response>("/api/expense-categories");

            var expenseCategories = JsonSerializer.Deserialize<IEnumerable<ExpenseCategoryForResultDto>>(response.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return expenseCategories;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the expense categories. Please try again.", ex);
        }
    }

    public async Task<ExpenseCategoryForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.GetFromJsonAsync<Response>($"/api/expense-categories/{id}");

            var expenseCategory = JsonSerializer.Deserialize<ExpenseCategoryForResultDto>(response.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return expenseCategory;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the expense category. Please try again.", ex);
        }
    }

    public async Task<Response> UpdateAsync(long id, ExpenseCategoryForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"/api/expense-categories/{id}", dto);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the expense category. Please try again.", ex);
        }
    }
    private async Task SetAuthorizationHeader()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
