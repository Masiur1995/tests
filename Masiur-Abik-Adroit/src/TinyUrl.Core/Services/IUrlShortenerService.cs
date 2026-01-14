using System.Collections.Generic;
using TinyUrl.Core.Models;

namespace TinyUrl.Core.Services;

public interface IUrlShortenerService
{
    string CreateShortUrl(string longUrl, string? customShortCode = null);
    bool DeleteShortUrl(string shortCode);
    string? GetLongUrl(string shortCode);
    int GetAccessCount(string shortCode);
    IEnumerable<ShortUrl> GetAllUrls();
}
