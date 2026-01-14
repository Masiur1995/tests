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

Key Folders:

- `src/TinyUrl.Core/` — core logic (model + `UrlShortenerService`)
- `src/TinyUrl.Console/` — the CLI menu app
- `test/TinyUrl.Tests/` — xUnit tests (**12 tests**)

## Prerequisites

- .NET 8 SDK
- IDE: Visual Studio Code

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
- **Code Generation**: 7-character alphanumeric codes
- **Uniqueness**: Enforced for all short codes
- **Statistics**: Click count incremented on each URL retrieval
- **Multiple Mappings**: Same long URL can have different short codes
- **Constraints**:
  - Long URL must be an absolute `http`/`https` URL
  - Custom short code must be alphanumeric and max 32 chars

- **Short link format**: the CLI prints a friendly “short link” like `https://shorturl.com/<code>`.  
  It’s just a string (no web server). You can paste either the raw `<code>` or the full link into the CLI.


## Test Coverage

12 unit tests covering:
- URL creation (auto & custom codes)
- Duplicate handling
- Input validation
- Retrieval and access counting
- Deletion
- Listing URLs
- Multiple codes per URL