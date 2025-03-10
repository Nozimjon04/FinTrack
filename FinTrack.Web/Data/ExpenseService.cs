using Blazored.LocalStorage;
using FinTrack.Web.Data.Interfaces;
using FinTrack.Web.Helpers;
using FinTrack.Web.Models.Expenses;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FinTrack.Web.Data;

public class ExpenseService : IExpenseService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ExpenseService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    public async Task<Response> AddAsync(ExpenseForCreationDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader(); // Add token before request
            var response = await _httpClient.PostAsJsonAsync("/api/expenses", dto);

            var responseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the expense. Please try again.", ex);
        }

    }

    public async Task<Response> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"/api/expenses/{id}");

            var responseContent = response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response>(responseContent.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the expense. Please try again.", ex);
        }
    }

    public async Task<PagedResponse<IEnumerable<ExpenseForResultDto>>> RetrieveAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var requestUri = $"/api/expenses?PageSize={pageSize}&PageIndex={page}";
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to retrieve expenses.");

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseContentObj = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var expenses = JsonSerializer.Deserialize<IEnumerable<ExpenseForResultDto>>(responseContentObj.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            // Extract Pagination Headers
            var paginationHeader = response.Headers.GetValues("x-pagination").FirstOrDefault();
            var paginationData = JsonSerializer.Deserialize<PaginationMetadata>(paginationHeader);

            return new PagedResponse<IEnumerable<ExpenseForResultDto>>
            {
                Data = expenses,
                Pagination = paginationData
            };

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving expenses. Please try again.", ex);
        }
    }

    public async Task<PagedResponse<IEnumerable<ExpenseForResultDto>>> RetrieveByCategoryIdAsync(long categoryId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var requestUri = $"/api/expenses/category/{categoryId}?PageSize={pageSize}&PageIndex={page}";
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to retrieve expenses.");

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseContentObj = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var expenses = JsonSerializer.Deserialize<IEnumerable<ExpenseForResultDto>>(responseContentObj.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Extract Pagination Headers
            var paginationHeader = response.Headers.GetValues("x-pagination").FirstOrDefault();
            var paginationData = JsonSerializer.Deserialize<PaginationMetadata>(paginationHeader);

            return new PagedResponse<IEnumerable<ExpenseForResultDto>>
            {
                Data = expenses,
                Pagination = paginationData
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving expenses by category. Please try again.", ex);
        }
    }

    public async Task<ExpenseForResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.GetFromJsonAsync<Response>($"/api/expenses/{id}");

            var expense = JsonSerializer.Deserialize<ExpenseForResultDto>(response.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return expense;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the expense. Please try again.", ex);
        }

    }

    public async Task<PagedResponse<IEnumerable<ExpenseForResultDto>>> SearchByNameAsync(string name, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var requestUri = $"/api/expenses/search?name={name}&PageSize={pageSize}&PageIndex={page}";
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to retrieve expenses.");

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseContentObj = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var expenses = JsonSerializer.Deserialize<IEnumerable<ExpenseForResultDto>>(responseContentObj.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Extract Pagination Headers
            var paginationHeader = response.Headers.GetValues("x-pagination").FirstOrDefault();
            var paginationData = JsonSerializer.Deserialize<PaginationMetadata>(paginationHeader);

            return new PagedResponse<IEnumerable<ExpenseForResultDto>>
            {
                Data = expenses,
                Pagination = paginationData
            };
          
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while searching for expenses. Please try again.", ex);
        }
    }

    public async Task<Response> UpdateAsync(long id, ExpenseForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"/api/expenses/{id}", dto);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the expense. Please try again.", ex);
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
