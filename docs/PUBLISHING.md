# Pubblicazione NuGet

Pacchetto esistente: https://www.nuget.org/packages/ConversioneQrCodeAnagrafica/  
Profilo proprietario visibile alla revisione: **53PHIR07**. Ultima versione pubblica verificata prima dell’aggiornamento: **1.0.2**, 19 aprile 2020. La nuova versione dei sorgenti è **1.1.0**.

## Metodo predisposto: Trusted Publishing

Il workflow `.github/workflows/publish-nuget.yml` si avvia manualmente dal branch `master`, compila, esegue i test, crea il pacchetto e usa l’identità GitHub per ottenere una credenziale NuGet temporanea. Non richiede di conservare API key nel repository o condividerle in chat.

Il proprietario deve accedere a nuget.org e configurare **Trusted Publishing → Add policy**:

| Campo | Valore |
| --- | --- |
| Repository owner | `antoniopisarra` |
| Repository | `ConversioneQrCodeAnagrafica` |
| Workflow file | `publish-nuget.yml` (solo il nome) |
| Environment | vuoto: il workflow non ne usa uno |
| Package scope/glob | `ConversioneQrCodeAnagrafica` |
| Azione consentita | pubblicare nuove versioni del pacchetto esistente |

La policy deve appartenere all’account che possiede il pacchetto. Il campo `nuget_user` all’avvio del workflow deve contenere il nome di quel profilo, non l’email. Il valore predefinito è `53PHIR07`; aggiornarlo se il proprietario effettivo è diverso.

Dopo aver salvato la policy, avviare **Actions → Publish NuGet → Run workflow**, scegliere `master` e verificare l’esito. Confermare poi la presenza della versione 1.1.0 su NuGet: l’indicizzazione può richiedere tempo. Una versione pubblicata non si sovrascrive; per ulteriori modifiche occorre incrementare la versione nel `.csproj`.

Riferimenti ufficiali: [Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing), [azione NuGet/login](https://github.com/NuGet/login).

## Pacchetto locale

```sh
dotnet test ConversioneQrCodeAnagrafica.sln -c Release
dotnet pack ConversioneQrCodeAnagrafica -c Release -o artifacts
```

`artifacts/ConversioneQrCodeAnagrafica.1.1.0.nupkg` contiene DLL, XML API, licenza e README. La generazione locale non pubblica nulla. Il comando `dotnet add package ConversioneQrCodeAnagrafica` scarica l’ultima versione pubblica disponibile, non il codice modificato localmente.

Prima di una nuova pubblicazione verificare che il changelog distingua input validi compatibili da cambi di comportamento per input errati e che il pacchetto sia creato dal commit desiderato. Non aggiungere credenziali o dati anagrafici reali al repository.

## Pubblicazione 1.1.0

Il workflow [39597258729](https://github.com/antoniopisarra/ConversioneQrCodeAnagrafica/actions/runs/39597258729) ha completato con successo compilazione, test, autenticazione OIDC e invio della versione 1.1.0 il 21 ottobre 2026. Il badge NuGet nei README segue automaticamente la versione pubblica indicizzata, con possibili ritardi dovuti all’indicizzazione e alla cache.
