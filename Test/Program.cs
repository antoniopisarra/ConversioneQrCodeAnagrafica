using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonString = @"{""anag"":{""naz"":""IT"",""cf"":""07892720967"",""piva"":""07892720967"",""denom"":""BRANDON GROUP SRL"",""domFisc"":{""prov"":""MI"",""cap"":""20129"",""com"":""MILANO"",""ind"":""VIA MASERA N 10"",""naz"":""IT""}},""dtGenQr"":""2018-12-18T11:27:38.734+0100"",""SDI"":{""pec"":"""",""cod"":""M5ITOJA""}}";

            var anagrafica = new ConversioneQrCodeAnagrafica.Anagrafica(jsonString);

            Console.WriteLine(anagrafica.RagioneSociale);
            Console.WriteLine(anagrafica.PartitaIva);
            Console.WriteLine(anagrafica.CodiceFiscale);
            Console.WriteLine(anagrafica.Nazione);
            Console.WriteLine(anagrafica.Provincia);
            Console.WriteLine(anagrafica.Cap);
            Console.WriteLine(anagrafica.Comune);
            Console.WriteLine(anagrafica.Indirizzo);
            Console.WriteLine(anagrafica.CodiceUnivoco);
            Console.WriteLine(anagrafica.Pec);

            Console.ReadKey();
        }
    }
}
