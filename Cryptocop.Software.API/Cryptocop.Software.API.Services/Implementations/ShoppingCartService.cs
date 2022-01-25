using Cryptocop.Software.API.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using System;
using Cryptocop.Software.API.Models.Exceptions;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        public readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingCartRepository.GetCartItems(email);
        }

        public async Task AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem)
        {
            using(HttpClient client = new HttpClient())
                {
                    using(HttpResponseMessage response = await client.GetAsync("https://data.messari.io/api/v2/assets?fields=id,slug,symbol,metrics/market_data/price_usd"))
                    {
                        var listedResponse = await HttpResponseMessageExtensions.DeserializeJsonToList<CryptocurrencyDto>(response, true);
                        foreach(CryptocurrencyDto cryptoDto in listedResponse)
                        {
                            if (String.Compare(cryptoDto.Symbol.ToLower(), shoppingCartItemItem.ProductIdentifier.ToLower(), true) == 0)
                            {
                                await _shoppingCartRepository.AddCartItem(email, shoppingCartItemItem, cryptoDto.PriceInUsd);
                                return;
                            }
                        }
                        throw new ResourceNotFoundException($"Cryptocurrenty {shoppingCartItemItem.ProductIdentifier} was not found.");
                   
                    }
                }
        }

        public void RemoveCartItem(string email, int id)
        {
            _shoppingCartRepository.RemoveCartItem(email, id);
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
        }

        public void ClearCart(string email)
        {
            _shoppingCartRepository.ClearCart(email);
        }
    }
}
