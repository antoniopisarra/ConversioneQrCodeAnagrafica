using Newtonsoft.Json;

namespace ConversioneQrCodeAnagrafica.JsonModel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class AnagraficaJson
    {
        [JsonProperty("naz")]
        public string Nazione { get; set; }

        [JsonProperty("cf")]
        public string CodiceFiscale { get; set; }

        [JsonProperty("piva")]
        public string PartitaIva { get; set; }

        [JsonProperty("denom")]
        public string RagioneSociale { get; set; }

        [JsonProperty("domFisc")]
        public DomicilioFiscaleJson DomicilioFiscale { get; set; }
    }
}