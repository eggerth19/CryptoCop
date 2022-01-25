using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
             using(HttpClient client = new HttpClient())
                {
                    using(HttpResponseMessage response = await client.GetAsync("https://data.messari.io/api/v1/markets?page="+pageNumber.ToString()))
                    {
                        var listedResponse = await HttpResponseMessageExtensions.DeserializeJsonToList<ExchangeDto>(response, true);
                        var envelope = new Envelope<ExchangeDto>
                        {
                            PageNumber=pageNumber, 
                            Items=listedResponse
                        };
                        return envelope;
                    }
                }
        }
    }
}