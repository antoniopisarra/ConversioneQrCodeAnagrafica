using ConversioneQrCodeAnagrafica;

// No file argument: use an explicitly fictional example. A file argument is read locally.
const string example = """
{"anag":{"naz":"IT","cf":"00000000000","piva":"00000000000","denom":"AZIENDA ESEMPIO (DATI FITTIZI)","domFisc":{"prov":"RM","cap":"00100","com":"ROMA","ind":"VIA ESEMPIO 1","naz":"IT"}},"SDI":{"pec":"fatture@example.invalid","cod":"0000000"}}
""";
if (args.Length > 1)
{
    Console.Error.WriteLine("Usage: dotnet run --project examples/ConsoleDemo -- [decoded-qr.json]");
    return 2;
}
string json;
try { json = args.Length == 0 ? example : File.ReadAllText(args[0]); }
catch (Exception error) when (error is IOException or UnauthorizedAccessException)
{
    Console.Error.WriteLine("Unable to read the input file.");
    return 2;
}
if (!Anagrafica.TryParse(json, out var model))
{
    Console.Error.WriteLine("Invalid decoded QR JSON.");
    return 1;
}
// This example explicitly prints personal data: use fictional inputs for demos and CI logs.
Console.WriteLine($"Ragione sociale: {model!.RagioneSociale}");
Console.WriteLine($"Partita IVA: {model.PartitaIva}");
Console.WriteLine($"Codice fiscale: {model.CodiceFiscale}");
Console.WriteLine($"Nazione: {model.Nazione}");
Console.WriteLine($"Indirizzo: {model.Indirizzo}");
Console.WriteLine($"CAP / Comune / Provincia: {model.Cap} {model.Comune} ({model.Provincia})");
Console.WriteLine($"Codice destinatario: {model.CodiceUnivoco}");
Console.WriteLine($"PEC: {model.Pec}");
return 0;
