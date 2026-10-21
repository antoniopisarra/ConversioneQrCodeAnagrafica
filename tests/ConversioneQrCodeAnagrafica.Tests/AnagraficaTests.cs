using ConversioneQrCodeAnagrafica;
using Newtonsoft.Json;
using Xunit;

public class AnagraficaTests
{
    private const string Complete = """
    {"anag":{"naz":"IT","cf":"00000000000","piva":"00000000000","denom":"ESEMPIO À & C.","domFisc":{"prov":"RM","cap":"00100","com":"ROMA","ind":"VIA ESEMPIO 1","naz":"FR"}},"dtGenQr":"2026-10-21T12:00:00+0200","SDI":{"pec":"fatture@example.invalid","cod":"0000000"}}
    """;

    [Fact]
    public void MapsAllOriginalPropertiesWithoutLosingLeadingZeroes()
    {
        var value = new Anagrafica(Complete);
        Assert.Equal("ESEMPIO À & C.", value.RagioneSociale);
        Assert.Equal("00000000000", value.PartitaIva);
        Assert.Equal("00000000000", value.CodiceFiscale);
        Assert.Equal("IT", value.Nazione);
        Assert.Equal("RM", value.Provincia);
        Assert.Equal("00100", value.Cap);
        Assert.Equal("ROMA", value.Comune);
        Assert.Equal("VIA ESEMPIO 1", value.Indirizzo);
        Assert.Equal("0000000", value.CodiceUnivoco);
        Assert.Equal("fatture@example.invalid", value.Pec);
    }

    [Theory]
    [InlineData("{\"anag\":{}}")]
    [InlineData("{\"anag\":{\"domFisc\":null},\"SDI\":null}")]
    public void OptionalAbsentOrNullFieldsBecomeEmptyStrings(string json)
    {
        var value = new Anagrafica(json);
        Assert.Equal(string.Empty, value.RagioneSociale);
        Assert.Equal(string.Empty, value.Cap);
        Assert.Equal(string.Empty, value.CodiceUnivoco);
        Assert.Equal(string.Empty, value.Pec);
    }

    [Fact]
    public void AcceptsUnknownFieldsAndIgnoresTypeMetadata()
    {
        Assert.True(Anagrafica.TryParse("""
        {"$type":"Missing.Type, Missing.Assembly","extra":{"future":true},"anag":{"denom":"ESEMPIO"}}
        """, out var value));
        Assert.Equal("ESEMPIO", value!.RagioneSociale);
    }

    [Fact]
    public void SupportsCaseInsensitiveNamesAsBefore()
    {
        var value = new Anagrafica("{\"ANAG\":{\"DENOM\":\"ESEMPIO\"},\"sdi\":{\"COD\":\"0000000\"}}");
        Assert.Equal("ESEMPIO", value.RagioneSociale);
        Assert.Equal("0000000", value.CodiceUnivoco);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t\n")]
    [InlineData("null")]
    [InlineData("[]")]
    [InlineData("{}")]
    [InlineData("{\"anag\":null}")]
    [InlineData("{\"anag\":[]}")]
    [InlineData("{\"anag\":true}")]
    [InlineData("{\"anag\":{\"domFisc\":42}}")]
    [InlineData("{\"anag\":{},\"SDI\":[]}")]
    [InlineData("{\"anag\":{\"piva\":123}}")]
    [InlineData("{\"anag\":{\"domFisc\":{\"cap\":100}}}")]
    [InlineData("{\"anag\":{\"denom\":[]}}")]
    [InlineData("{\"anag\":{\"denom\":true}}")]
    [InlineData("{\"anag\":{\"denom\":{}}}")]
    [InlineData("{\"anag\":{},\"SDI\":{\"cod\":123}}")]
    [InlineData("{\"anag\":{},\"anag\":{}}")]
    [InlineData("{\"anag\":{},\"ANAG\":{}}")]
    [InlineData("{\"anag\":{\"piva\":null,\"PIVA\":\"00000000000\"}}")]
    [InlineData("{\"anag\":{}} {}")]
    [InlineData("{\"anag\":{}} true")]
    [InlineData("{\"anag\":{}} garbage")]
    [InlineData("{\"anag\":")]
    public void TryParseRejectsInvalidInputsAndNeverReturnsPartialModel(string? json)
    {
        Assert.False(Anagrafica.TryParse(json, out var value));
        Assert.Null(value);
    }

    [Fact]
    public void ReportsUsefulExceptionCategories()
    {
        Assert.Throws<ArgumentNullException>(() => new Anagrafica(null!));
        Assert.Throws<ArgumentException>(() => new Anagrafica(" "));
        Assert.Throws<FormatException>(() => new Anagrafica("{}"));
        Assert.Throws<FormatException>(() => new Anagrafica("null"));
    }

    [Fact]
    public void RejectsOversizedAndOverlyNestedInputs()
    {
        Assert.False(Anagrafica.TryParse(new string(' ', Anagrafica.MaxJsonLength) + "{}", out _));
        var deep = "{\"anag\":{},\"extra\":" + new string('[', 20) + "0" + new string(']', 20) + "}";
        Assert.False(Anagrafica.TryParse(deep, out _));
    }

    [Fact]
    public void DoesNotExposePayloadInParsingExceptions()
    {
        var error = Assert.Throws<FormatException>(() => new Anagrafica("{\"PRIVATE_PAYLOAD\":"));
        Assert.DoesNotContain("PRIVATE_PAYLOAD", error.ToString());
    }

    [Fact]
    public void NullStringsBecomeEmptyWhileWhitespaceAndDatesArePreserved()
    {
        var value = new Anagrafica("{\"anag\":{\"piva\":null,\"denom\":\"  Example  \",\"cf\":\"2026-10-21T12:00:00Z\"}}");
        Assert.Equal("", value.PartitaIva);
        Assert.Equal("  Example  ", value.RagioneSociale);
        Assert.Equal("2026-10-21T12:00:00Z", value.CodiceFiscale);
    }

    [Fact]
    public void SuccessTryParseProducesModel()
    {
        Assert.True(Anagrafica.TryParse(Complete, out var value));
        Assert.Equal("00100", value!.Cap);
    }

    [Fact]
    public void ModelCanStillBeSubclassed()
    {
        Assert.Equal("", new ExistingDerivedModel().PartitaIva);
    }

    private class ExistingDerivedModel : Anagrafica
    {
        public ExistingDerivedModel() : base("{\"anag\":{}}") { }
    }
}
