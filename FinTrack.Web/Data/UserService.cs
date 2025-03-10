using FinTrack.Web.Data.Interfaces;
using FinTrack.Web.Models;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using System.Text.Json;
using FinTrack.Web.Helpers;

namespace FinTrack.Web.Data;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public UserService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    public async Task<UserForResultDto> RetrieveProfileAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.GetFromJsonAsync<Response>("api/users");
            var user = JsonSerializer.Deserialize<UserForResultDto>(response.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return user;

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the profile. Please try again.", ex);
        }
    }

    public async Task<Response> UpdateProfileAsync(long id, UserForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", dto);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the profile. Please try again.", ex);
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
