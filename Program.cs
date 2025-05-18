using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Введіть назву репозиторію у форматі 'owner/repo' (наприклад, torvalds/linux):");
        string repo = Console.ReadLine() ?? "";

        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("request"); // GitHub вимагає User-Agent

        try
        {
            string url = $"https://api.github.com/repos/{repo}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);

            string name = doc.RootElement.GetProperty("full_name").GetString()!;
            string description = doc.RootElement.GetProperty("description").GetString()!;
            int stars = doc.RootElement.GetProperty("stargazers_count").GetInt32();
            string htmlUrl = doc.RootElement.GetProperty("html_url").GetString()!;

            Console.WriteLine($"
Назва: {name}");
            Console.WriteLine($"Опис: {description}");
            Console.WriteLine($"★ Зірки: {stars}");
            Console.WriteLine($"URL: {htmlUrl}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Помилка HTTP: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Інша помилка: {ex.Message}");
        }
    }
}
