using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace covid_praktika
{
    //cia JSON country klase, nes JSON output faile reikia tam tikru atributu, ne visu
    class CountryJson
    {
        public string countryTerritoryCode { get; set; }
        public string countriesAndTerritories { get; set; }
        public int year{ get; set; }
        public int month { get; set; }
        public int cases { get; set; }
        public int deaths { get; set; }
    }
}
