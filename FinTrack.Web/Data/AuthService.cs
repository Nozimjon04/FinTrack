using Blazored.LocalStorage;
using FinTrack.Web.Helpers;
using FinTrack.Web.Models;
using Microsoft.AspNetCore.Components.Authorization;


namespace FinTrack.Web.Data;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(
        IHttpClientFactory httpClientFactory, 
        ILocalStorageService localStorage, 
        AuthenticationStateProvider authStateProvider)
    {
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
        _httpClient = httpClientFactory.CreateClient("Api");
    }
    public async Task<Response> RegisterAsync(UserForCreationDto user)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

            // Deserialize the response into our Response class
            var responseContent = await response.Content.ReadFromJsonAsync<Response>();

            if (!response.IsSuccessStatusCode || responseContent == null)
            {
                return new Response
                {
                    Code = responseContent.Code,
                    Message = responseContent.Message ,
                    Data = responseContent.Data
                };
            }

            // Extract token if present and store it
            var token = responseContent.Data?.ToString();
            if (!string.IsNullOrEmpty(token))
            {
                await _localStorage.SetItemAsync("authToken", token);
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);
            }

            return new Response
            {
                Code = (int)response.StatusCode,
                Message = "Registration successful",
                Data = token
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing the request", ex);

        }
    }


    public async Task<Response> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/auth/login?email={email}&password={password}", null);

            var responseContent = await response.Content.ReadFromJsonAsync<Response>();

            if (!response.IsSuccessStatusCode)
            {
                return new Response
                {
                    Code = responseContent.Code,
                    Message = responseContent.Message,
                    Data = responseContent.Data
                };
            }

            var token = await response.Content.ReadAsStringAsync();

            await _localStorage.SetItemAsync("authToken", token);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);

            return new Response
            {
                Code = responseContent.Code,
                Message = responseContent.Message,
                Data = responseContent.Data
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing the request", ex);
        }
        
    }
    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}
