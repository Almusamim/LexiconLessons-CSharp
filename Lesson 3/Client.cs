using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniProjectOne
{
    class Client
    {
        public static (decimal MYR, decimal SEK, DateTime latestUpdate) Rates()
        {
            try
            {
                return (_rates.MYR, _rates.SEK, _rates.latestUpdate);
            }
            catch
            {
                return (4.16m, 8.68m, new DateTime(2021, 11, 12, 17, 30, 00));
            }
        }

        private static readonly (decimal MYR, decimal SEK, DateTime latestUpdate) _rates = FetchRates("https://open.er-api.com/v6/latest/USD").Result;

        private static async Task<(decimal MYR, decimal SEK, DateTime latestUpdate)> FetchRates(string url)
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync("https://open.er-api.com/v6/latest/USD");

            // More info for this solution https://zetcode.com/csharp/json/
            using JsonDocument doc = JsonDocument.Parse(content);
            JsonElement root = doc.RootElement;

            var time_last_update_unix = root.GetProperty("time_last_update_unix").GetInt64();
            DateTime latestUpdate = DateTimeOffset.FromUnixTimeSeconds(time_last_update_unix).DateTime;

            var MYR = root.GetProperty("rates").GetProperty("MYR").GetDecimal();
            var SEK = root.GetProperty("rates").GetProperty("SEK").GetDecimal();

            return (MYR, SEK, latestUpdate);
        }
    }
}
