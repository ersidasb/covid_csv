using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;

namespace covid_praktika
{
    public class CSV
    {

        //default csv failo path
        private static readonly string defaultFilePath = @"./input/data_source.csv";

        //download link
        private static readonly string csvURL = @"https://opendata.ecdc.europa.eu/covid19/casedistribution/csv";

        //input failo path, priskiriamas atsiustas failas arba default (metodas getCsvFile())
        private string targetFilePath;

        //atrinkti atributai
        private List<Country> records = new List<Country>();

        //input failo pirmos eilutes atributai
        private string[] variables = new string[12];

        //konstruktorius, pasileidzia kai tik sukuri objekta CSV
        public CSV(string countryTerritoryCode)
        {
            //gauna input csv faila
            getCsvFile();

            //gauna List<Country> lista is input failo kur sutanpa countryTerritoryCode
            csvToObjectList(countryTerritoryCode);
        }

        private void getCsvFile()
        {
            //pabando parsiust faila is intiko, jei nepaeina tada default path naudoja
            try
            {
                WebClient client = new WebClient();

                //parsiusto failo output path su data
                string csvOuptutPath = @"./input/source_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".csv";

                //parsiuncia file
                client.DownloadFile(csvURL, csvOuptutPath);

                //priskiria targetFilePath naujai parsiusto failo path
                targetFilePath = csvOuptutPath;
            }
            catch
            {
                //priskiria targetFilePath default failo path, nes parsisiuntime klaida buvo
                targetFilePath = defaultFilePath;
            }
        }

        private void csvToObjectList(string countryTerritoryCode)
        {
            //streamreaderis tam failui is kurio noresi skaityt
            StreamReader sr = new StreamReader(targetFilePath);

            //line yra skaitoma eilute i string
            string line;

            //row yra skaitoma eilute padalina i string array ten kur kableliai
            string[] row = new string[12];

            //skaito pirma eilute kur yra atributu pavadinimai ir issaugo atminti velesniam panaudojimui
            line = sr.ReadLine();
            variables = line.Split(',');

            //readina faila kol nebelieka ka readint. vienu ciklu apdirba viena eilute
            while((line = sr.ReadLine()) != null)
            {
                row = line.Split(',');

                //tikrina ar countryTerritoryCode lygus, .ToLower() tiesiog padaro kad lower case butu, tai nesvarbu ar caps lock ivesi ar ne vis tiek veiks
                if(row[8].ToLower() == countryTerritoryCode.ToLower())
                {
                    //sukuria nauja country objekta su 
                    Country country = new Country
                    {
                        dateRep = row[0],
                        cases = int.Parse(row[4]),
                        deaths = int.Parse(row[5]),
                        countriesAndTerritories = row[6],
                        geoId = row[7],
                        countryTerritoryCode = row[8],
                        popData2019 = int.Parse(row[9]),
                        continentExp = row[10]
                    };
                    try
                    {
                        //kai kur ta reiksme buna tuscia todel try catch bloke dedu, jei paeina parse tada nuskaito, jei ne tada 0 priskiria
                        country.cumulativeNumber = double.Parse(row[11]);
                    }
                    catch
                    {
                        country.cumulativeNumber = 0;
                    }

                    //prideda record i lista
                    records.Add(country);
                }
            }

            //parodo kiek records rado
            MessageBox.Show("Found records: " + records.Count.ToString());
        }

        public void csvToJson()
        {
            //output failo path su data
            string outputFilePath = @"./output/generated_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".json";

            //countriesJson listas, naudoju kita klase CountryJson nes uzduoty nurodyti specific atributai output failui
            List<CountryJson> countriesJson = new List<CountryJson>();

            //eina per kiekviena record
            foreach(Country c in records)
            {
                //naujas countryJson objektas is Country objekto
                CountryJson countryJson = new CountryJson
                {
                    countryTerritoryCode = c.countryTerritoryCode,
                    countriesAndTerritories = c.countriesAndTerritories,
                    year = int.Parse(c.dateRep.Split('/')[2]),
                    month = int.Parse(c.dateRep.Split('/')[1]),
                    cases = c.cases,
                    deaths = c.deaths
                };
                //prideda nauja countryJson i lista
                countriesJson.Add(countryJson);
            }

            //pavercia countriesJson lista i json stringa
            var json = JsonSerializer.Serialize(countriesJson);

            //i nauja faila iraso visa json stringa
            File.WriteAllText(outputFilePath, json);
        }

        public void csvToCsv()
        {
            //output failo path su data
            string outputFilePath = @"./output/generated_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".csv";

            //sukuri faila
            StreamWriter outputFile = new StreamWriter(outputFilePath, true);

            //ce bbz be sita normaliai neveik failo kurims
            outputFile.AutoFlush = true;

            //i output faila iraso pirma eilute su variable names
            outputFile.WriteLine($"{variables[8]},{variables[6]},{variables[0]},{variables[4]},{variables[5]}");

            //loop per kiekviena record listo elementa
            foreach(Country c in records)
            {
                //i output faila iraso viena eilute su duomenim
                outputFile.WriteLine($"{c.countryTerritoryCode},{c.countriesAndTerritories},{dateRepToLT(c.dateRep)},{c.cases},{c.deaths}");
            }
        }

        //konvertinu ta cancer dd/MM/yyyy data i yyyy-MM-dd (abu string, ne DateTime)
        private string dateRepToLT(string dateRep)
        {
            string[] dateValues = dateRep.Split('/');

            return $"{dateValues[2]}-{dateValues[1]}-{dateValues[0]}";
        }
    }
}
