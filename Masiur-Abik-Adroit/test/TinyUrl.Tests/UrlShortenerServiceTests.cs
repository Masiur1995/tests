using System;
using System.Linq;
using TinyUrl.Core.Services;
using Xunit;

namespace TinyUrl.Tests;

public class UrlShortenerServiceTests
{
    // Helper method for creating service instances
    private UrlShortenerService CreateService() => new UrlShortenerService();

    [Fact]
    public void CreateShortUrl_GeneratesCode()
    {
        var service = CreateService();
        var shortCode = service.CreateShortUrl("https://www.example.com");
        
        Assert.NotEmpty(shortCode);
        Assert.Equal(7, shortCode.Length);
    }

    [Fact]
    public void CreateShortUrl_WithCustomCode_UsesCustomCode()
    {
        var service = CreateService();
        var shortCode = service.CreateShortUrl("https://www.example.com", "custom");
        
        Assert.Equal("custom", shortCode);
    }

    [Fact]
    public void CreateShortUrl_DuplicateCode_ThrowsException()
    {
        var service = CreateService();
        service.CreateShortUrl("https://example1.com", "test");
        
        Assert.Throws<InvalidOperationException>(() => 
            service.CreateShortUrl("https://example2.com", "test"));
    }

    [Fact]
    public void CreateShortUrl_EmptyUrl_ThrowsException()
    {
        var service = CreateService();
        
        Assert.Throws<ArgumentException>(() => service.CreateShortUrl(""));
    }

    [Fact]
    public void CreateShortUrl_InvalidUrlFormat_ThrowsException()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() => service.CreateShortUrl("not-a-url"));
    }

    [Fact]
    public void CreateShortUrl_CustomCodeWithNonAlphanumeric_ThrowsException()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() => service.CreateShortUrl("https://example.com", "bad-code"));
    }

    [Fact]
    public void GetLongUrl_ValidCode_ReturnsUrl()
    {
        var service = CreateService();
        var shortCode = service.CreateShortUrl("https://www.example.com");
        
        var longUrl = service.GetLongUrl(shortCode);
        
        Assert.Equal("https://www.example.com/", longUrl);
    }

    [Fact]
    public void GetLongUrl_InvalidCode_ReturnsNull()
    {
        var service = CreateService();
        
        var longUrl = service.GetLongUrl("invalid");
        
        Assert.Null(longUrl);
    }

    [Fact]
    public void GetLongUrl_IncrementsAccessCount()
    {
        var service = CreateService();
        var shortCode = service.CreateShortUrl("https://www.example.com");
        
        service.GetLongUrl(shortCode);
        service.GetLongUrl(shortCode);
        
        Assert.Equal(2, service.GetAccessCount(shortCode));
    }

    [Fact]
    public void DeleteShortUrl_RemovesUrl()
    {
        var service = CreateService();
        var shortCode = service.CreateShortUrl("https://www.example.com");
        
        var deleted = service.DeleteShortUrl(shortCode);
        
        Assert.True(deleted);
        Assert.Null(service.GetLongUrl(shortCode));
    }

    [Fact]
    public void GetAllUrls_ReturnsAllCreated()
    {
        var service = CreateService();
        service.CreateShortUrl("https://example1.com");
        service.CreateShortUrl("https://example2.com");
        
        var urls = service.GetAllUrls().ToList();
        
        Assert.Equal(2, urls.Count);
    }

    [Fact]
    public void SameLongUrl_CanHaveMultipleShortCodes()
    {
        var service = CreateService();
        var code1 = service.CreateShortUrl("https://www.example.com");
        var code2 = service.CreateShortUrl("https://www.example.com");
        
        Assert.NotEqual(code1, code2);
    }
}
