using System;
using System.Linq;
using TinyUrl.Core.Services;

namespace TinyUrl.Console;

class Program
{
    private static readonly IUrlShortenerService _service = new UrlShortenerService();

    static void Main(string[] args)
    {
        System.Console.WriteLine("=== TinyURL POC ===\n");

        bool running = true;
        while (running)
        {
            System.Console.WriteLine("1. Create short URL");
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
        System.Console.Write("Long URL: ");
        var longUrl = System.Console.ReadLine();
        System.Console.Write("Custom code (optional): ");
        var customCode = System.Console.ReadLine();

        var shortCode = _service.CreateShortUrl(longUrl!, string.IsNullOrWhiteSpace(customCode) ? null : customCode);
        System.Console.WriteLine($"Created: {shortCode}");
    }

    static void GetLongUrl()
    {
        System.Console.Write("Short code: ");
        var longUrl = _service.GetLongUrl(System.Console.ReadLine()!);
        System.Console.WriteLine(longUrl != null ? $"URL: {longUrl}" : "Not found");
    }

    static void DeleteShortUrl()
    {
        System.Console.Write("Short code: ");
        var deleted = _service.DeleteShortUrl(System.Console.ReadLine()!);
        System.Console.WriteLine(deleted ? "Deleted" : "Not found");
    }

    static void GetStatistics()
    {
        System.Console.Write("Short code: ");
        var count = _service.GetAccessCount(System.Console.ReadLine()!);
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
            System.Console.WriteLine($"{url.ShortCode} -> {url.LongUrl} (clicks: {url.AccessCount})");
        }
    }
}
