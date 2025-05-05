using System.Text.Json;
using UserAnalysisAPI.Models;

namespace UserAnalysisAPI.Services;

public class UserService
{
    private List<User> _users = new();

    public async Task LoadUsersAsync(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("Arquivo JSON Não encontrado.", filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        using var stream = File.OpenRead(filePath);
        _users = await JsonSerializer.DeserializeAsync<List<User>>(stream, options) ?? new List<User>();

        // var options = new JsonSerializerOptions
        // {
        //     PropertyNameCaseInsensitive = true
        // };

        // using var stream = File.OpenRead(filePath);
        // _users = await JsonSerializer.DeserializeAsync<List<User>>(stream) ?? new List<User>();
    }

    public List<User> GetUsers()
    {
        return _users;
    }
}
