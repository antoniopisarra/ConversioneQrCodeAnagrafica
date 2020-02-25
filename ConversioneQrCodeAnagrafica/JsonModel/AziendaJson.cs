using Newtonsoft.Json;

namespace ConversioneQrCodeAnagrafica.JsonModel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class AziendaJson
    {
        [JsonProperty("anag")]
        public AnagraficaJson Anagrafica { get; set; }

        [JsonProperty("dtGenQr")]
        public string DataGenerazioneQr { get; set; }

        [JsonProperty("SDI")]
        public SdiJson Sdi { get; set; }
    }
}