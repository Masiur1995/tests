using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using TinyUrl.Core.Models;

namespace TinyUrl.Core.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly ConcurrentDictionary<string, ShortUrl> _storage = new();
    private const int DefaultShortCodeLength = 7;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int MaxCustomCodeLength = 32;

    public string CreateShortUrl(string longUrl, string? customShortCode = null)
    {
        if (string.IsNullOrWhiteSpace(longUrl))
            throw new ArgumentException("Long URL cannot be empty");

        var normalizedLongUrl = NormalizeAndValidateLongUrl(longUrl);

        string shortCode = string.IsNullOrWhiteSpace(customShortCode)
            ? GenerateUniqueShortCode()
            : NormalizeAndValidateCustomCode(customShortCode);

        var shortUrl = new ShortUrl
        {
            ShortCode = shortCode,
            LongUrl = normalizedLongUrl,
            AccessCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        if (!_storage.TryAdd(shortCode, shortUrl))
            throw new InvalidOperationException($"Short code '{shortCode}' already exists");

        return shortCode;
    }

    public bool DeleteShortUrl(string shortCode)
    {
        return !string.IsNullOrWhiteSpace(shortCode) && _storage.TryRemove(shortCode, out _);
    }

    public string? GetLongUrl(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode) || !_storage.TryGetValue(shortCode, out var shortUrl))
            return null;

        shortUrl.AccessCount++;
        return shortUrl.LongUrl;
    }

    public int GetAccessCount(string shortCode)
    {
        return string.IsNullOrWhiteSpace(shortCode) || !_storage.TryGetValue(shortCode, out var shortUrl) 
            ? 0 
            : shortUrl.AccessCount;
    }

    public IEnumerable<ShortUrl> GetAllUrls()
    {
        return _storage.Values;
    }

    private string GenerateUniqueShortCode()
    {
        string shortCode;
        int attempts = 0;

        do
        {
            if (attempts++ > 100)
                throw new InvalidOperationException("Failed to generate unique short code");

            var chars = new char[DefaultShortCodeLength];
            var bytes = RandomNumberGenerator.GetBytes(DefaultShortCodeLength);
            for (int i = 0; i < DefaultShortCodeLength; i++)
                chars[i] = Alphabet[bytes[i] % Alphabet.Length];

            shortCode = new string(chars);

        } while (_storage.ContainsKey(shortCode));

        return shortCode;
    }

    private static string NormalizeAndValidateLongUrl(string longUrl)
    {
        var trimmed = longUrl.Trim();
        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri))
            throw new ArgumentException("Long URL must be a valid absolute URL (http/https)");

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw new ArgumentException("Long URL must use http or https");

        return uri.ToString();
    }

    private static string NormalizeAndValidateCustomCode(string customShortCode)
    {
        var code = customShortCode.Trim();
        if (code.Length == 0)
            throw new ArgumentException("Custom code cannot be empty");

        if (code.Length > MaxCustomCodeLength)
            throw new ArgumentException($"Custom code too long (max {MaxCustomCodeLength} chars)");

        // Keep it simple for a 2-hour POC: only alphanumeric codes.
        foreach (var ch in code)
        {
            if (!char.IsLetterOrDigit(ch))
                throw new ArgumentException("Custom code must be alphanumeric");
        }

        return code;
    }
}
