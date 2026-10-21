using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConversioneQrCodeAnagrafica
{
    /// <summary>Converts the already-decoded JSON text of an Italian tax QR code into contact data.</summary>
    /// <remarks>This class does not scan images, validate tax identifiers or contact external services.</remarks>
    public class Anagrafica
    {
        /// <summary>Maximum accepted input length, in UTF-16 characters (64K characters).</summary>
        public const int MaxJsonLength = 65536;

        /// <summary>Creates a model from decoded QR JSON. Missing or null optional strings become empty strings.</summary>
        /// <param name="jsonString">Decoded JSON containing an object named <c>anag</c>.</param>
        /// <exception cref="ArgumentNullException">The input is null.</exception>
        /// <exception cref="ArgumentException">The input is empty, whitespace or exceeds <see cref="MaxJsonLength"/>.</exception>
        /// <exception cref="FormatException">The JSON or a recognized field has an unsupported structure.</exception>
        public Anagrafica(string jsonString)
        {
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));
            if (string.IsNullOrWhiteSpace(jsonString))
                throw new ArgumentException("QR JSON must not be empty.", nameof(jsonString));
            if (jsonString.Length > MaxJsonLength)
                throw new ArgumentException("QR JSON exceeds the supported length.", nameof(jsonString));

            JObject root;
            try
            {
                using (var text = new StringReader(jsonString))
                using (var reader = new JsonTextReader(text) { DateParseHandling = DateParseHandling.None, MaxDepth = 16 })
                {
                    root = JObject.Load(reader, new JsonLoadSettings
                    {
                        DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error,
                        LineInfoHandling = LineInfoHandling.Ignore
                    });
                    if (reader.Read()) throw new FormatException("QR JSON must contain a single object.");
                }
            }
            catch (JsonException)
            {
                // Do not put personal data or raw JSON in exception messages.
                throw new FormatException("Invalid QR JSON: expected a single object with supported structure.");
            }

            var anag = ObjectField(root, "anag", required: true)!;
            var domicilio = ObjectField(anag, "domFisc");
            var sdi = ObjectField(root, "SDI");
            RagioneSociale = StringField(anag, "denom");
            PartitaIva = StringField(anag, "piva");
            CodiceFiscale = StringField(anag, "cf");
            Nazione = StringField(anag, "naz");
            Provincia = StringField(domicilio, "prov");
            Cap = StringField(domicilio, "cap");
            Comune = StringField(domicilio, "com");
            Indirizzo = StringField(domicilio, "ind");
            CodiceUnivoco = StringField(sdi, "cod");
            Pec = StringField(sdi, "pec");
        }

        /// <summary>Parses decoded QR JSON, returning false and a null model for invalid input.</summary>
        /// <param name="jsonString">Decoded JSON text.</param>
        /// <param name="anagrafica">The parsed model, or null on failure.</param>
        public static bool TryParse(string? jsonString, out Anagrafica? anagrafica)
        {
            anagrafica = null;
            if (jsonString == null) return false;
            try
            {
                anagrafica = new Anagrafica(jsonString);
                return true;
            }
            catch (ArgumentException) { return false; }
            catch (FormatException) { return false; }
        }

        private static JObject? ObjectField(JObject parent, string name, bool required = false)
        {
            var token = Field(parent, name);
            if (token == null || token.Type == JTokenType.Null)
            {
                if (required) throw new FormatException("QR JSON must contain an anag object.");
                return null;
            }
            if (token is JObject value) return value;
            throw new FormatException("QR JSON contains a field with an invalid object type.");
        }

        private static string StringField(JObject? parent, string name)
        {
            if (parent == null) return string.Empty;
            var token = Field(parent, name);
            if (token == null || token.Type == JTokenType.Null) return string.Empty;
            if (token.Type == JTokenType.String) return token.Value<string>()!;
            throw new FormatException("QR JSON text fields must be strings or null.");
        }

        private static JToken? Field(JObject parent, string name)
        {
            // Match the original deserializer's case-insensitive field names, without ambiguous duplicates.
            JToken? result = null;
            foreach (var property in parent.Properties())
            {
                if (!string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)) continue;
                if (result != null) throw new FormatException("QR JSON contains ambiguous field names.");
                result = property.Value;
            }
            return result;
        }

        /// <summary>Company name (<c>anag.denom</c>).</summary>
        public string RagioneSociale { get; }
        /// <summary>VAT identifier, preserved as text (<c>anag.piva</c>).</summary>
        public string PartitaIva { get; }
        /// <summary>Tax identifier (<c>anag.cf</c>).</summary>
        public string CodiceFiscale { get; }
        /// <summary>Taxpayer country (<c>anag.naz</c>), not the separate domicile country.</summary>
        public string Nazione { get; }
        /// <summary>Fiscal domicile province (<c>anag.domFisc.prov</c>).</summary>
        public string Provincia { get; }
        /// <summary>Postal code, preserving leading zeroes (<c>anag.domFisc.cap</c>).</summary>
        public string Cap { get; }
        /// <summary>Fiscal domicile municipality (<c>anag.domFisc.com</c>).</summary>
        public string Comune { get; }
        /// <summary>Fiscal domicile street address (<c>anag.domFisc.ind</c>).</summary>
        public string Indirizzo { get; }
        /// <summary>SDI recipient code (<c>SDI.cod</c>).</summary>
        public string CodiceUnivoco { get; }
        /// <summary>Certified email address (<c>SDI.pec</c>).</summary>
        public string Pec { get; }
    }
}
