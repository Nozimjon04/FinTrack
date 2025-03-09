using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = await LoadTokenAsync();

        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    private async Task<string> LoadTokenAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        // 🛑 Ensure token is valid before using it
        if (string.IsNullOrEmpty(token) || !IsValidJwt(token))
        {
            await _localStorage.RemoveItemAsync("authToken"); // Remove bad token
            return null;
        }

        return token;
    }
    public void NotifyUserAuthentication(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token) || !token.Contains("."))
                return new List<Claim>();

            var payload = token.Split('.')[1]; // Extract payload part of JWT

            // Ensure proper padding for Base64
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JWT Parsing Error: {ex.Message}");
            return new List<Claim>(); // Return empty claims if parsing fails
        }
    }

    // ✅ Helper function to check if token is a valid JWT
    private bool IsValidJwt(string token)
    {
        var parts = token?.Split('.');
        return parts?.Length == 3; // JWT should have 3 parts (Header.Payload.Signature)
    }
}
