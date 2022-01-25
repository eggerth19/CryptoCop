using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        public async Task<IEnumerable<CryptocurrencyDto>> GetAvailableCryptocurrencies()
        {
            using(HttpClient client = new HttpClient())
                {
                    using(HttpResponseMessage response = await client.GetAsync("https://data.messari.io/api/v2/assets?fields=id,name,slug,symbol,metrics/market_data/price_usd,profile/general/overview/project_details"))
                    {
                        var listedResponse = await HttpResponseMessageExtensions.DeserializeJsonToList<CryptocurrencyDto>(response, true);
                        List<CryptocurrencyDto> selectedCurrency = new List<CryptocurrencyDto>();
                        var currencies = new List<string>() { "ETH", "USDT", "BTC", "XMR" };
                        foreach(CryptocurrencyDto cryptoDto in listedResponse)
                        {
                            if(currencies.Contains(cryptoDto.Symbol))
                            {
                                selectedCurrency.Add(cryptoDto);
                            }
                        }
                        return selectedCurrency;
                    }
                }
        }
    }
}