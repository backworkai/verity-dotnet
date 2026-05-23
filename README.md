# Verity .NET SDK

Official .NET client for the [Verity API](https://verity.backworkai.com): Medicare coverage policies, medical code intelligence, prior authorization checks, claim validation, compliance review, and drug formulary evidence.

The SDK targets .NET Standard 2.0 and is compatible with .NET Framework 4.6.1+, .NET Core 2.0+, and modern .NET releases.

## Installation

```bash
dotnet add package Verity.SDK
```

NuGet Package Manager:

```powershell
Install-Package Verity.SDK
```

## Quick Start

```csharp
using Verity.SDK;

using var client = new VerityClient("vrt_live_YOUR_API_KEY");

var code = await client.LookupCodeAsync(
    code: "76942",
    include: new[] { "rvu", "policies" }
);

Console.WriteLine(code.Data?.Description);

var priorAuth = await client.CheckPriorAuthAsync(
    procedureCodes: new[] { "76942" },
    diagnosisCodes: new[] { "M54.5" },
    state: "TX",
    payer: "medicare"
);

Console.WriteLine(priorAuth.Data?.PaRequired);
```

Get an API key from the [Verity dashboard](https://verity.backworkai.com/dashboard).

## Core Workflows

### Code Lookup

```csharp
var result = await client.LookupCodeAsync(
    code: "76942",
    codeSystem: "CPT",
    jurisdiction: "JM",
    include: new[] { "rvu", "policies", "rates" },
    fuzzy: true
);
```

### Policy Search and Retrieval

```csharp
var policies = await client.ListPoliciesAsync(
    query: "ultrasound guidance",
    mode: "keyword",
    policyType: "LCD",
    status: "active",
    limit: 25
);

var policy = await client.GetPolicyAsync(
    "L33831",
    include: new[] { "criteria", "codes" }
);
```

### Prior Authorization and Claim Validation

```csharp
var priorAuth = await client.CheckPriorAuthAsync(
    procedureCodes: new[] { "76942" },
    diagnosisCodes: new[] { "M54.5" },
    state: "TX",
    payer: "medicare"
);

var claim = await client.ValidateClaimWithDateOfServiceAsync(
    procedureCodes: new[] { "99213" },
    diagnosisCodes: new[] { "E11.9" },
    payer: "Medicare",
    state: "TX",
    dateOfService: "2026-05-23"
);

Console.WriteLine($"{claim.Data?.CoverageStatus} {claim.Data?.DenialRisk}");
Console.WriteLine(string.Join(", ", claim.Data?.Issues ?? new()));
```

### Coverage, Spending, and Compliance

```csharp
var criteria = await client.SearchCriteriaAsync(
    query: "diabetes",
    section: "indications",
    limit: 10
);
Console.WriteLine($"{criteria.Data?.FirstOrDefault()?.PolicyId}: {criteria.Data?.FirstOrDefault()?.PolicyTitle}");

var spending = await client.GetSpendingByCodeAsync(
    codes: new[] { "T1019", "T1020" },
    year: 2023
);

var changes = await client.ListUnreviewedChangesAsync(limit: 10);
var stats = await client.GetComplianceStatsAsync();
```

### Drug Formulary Evidence

```csharp
var formulary = await client.SearchDrugFormularyEvidenceAsync(
    query: "ozempic",
    payer: "all",
    limit: 5
);
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
    Console.WriteLine($"Verity API error ({ex.Code}): {ex.Message}");
}
```

## Dependency Injection

```csharp
services.AddSingleton<VerityClient>(_ =>
    new VerityClient(configuration["Verity:ApiKey"]));
```

## Configuration and Disposal

```csharp
using var client = new VerityClient(
    "vrt_live_YOUR_API_KEY",
    "https://verity.backworkai.com/api/v1"
);
```

`VerityClient` implements `IDisposable`. Use a `using` statement or register it with your dependency injection container.

## Development

```bash
dotnet restore
dotnet build src/Verity.SDK/Verity.SDK.csproj
```

## Support

- Documentation: https://verity.backworkai.com/docs
- Issues: https://github.com/backworkai/verity-dotnet/issues
- Email: support@verity.backworkai.com

## License

MIT
