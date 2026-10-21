# Conversione QR Code Anagrafica

Made by [Logical Flow](https://www.logicalflow.it/en/) · Author: Antonio Pisarra

[Italiano](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/README.md) · [NuGet](https://www.nuget.org/packages/ConversioneQrCodeAnagrafica/) · [Source](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica) · [MIT license](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/LICENSE)

## Purpose

A small C# library originally written in 2020 to convert the **already-decoded JSON text** from an Italian Revenue Agency tax QR code into a read-only contact model for invoicing software. Version 1.1.0 retains the original constructor and property names, updates the JSON dependency and adds predictable parsing errors, automated tests and a cross-platform console example.

This library **does not scan QR images**, generate invoice XML, submit invoices to SDI or validate tax identifiers, VAT registration, certified email addresses or recipient codes. It is independent of the Italian Revenue Agency.

## Installation and compatibility

```sh
dotnet add package ConversioneQrCodeAnagrafica
```

The public package is available on [NuGet](https://www.nuget.org/packages/ConversioneQrCodeAnagrafica/). Check that page for published versions; a locally built 1.1.0 package is not automatically a public release.

The library targets **.NET Standard 2.0**, retaining compatibility with consuming projects such as modern .NET and .NET Framework 4.8. The demo and tests use **.NET 10**, which is a development requirement rather than a requirement for consuming the DLL. Local checks run on macOS ARM64; other environments require their own acceptance. Runtime dependency: Newtonsoft.Json 13.0.4 (MIT). No platform-native components are included.

## Usage

```csharp
using ConversioneQrCodeAnagrafica;

string json = "{\"anag\":{\"naz\":\"IT\",\"piva\":\"00000000000\",\"denom\":\"EXAMPLE COMPANY\"},\"SDI\":{\"cod\":\"0000000\"}}";
var model = new Anagrafica(json);
Console.WriteLine(model.RagioneSociale);

// Expected input errors can be handled without exceptions:
if (Anagrafica.TryParse(json, out var parsed))
    Console.WriteLine(parsed!.PartitaIva);
```

All example identifiers are fictional and are not validated tax data. A [complete example](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/examples/anagrafica.json) is provided.

## Field mapping

| Read-only property | JSON path |
| --- | --- |
| `RagioneSociale` | `anag.denom` |
| `PartitaIva` | `anag.piva` |
| `CodiceFiscale` | `anag.cf` |
| `Nazione` | `anag.naz` |
| `Provincia` | `anag.domFisc.prov` |
| `Cap` | `anag.domFisc.cap` |
| `Comune` | `anag.domFisc.com` |
| `Indirizzo` | `anag.domFisc.ind` |
| `CodiceUnivoco` | `SDI.cod` |
| `Pec` | `SDI.pec` |

All properties are strings. Country comes from `anag.naz`, as in the original release; `domFisc.naz` is not a fallback. `dtGenQr` and unmapped fields are not exposed. Values are neither trimmed nor normalized; leading zeroes are preserved.

## Parsing contract

- The root must contain an `anag` object. An empty `anag` is structurally accepted, without implying a complete or valid invoice contact.
- `domFisc` and `SDI` may be absent or null. Missing/null mapped text values become empty strings.
- Present mapped values must be strings or null; numbers, booleans, arrays and objects are rejected. This avoids implicit conversions of identifiers and postal codes.
- Mapped names remain case-insensitive. Exact duplicates and ambiguous case variants of mapped names are rejected.
- Additional fields are accepted. `$type` metadata never activates object types. Parsing follows Json.NET syntax, not JSON Schema validation.
- Limits: 65,536 UTF-16 characters, depth 16, one root object.

The constructor throws `ArgumentNullException` for null, `ArgumentException` for blank/oversized input and `FormatException` for unsupported JSON or structure. `TryParse` returns false with a null result for these errors. Parsing exception messages omit the original payload.

## Build, test and pack

From the repository root, using .NET SDK 10:

```sh
dotnet restore ConversioneQrCodeAnagrafica.sln
dotnet build ConversioneQrCodeAnagrafica.sln -c Release --no-restore
dotnet test ConversioneQrCodeAnagrafica.sln -c Release --no-build
dotnet run --project examples/ConsoleDemo -c Release
dotnet run --project examples/ConsoleDemo -c Release -- examples/anagrafica.json
dotnet pack ConversioneQrCodeAnagrafica -c Release -o artifacts
```

The former Windows-only interactive `Test` project is replaced by automated tests in `tests/ConversioneQrCodeAnagrafica.Tests`. An optional demo is in `examples/ConsoleDemo`; it exits without waiting for a key. Tests cover all mappings, Unicode, leading zeroes, absent values, invalid structures, duplicates, bounds and privacy of error messages. CI is configured for Linux, Windows and macOS; actual run results must be checked in Actions.

## Upgrading from 1.0.2

The constructor, ten property names and .NET Standard 2.0 target remain available. Well-formed complete string-based payloads map as before. Error types change, absent optional strings now become empty strings, and numeric values in text fields are rejected instead of implicitly converted. Review these behavior changes before updating existing applications. .NET Framework consumers should also check binding redirects for the updated Newtonsoft.Json assembly. See the [changelog](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/CHANGELOG.md).

## Privacy, license and contribution

The library processes text in memory, without network access, file I/O or telemetry. The demo optionally reads a local file and prints its data: use fictional data in logs and public issues.

Copyright Antonio Pisarra; made by [Logical Flow](https://www.logicalflow.it/en/). Licensed under **MIT**; Newtonsoft.Json retains its own MIT license. Contributions should include a minimal fictional reproduction and the .NET version. [Website copy](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/docs/WEBSITE.md) and [review notes](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/docs/REVIEW.md) are included.
