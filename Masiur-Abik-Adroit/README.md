# TinyURL POC - Masiur Abik

Command-line URL shortening service in C#.

## Features

- Create short URLs (auto-generated or custom codes)
- Get long URL from short code (tracks click count)
- Delete short URLs
- View statistics
- List all URLs
- Thread-safe in-memory storage

## Structure

```
TinyUrl/
├── src/
│   ├── TinyUrl.Core/           # Core library
│   │   ├── Models/ShortUrl.cs
│   │   └── Services/UrlShortenerService.cs
│   └── TinyUrl.Console/        # CLI app
└── test/
    └── TinyUrl.Tests/          # Unit tests (10 tests)
```

## Prerequisites

- .NET 8 SDK

## Build & Run

**Build:**
```bash
dotnet build
```

**Run Console App:**
```bash
cd src/TinyUrl.Console
dotnet run
```

**Run Tests:**
```bash
dotnet test
```

## Console Menu

1. Create short URL - enter long URL and optional custom code
2. Get long URL - retrieve URL and increment click count
3. Delete short URL
4. Get statistics - view click count
5. List all URLs
6. Exit

## Implementation Details

- **Storage**: `ConcurrentDictionary` for thread-safe in-memory storage
- **Code Generation**: 7-character alphanumeric codes (a-z, A-Z, 0-9)
- **Uniqueness**: Enforced for all short codes
- **Statistics**: Click count incremented on each URL retrieval
- **Multiple Mappings**: Same long URL can have different short codes
- **Constraints (minimal)**:
  - Long URL must be an absolute `http`/`https` URL
  - Custom short code must be alphanumeric and max 32 chars

## Example Usage

```csharp
var service = new UrlShortenerService();

// Create with auto code
string code = service.CreateShortUrl("https://www.example.com");

// Create with custom code
string custom = service.CreateShortUrl("https://www.example.com", "mycustom");

// Get URL (increments count)
string? url = service.GetLongUrl(code);

// Get stats
int clicks = service.GetAccessCount(code);

// Delete
bool deleted = service.DeleteShortUrl(code);
```

## Test Coverage

10 unit tests covering:
- URL creation (auto & custom codes)
- Duplicate handling
- Input validation
- Retrieval and access counting
- Deletion
- Listing URLs
- Multiple codes per URL

## Submission

Package for submission (excludes bin/obj):
```bash
zip -r Masiur-Abik-Adroit.zip . -x "*/bin/*" "*/obj/*" "*/.vs/*"
```
