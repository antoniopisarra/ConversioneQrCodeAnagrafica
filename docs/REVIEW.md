# Revisione — 21 ottobre 2026

## Problemi trovati

- Newtonsoft.Json 12.0.3 era datato: aggiornato alla versione stabile 13.0.4.
- `null`, oggetti incompleti e sezioni `domFisc`/`SDI` mancanti producevano dereferenziazioni nulle: introdotto un contratto documentato e `TryParse`.
- La coercizione automatica dei campi testuali poteva accettare numeri, perdendo il significato degli zeri iniziali: ora sono richieste stringhe o null.
- Parsing senza limiti espliciti e duplicati ambigui: introdotti limiti di lunghezza/profondità e rifiuto dei duplicati.
- Il vecchio `Test` era un esempio interattivo .NET Framework 4.8, con dati di un’azienda reale, non una suite di unit test: sostituito con regressioni automatiche. Demo separata in `examples/ConsoleDemo`, eseguibile su .NET 10, con dati fittizi.
- Mancavano README, licenza e istruzioni NuGet: aggiunti README IT/EN, licenza MIT approvata dall’autore e documentazione di pubblicazione.

## Scelte

Si conserva .NET Standard 2.0 per non imporre la migrazione dei programmi che usano già la DLL. Si mantiene Newtonsoft.Json per evitare una nuova catena di dipendenze e un cambio non necessario del comportamento di lettura. Il costruttore e le dieci proprietà pubbliche non sono stati rinominati; la classe resta ereditabile e la versione assembly è conservata. Le differenze per input assenti o malformati sono documentate nel changelog.

La libreria è volutamente piccola. Non si aggiungono scansione immagini, chiamate fiscali, GUI o generazione fatture. I campi sconosciuti rimangono ignorati e il payload non attiva tipi .NET tramite metadati. I limiti non costituiscono un audit di sicurezza completo.

La mappatura deriva dal codice originale e dai payload di esempio del progetto: non si dichiara una certificazione dell’intero schema attuale dell’Agenzia delle Entrate. Prima dell’integrazione in un gestionale, verificare i propri payload reali senza pubblicare dati personali.

## Verifica

Compilazione Release completata senza avvisi o errori sul Mac ARM64 con .NET SDK 10.0.401. Passati 35 unit test; l’esempio completo con dati fittizi ha restituito i campi attesi. La verifica delle dipendenze transitive tramite le fonti NuGet configurate non segnala vulnerabilità note al controllo del 21 ottobre 2026: non equivale a una garanzia di assenza di vulnerabilità. Il pacchetto contiene la DLL netstandard2.0, la documentazione XML, README IT/EN, changelog e licenza. Un test separato installa il pacchetto locale in un progetto consumer per verificare anche l’artefatto distribuibile. La CI copre Windows/Linux/macOS quando viene eseguita su GitHub: non equivale a prove locali già effettuate su tutti i sistemi.

L’aggiornamento NuGet richiede l’accesso del proprietario del pacchetto: la generazione del `.nupkg` non lo pubblica automaticamente. Vedere PUBLISHING.md.
