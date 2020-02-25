using Newtonsoft.Json;

namespace ConversioneQrCodeAnagrafica.JsonModel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class DomicilioFiscaleJson
    {
        [JsonProperty("prov")]
        public string Provincia { get; set; }

        [JsonProperty("cap")]
        public string Cap { get; set; }

        [JsonProperty("com")]
        public string Comune { get; set; }

        [JsonProperty("ind")]
        public string Indirizzo { get; set; }

        [JsonProperty("naz")]
        public string Nazione { get; set; }
    }
}