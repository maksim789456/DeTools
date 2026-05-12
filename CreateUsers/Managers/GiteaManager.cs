using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CreateUsers.Models;
using DotNetEnv;

namespace CreateUsers.Managers;

public sealed class GiteaUser
{
    [JsonPropertyName("login")] public string Username { get; set; } = string.Empty;
}

public sealed class GiteaCreateUserRequest
{
    [JsonPropertyName("login_name")] public string LoginName => Username;
    [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
    [JsonPropertyName("password")] public string Password { get; set; } = string.Empty;
    [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
    [JsonPropertyName("send_notify")] public bool SendNotify => false;
    [JsonPropertyName("must_change_password")] public bool MustChangePass => false;
    [JsonPropertyName("visibility")] public string Visibility => "private";
}

public class GiteaManager: IAccountManager
{
    public string Name => "Gitea";

    private readonly string _giteaUrl = Env.GetString("GITEA_URL");
    private readonly string _gitEmail = Env.GetString("GIT_EMAIL", "de.test.ru");
    private Uri _giteaUri = null!;
    private readonly string _giteaAdminApiKey = Env.GetString("GITEA_ADMIN_TOKEN");

    private readonly HttpClient _httpClient = new();

    public async Task<(bool, string)> CheckAccess()
    {
        if (string.IsNullOrWhiteSpace(_giteaUrl))
            return (false, "Отсутствует адрес сервера Gitea");
        try
        {
            _giteaUri = new Uri(_giteaUrl);
        }
        catch (Exception e)
        {
            return (false, "Некорректный адрес сервера Gitea: " + e.Message);
        }

        try
        {
            using var giteaAliveRes =
                await _httpClient.GetAsync(_giteaUri);
            if (!giteaAliveRes.IsSuccessStatusCode)
                return (false, "Сервер Gitea недоступен или не отвечает");

            if (string.IsNullOrWhiteSpace(_giteaAdminApiKey))
                return (false, "Отсутствует админский API ключ");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {_giteaAdminApiKey}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            using var gogsAdminRes =
                await _httpClient.GetAsync(new Uri(_giteaUri, "/api/v1/admin/cron"));
            if (gogsAdminRes.StatusCode == HttpStatusCode.Forbidden)
                return (false, "Некорректный ключ администратора");
        }
        catch (Exception e)
        {
            return (false, "Ошибка подключения к Gitea: " + e.Message);
        }

        return (true, "");
    }

    public async Task<IReadOnlyList<string>> GetUsers(string? prefix)
    {
        var url = new Uri(_giteaUri, $"/api/v1/admin/users?q={prefix}&limit=100&is_admin=false");
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var users =
            await response.Content.ReadFromJsonAsync<List<GiteaUser>>() ?? [];
        var result = users.Select(x => x.Username);

        return result.ToList();
    }

    public async Task<bool> CreateUser(User user)
    {
        var url = new Uri(_giteaUri, "/api/v1/admin/users");
        var request = new GiteaCreateUserRequest
        {
            Username = user.Username,
            Password = user.Password,
            Email = $"{user.Username}@{_gitEmail}"
        };

        using var response = await _httpClient.PostAsJsonAsync(url, request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUser(string username)
    {
        var url = new Uri(_giteaUri, $"/api/v1/admin/users/{Uri.EscapeDataString(username)}?purge=true");
        using var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}