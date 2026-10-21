# Changelog

## 1.1.0

- Retained the public constructor, ten read-only properties, assembly version and .NET Standard 2.0 target.
- Updated Newtonsoft.Json from 12.0.3 to 13.0.4.
- Added `TryParse`, nullable annotations, XML API documentation and bounded input/depth handling.
- Replaced null-reference failures with documented argument/format exceptions; absent optional values now become empty strings.
- Reject numeric/boolean/object/array values in mapped text fields, duplicate properties and multiple root values. Existing callers depending on Json.NET coercions must adapt their inputs.
- Preserved case-insensitive mapped names while rejecting ambiguous variants; extra fields are ignored and JSON type metadata is never activated.
- Replaced the Windows-only .NET Framework demo with a .NET 10 cross-platform console example using fictional data.
- Added automated regression tests, cross-platform CI, IT/EN READMEs, NuGet packaging documentation and website presentation copy.
- Added the MIT license with the author's authorization and credited Logical Flow.

## 1.0.2 and earlier

Original NuGet releases published in 2020: decoded QR JSON to the `Anagrafica` model, using Newtonsoft.Json 12.0.3. The former `Test` project was an interactive .NET Framework 4.8 console example, not an automated test suite.
