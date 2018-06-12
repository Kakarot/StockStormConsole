using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HistoricalDataInfo
{
    class HistoricalDataInfo
    {
        public static void Main(string[] args)
        {
         
            Console.WriteLine("Please enter stock symbol that is exactly 3 characters long.");
            var symbol = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(symbol) || symbol.Length != 3)
            {
                Console.WriteLine("Invalid stock symbol. Symbol must be 3 characters long.");
                Environment.Exit(-1);
            }
          
            var IEXTrading_API_PATH = "https://api.iextrading.com/1.0/stock/{0}/chart/1y";

            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //For IP-API
                client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage response = client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<HistoricalDataResponse> historicalDataList = JsonConvert.DeserializeObject<List<HistoricalDataResponse>>(result);
                    
                    if(historicalDataList == null || historicalDataList.Count == 0)
                    {
                        Console.WriteLine(string.Format("Stock symbol exists, but no results exist for symbol {0}", symbol));
                        Environment.Exit(-1);
                    }
                    
                    foreach (var historicalData in historicalDataList)
                    {
                        if (historicalData != null)
                        {
                            Console.WriteLine("Open: " + historicalData.open);
                            Console.WriteLine("Close: " + historicalData.close);
                            Console.WriteLine("Low: " + historicalData.low);
                            Console.WriteLine("High: " + historicalData.high);
                            Console.WriteLine("Change: " + historicalData.change);
                            Console.WriteLine("Change Percentage: " + historicalData.changePercent);
                            Console.WriteLine("----");
                            Console.WriteLine();
                        }
                    }
                    
                    Console.WriteLine(string.Format("Program has finished processing {0} elements.",historicalDataList.Count));
                }
                else
                {
                    Console.WriteLine(string.Format("No results found for symbol {0}", symbol));
                    Environment.Exit(-1);
                }
            }
        }
    }


    public class HistoricalDataResponse
    {
        public string date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double vwap { get; set; }
        public string label { get; set; }
        public double changeOverTime { get; set; }
    }
}