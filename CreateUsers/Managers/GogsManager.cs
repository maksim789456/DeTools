using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CreateUsers.Models;
using DotNetEnv;

namespace CreateUsers.Managers;

public sealed class GogsResponse<T>
{
    public List<T> Data { get; set; }
}

public sealed class GogsUser
{
    [JsonPropertyName("login")] public string Username { get; set; } = string.Empty;
}

public sealed class CreateUserRequest
{
    [JsonPropertyName("login_name")] public string LoginName => Username;

    [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")] public string Password { get; set; } = string.Empty;

    [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;

    [JsonPropertyName("send_notify")] public bool SendNotify => false;
}

public class GogsManager : IAccountManager
{
    private readonly string _gogsUrl = Env.GetString("GOGS_URL");
    private readonly string _gogsEmail = Env.GetString("GOGS_EMAIL", "de.test.ru");
    private Uri _gogsUri = null!;
    private readonly string _gogsAdminApiKey = Env.GetString("GOGS_ADMIN_TOKEN");

    private readonly HttpClient _httpClient = new();

    public async Task<(bool, string)> CheckAccess()
    {
        if (string.IsNullOrWhiteSpace(_gogsUrl))
            return (false, "Отсутствует адрес сервера Gogs");
        try
        {
            _gogsUri = new Uri(_gogsUrl);
        }
        catch (Exception e)
        {
            return (false, "Некорректный адрес сервера Gogs: " + e.Message);
        }

        try
        {
            using var gogsAliveRes =
                await _httpClient.GetAsync(_gogsUri);
            if (!gogsAliveRes.IsSuccessStatusCode)
                return (false, "Сервер Gogs недоступен или не отвечает");

            if (string.IsNullOrWhiteSpace(_gogsAdminApiKey))
                return (false, "Отсутствует админский API ключ");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {_gogsAdminApiKey}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            using var gogsAdminRes =
                await _httpClient.GetAsync(new Uri(_gogsUri, "/api/v1/admin/teams/-1/members"));
            if (gogsAdminRes.StatusCode == HttpStatusCode.Forbidden)
                return (false, "Некорректный ключ администратора");
        }
        catch (Exception e)
        {
            return (false, "Ошибка подключения к Gogs: " + e.Message);
        }

        return (true, "");
    }

    public async Task<IReadOnlyList<string>> GetUsers(string? prefix)
    {
        var url = new Uri(_gogsUri, $"/api/v1/users/search?q={prefix}&limit=100");
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var users =
            await response.Content.ReadFromJsonAsync<GogsResponse<GogsUser>>() ?? new GogsResponse<GogsUser>();
        var result = users.Data.Select(x => x.Username);

        return result.ToList();
    }

    public async Task<bool> CreateUser(User user)
    {
        var url = new Uri(_gogsUri, "/api/v1/admin/users");
        var request = new CreateUserRequest
        {
            Username = user.Username,
            Password = user.Password,
            Email = $"{user.Username}@{_gogsEmail}"
        };

        using var response = await _httpClient.PostAsJsonAsync(url, request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUser(string username)
    {
        var url = new Uri(_gogsUri, $"/api/v1/admin/users/{Uri.EscapeDataString(username)}");
        using var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}