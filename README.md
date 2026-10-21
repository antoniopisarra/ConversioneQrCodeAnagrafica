# Conversione QR Code Anagrafica

[![NuGet](https://img.shields.io/nuget/v/ConversioneQrCodeAnagrafica.svg?logo=nuget)](https://www.nuget.org/packages/ConversioneQrCodeAnagrafica/)

Realizzato da [Logical Flow](https://www.logicalflow.it/) · Autore: Antonio Pisarra

[English](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/README.en.md) · [NuGet](https://www.nuget.org/packages/ConversioneQrCodeAnagrafica/) · [Codice sorgente](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica) · [MIT](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/LICENSE)

## Perché questo progetto

Una piccola libreria C# nata nel 2020 per trasformare il testo JSON contenuto nel QR anagrafico dell’Agenzia delle Entrate in un modello semplice da usare nei gestionali e nei flussi di fatturazione elettronica. La revisione 1.1.0 mantiene costruttore e proprietà originali, aggiorna la dipendenza JSON e aggiunge gestione degli input non validi, test automatici ed esempio multipiattaforma.

**Riceve il testo già decodificato dal QR, non un’immagine.** La lettura del QR tramite fotocamera o file va effettuata da un altro componente. La libreria non genera fatture XML, non invia dati allo SDI e non verifica l’esistenza o la correttezza fiscale di partita IVA, codice fiscale, PEC o codice destinatario. Non è un prodotto ufficiale dell’Agenzia delle Entrate.

## Installazione e compatibilità

Il pacchetto è pubblicato su [NuGet](https://www.nuget.org/packages/ConversioneQrCodeAnagrafica/):

```sh
dotnet add package ConversioneQrCodeAnagrafica
```

La libreria continua a compilare per **.NET Standard 2.0**: può essere referenziata da progetti compatibili, inclusi .NET moderni e .NET Framework 4.8. Non contiene dipendenze native specifiche per sistema operativo. L’esempio e i test usano **.NET 10**; non impongono questo requisito alle applicazioni che consumano la DLL. La verifica locale della revisione è eseguita su macOS ARM64; .NET Framework/Windows richiedono una verifica nel proprio ambiente.

Dipendenza runtime: **Newtonsoft.Json 13.0.4** (MIT). La versione dei sorgenti è **1.1.0**; la disponibilità del relativo aggiornamento pubblico è verificabile nella pagina NuGet. Non confondere un pacchetto locale con una versione già pubblicata.

## Uso rapido

```csharp
using ConversioneQrCodeAnagrafica;

string json = "{\"anag\":{\"naz\":\"IT\",\"piva\":\"00000000000\",\"denom\":\"AZIENDA ESEMPIO\"},\"SDI\":{\"cod\":\"0000000\"}}";
var anagrafica = new Anagrafica(json);
Console.WriteLine(anagrafica.RagioneSociale);
Console.WriteLine(anagrafica.PartitaIva);
```

Per input provenienti dall’esterno, `TryParse` evita di gestire eccezioni per i normali errori di formato:

```csharp
if (Anagrafica.TryParse(json, out var anagrafica))
{
    Console.WriteLine(anagrafica!.RagioneSociale);
}
else
{
    Console.WriteLine("Contenuto QR non riconosciuto.");
}
```

Gli esempi usano dati fittizi, non identificativi fiscalmente validati. Il [JSON completo di esempio](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/examples/anagrafica.json) include anche domicilio e PEC.

## Mappatura dei campi

| Proprietà C# | Campo JSON |
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

Le proprietà sono di sola lettura e contengono stringhe. `Nazione` proviene da `anag.naz`, come nella versione originale: non viene sostituita da `domFisc.naz`. `dtGenQr`, `domFisc.naz` e campi non mappati non sono esposti. Nessun trim, normalizzazione o controllo fiscale viene applicato: gli zeri iniziali di CAP e identificativi rimangono intatti.

## Input mancanti e non validi

- È obbligatorio un oggetto `anag`; `{ "anag": {} }` è strutturalmente accettato, ma non significa che l’anagrafica sia completa o valida per una fattura.
- `domFisc` e `SDI` possono mancare o essere `null`. Le stringhe mappate assenti o `null` diventano `string.Empty`.
- I valori presenti devono essere stringhe, non numeri, array o booleani. In particolare, CAP e partita IVA numerici sono rifiutati per evitare conversioni implicite e perdita degli zeri iniziali.
- I nomi mappati sono confrontati senza distinzione maiuscole/minuscole. Duplicati esatti o varianti ambigue dello stesso nome mappato sono rifiutati.
- Sono accettati campi aggiuntivi; la deserializzazione non attiva tipi indicati in `$type`. La sintassi di lettura è quella di Json.NET, non un validatore JSON Schema.
- Limiti: **65.536 caratteri UTF-16**, profondità massima **16**, un solo oggetto radice.

Il costruttore genera `ArgumentNullException` per `null`, `ArgumentException` per input vuoto o troppo lungo e `FormatException` per JSON/struttura non supportati. `TryParse` restituisce `false` e un modello `null` in questi casi. I messaggi di parsing non includono il contenuto ricevuto.

## Eseguire esempio, test e pacchetto

Dalla cartella del repository, con .NET SDK 10:

```sh
dotnet restore ConversioneQrCodeAnagrafica.sln
dotnet build ConversioneQrCodeAnagrafica.sln -c Release --no-restore
dotnet test ConversioneQrCodeAnagrafica.sln -c Release --no-build
dotnet run --project examples/ConsoleDemo -c Release
dotnet run --project examples/ConsoleDemo -c Release -- examples/anagrafica.json
dotnet pack ConversioneQrCodeAnagrafica -c Release -o artifacts
```

L’esempio stampa i dati in console ed esce senza attendere un tasto. I test verificano mappatura, Unicode, zeri iniziali, campi facoltativi, input errati, duplicati, limiti e assenza del payload nei messaggi d’errore. La CI esegue compilazione e test su Windows, macOS e Linux; l’esito effettivo si consulta nelle Actions, senza presumere che ogni piattaforma sia già verificata.

## Aggiornamento dalla 1.0.2

`new Anagrafica(json)` e i dieci nomi di proprietà restano disponibili. Per JSON completi e corretti la mappatura rimane invariata. Cambiano gli errori per input non validi; stringhe facoltative mancanti ora sono vuote e numeri nei campi testuali non vengono più convertiti implicitamente. Verificare queste differenze prima di aggiornare un’applicazione esistente. Nei progetti .NET Framework controllare anche gli eventuali binding redirect per la nuova versione di Newtonsoft.Json. Dettagli in [CHANGELOG](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/CHANGELOG.md).

## Privacy, licenza e contributi

La libreria elabora il testo in memoria: non usa rete, telemetria o file. L’esempio legge un file solo se richiesto e stampa i suoi dati; evitare documenti reali nei log, nei test pubblici e nelle issue.

Codice di Antonio Pisarra, realizzato da [Logical Flow](https://www.logicalflow.it/), distribuito con licenza **MIT**. Newtonsoft.Json mantiene la propria licenza MIT. Segnalazioni e contributi sono benvenuti nel repository: includere una riproduzione minima con dati fittizi e la versione .NET usata.

Per presentarlo sul sito sono disponibili una [scheda IT/EN](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/docs/WEBSITE.md) e le [note tecniche della revisione](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/blob/master/docs/REVIEW.md).
