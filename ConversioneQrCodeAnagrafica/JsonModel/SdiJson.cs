using Newtonsoft.Json;

namespace ConversioneQrCodeAnagrafica.JsonModel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class SdiJson
    {
        [JsonProperty("cod")]
        public string CodiceUnivoco { get; set; }

        [JsonProperty("pec")]
        public string Pec { get; set; }
    }
}