using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace covid_praktika
{
    //cia pacio Country klase su visais duotais atributais
    class Country
    {
        //atskiru day, month, year neimiau, nes visi trys sitame viename yra
        public string dateRep { get; set; }
        public int cases { get; set; }
        public int deaths { get; set; }
        public string countriesAndTerritories { get; set; }
        public string geoId { get; set; }
        public string countryTerritoryCode { get; set; }
        public int popData2019 { get; set; }
        public string continentExp { get; set; }

        //nereikalingas
        public double cumulativeNumber { get; set; }
    }
}
