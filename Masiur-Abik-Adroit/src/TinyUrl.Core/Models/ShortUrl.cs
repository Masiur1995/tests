using System;

namespace TinyUrl.Core.Models;

public class ShortUrl
{
    public string ShortCode { get; set; } = string.Empty;
    public string LongUrl { get; set; } = string.Empty;
    public int AccessCount;
    public DateTime CreatedAt { get; set; }
}
