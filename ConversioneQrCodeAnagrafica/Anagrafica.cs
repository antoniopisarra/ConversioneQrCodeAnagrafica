using ConversioneQrCodeAnagrafica.JsonModel;
using Newtonsoft.Json;

namespace ConversioneQrCodeAnagrafica
{
    public class Anagrafica
    {
        public Anagrafica(string jsonString)
        {
            var azienda = JsonConvert.DeserializeObject<AziendaJson>(jsonString);
            RagioneSociale = azienda.Anagrafica.RagioneSociale;
            PartitaIva = azienda.Anagrafica.PartitaIva;
            CodiceFiscale = azienda.Anagrafica.CodiceFiscale;
            Nazione = azienda.Anagrafica.Nazione;
            Provincia = azienda.Anagrafica.DomicilioFiscale.Provincia;
            Cap = azienda.Anagrafica.DomicilioFiscale.Cap;
            Comune = azienda.Anagrafica.DomicilioFiscale.Comune;
            Indirizzo = azienda.Anagrafica.DomicilioFiscale.Indirizzo;
            CodiceUnivoco = azienda.Sdi.CodiceUnivoco;
            Pec = azienda.Sdi.Pec;
        }

        public string RagioneSociale { get; }
        public string PartitaIva { get; }
        public string CodiceFiscale { get; }
        public string Nazione { get; }
        public string Provincia { get; }
        public string Cap { get; }
        public string Comune { get; }
        public string Indirizzo { get; }
        public string CodiceUnivoco { get; }
        public string Pec { get; }
    }
}