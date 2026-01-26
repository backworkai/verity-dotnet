# Verity .NET SDK

.NET client library for the [Verity API](https://verity.backworkai.com) - Medicare coverage policies, prior authorization requirements, and medical code lookups.

## Installation

```bash
dotnet add package Verity.SDK
```

Or via NuGet Package Manager:
```
Install-Package Verity.SDK
```

## Quick Start

```csharp
using Verity.SDK;
using Verity.SDK.Models;

// Initialize the client
var client = new VerityClient("vrt_live_YOUR_API_KEY");

// Look up a medical code
var result = await client.LookupCodeAsync(
    "76942",
    include: new[] { "rvu", "policies" }
);
Console.WriteLine(result.Data?.Description);
// Output: "Ultrasonic guidance for needle placement"

// Check prior authorization requirements
var paCheck = await client.CheckPriorAuthAsync(
    procedureCodes: new[] { "76942" },
    diagnosisCodes: new[] { "M54.5" },
    state: "TX"
);
Console.WriteLine($"PA Required: {paCheck.Data?.PaRequired}");

// Search policies
var policies = await client.ListPoliciesAsync(
    query: "ultrasound guidance",
    policyType: "LCD",
    limit: 10
);

// Get specific policy details
var policy = await client.GetPolicyAsync(
    "L33831",
    include: new[] { "criteria", "codes" }
);
```

## Features

- **.NET Standard 2.0+** - Compatible with .NET Framework 4.6.1+, .NET Core 2.0+, and .NET 5+
- **Async/await patterns** - Modern asynchronous API
- **Full IntelliSense support** - Complete XML documentation
- **Type-safe** - Strongly-typed models
- **NuGet package** - Easy installation and updates

## Authentication

Get your API key from the [Verity Dashboard](https://verity.backworkai.com/dashboard).

```csharp
var client = new VerityClient("vrt_live_YOUR_API_KEY");

// Or with custom base URL
var client = new VerityClient(
    "vrt_live_YOUR_API_KEY",
    "https://verity.backworkai.com/api/v1"
);
```

## Usage Examples

### Code Lookup

```csharp
// Basic lookup
var result = await client.LookupCodeAsync("76942");

// With additional data
var result = await client.LookupCodeAsync(
    code: "76942",
    codeSystem: "HCPCS",
    jurisdiction: "JM",
    include: new[] { "rvu", "policies" },
    fuzzy: true
);

if (result.Data?.Found == true)
{
    Console.WriteLine($"Code: {result.Data.Code}");
    Console.WriteLine($"Description: {result.Data.Description}");
    
    if (result.Data.Rvu != null)
    {
        Console.WriteLine($"Price: ${result.Data.Rvu.NonFacilityPrice}");
    }
}
```

### Policy Search

```csharp
// Keyword search
var policies = await client.ListPoliciesAsync(
    query: "ultrasound guidance",
    mode: "keyword",
    policyType: "LCD",
    status: "active",
    limit: 50
);

if (policies.Data != null)
{
    foreach (var policy in policies.Data)
    {
        Console.WriteLine($"{policy.PolicyId}: {policy.Title}");
    }
}

// Semantic search
var policies = await client.ListPoliciesAsync(
    query: "imaging guidance for procedures",
    mode: "semantic"
);
```

### Prior Authorization

```csharp
var result = await client.CheckPriorAuthAsync(
    procedureCodes: new[] { "76942", "76937" },
    diagnosisCodes: new[] { "M54.5", "G89.29" },
    state: "TX",
    payer: "medicare"
);

if (result.Data != null)
{
    Console.WriteLine($"PA Required: {result.Data.PaRequired}");
    Console.WriteLine($"Confidence: {result.Data.Confidence}");
    Console.WriteLine($"Reason: {result.Data.Reason}");
    
    if (result.Data.DocumentationChecklist != null)
    {
        Console.WriteLine("\nDocumentation Checklist:");
        foreach (var item in result.Data.DocumentationChecklist)
        {
            Console.WriteLine($"  - {item}");
        }
    }
}
```

### Get Policy Details

```csharp
var policy = await client.GetPolicyAsync(
    "L33831",
    include: new[] { "criteria", "codes", "attachments" }
);

if (policy.Data != null)
{
    Console.WriteLine($"Policy: {policy.Data.Title}");
    Console.WriteLine($"Type: {policy.Data.PolicyType}");
    Console.WriteLine($"Status: {policy.Data.Status}");
    Console.WriteLine($"\n{policy.Data.Summary}");
}
```

## Error Handling

```csharp
using Verity.SDK;

try
{
    var result = await client.LookupCodeAsync("76942");
}
catch (AuthenticationException ex)
{
    Console.WriteLine($"Invalid API key: {ex.Message}");
}
catch (NotFoundException ex)
{
    Console.WriteLine($"Resource not found: {ex.Message}");
}
catch (RateLimitException ex)
{
    Console.WriteLine($"Rate limit exceeded: {ex.Message}");
}
catch (VerityException ex)
{
    Console.WriteLine($"API error ({ex.Code}): {ex.Message}");
}
```

## Using with Dependency Injection

```csharp
// Startup.cs or Program.cs
services.AddSingleton<VerityClient>(sp => 
    new VerityClient(configuration["Verity:ApiKey"]));

// In your service/controller
public class MyService
{
    private readonly VerityClient _verity;
    
    public MyService(VerityClient verity)
    {
        _verity = verity;
    }
    
    public async Task<bool> CheckPriorAuth(string procedureCode)
    {
        var result = await _verity.CheckPriorAuthAsync(
            new[] { procedureCode },
            state: "TX"
        );
        return result.Data?.PaRequired ?? false;
    }
}
```

## Disposal

The client implements `IDisposable` and should be properly disposed:

```csharp
using (var client = new VerityClient("vrt_live_YOUR_API_KEY"))
{
    var result = await client.LookupCodeAsync("76942");
    // Client automatically disposed here
}

// Or with using declaration (C# 8+)
using var client = new VerityClient("vrt_live_YOUR_API_KEY");
var result = await client.LookupCodeAsync("76942");
```

## Requirements

- .NET Standard 2.0+ (.NET Framework 4.6.1+, .NET Core 2.0+, .NET 5+)
- Newtonsoft.Json 13.0.3+

## License

MIT License - see LICENSE file for details.

## Support

- Documentation: https://verity.backworkai.com/docs
- Issues: https://github.com/tylerbryy/verity-dotnet/issues
- Email: support@verity.backworkai.com
