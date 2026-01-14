using System;
using System.Linq;
using TinyUrl.Core.Services;

namespace TinyUrl.Console;

class Program
{
    private const string ShortLinkBase = "https://shorturl.com/";
    private static readonly IUrlShortenerService _service = new UrlShortenerService();

    static void Main(string[] args)
    {
        System.Console.WriteLine("=== TinyURL POC ===\n");

        bool running = true;
        while (running)
        {
            System.Console.WriteLine("1. Create short link (from a long URL)");
            System.Console.WriteLine("2. Get long URL");
            System.Console.WriteLine("3. Delete short URL");
            System.Console.WriteLine("4. Get statistics");
            System.Console.WriteLine("5. List all URLs");
            System.Console.WriteLine("6. Exit");
            System.Console.Write("> ");

            try
            {
                switch (System.Console.ReadLine())
                {
                    case "1": CreateShortUrl(); break;
                    case "2": GetLongUrl(); break;
                    case "3": DeleteShortUrl(); break;
                    case "4": GetStatistics(); break;
                    case "5": ListAllUrls(); break;
                    case "6": running = false; break;
                    default: System.Console.WriteLine("Invalid option"); break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
            System.Console.WriteLine();
        }
    }

    static void CreateShortUrl()
    {
        System.Console.Write("Enter the long URL: ");
        var longUrl = System.Console.ReadLine();
        System.Console.Write("Optional custom short code (alphanumeric): ");
        var customCode = System.Console.ReadLine();

        var shortCode = _service.CreateShortUrl(longUrl!, string.IsNullOrWhiteSpace(customCode) ? null : customCode);
        System.Console.WriteLine($"Short code: {shortCode}");
        System.Console.WriteLine($"Short link: {ShortLinkBase}{shortCode}");
    }

    static void GetLongUrl()
    {
        System.Console.Write("Short code or short link: ");
        var code = ExtractCode(System.Console.ReadLine());
        var longUrl = code == null ? null : _service.GetLongUrl(code);
        System.Console.WriteLine(longUrl != null ? $"URL: {longUrl}" : "Not found");
    }

    static void DeleteShortUrl()
    {
        System.Console.Write("Short code or short link: ");
        var code = ExtractCode(System.Console.ReadLine());
        var deleted = code != null && _service.DeleteShortUrl(code);
        System.Console.WriteLine(deleted ? "Deleted" : "Not found");
    }

    static void GetStatistics()
    {
        System.Console.Write("Short code or short link: ");
        var code = ExtractCode(System.Console.ReadLine());
        var count = code == null ? 0 : _service.GetAccessCount(code);
        System.Console.WriteLine($"Access count: {count}");
    }

    static void ListAllUrls()
    {
        var urls = _service.GetAllUrls().ToList();
        if (!urls.Any())
        {
            System.Console.WriteLine("No URLs");
            return;
        }

        foreach (var url in urls)
        {
            System.Console.WriteLine($"{ShortLinkBase}{url.ShortCode} -> {url.LongUrl} (clicks: {url.AccessCount})");
        }
    }

    private static string? ExtractCode(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        var trimmed = input.Trim();
        trimmed = trimmed.TrimEnd('/');
        var lastSlash = trimmed.LastIndexOf('/');
        return lastSlash >= 0 ? trimmed[(lastSlash + 1)..] : trimmed;
    }
}
