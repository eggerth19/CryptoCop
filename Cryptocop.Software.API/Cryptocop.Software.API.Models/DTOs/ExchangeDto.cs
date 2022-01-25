using System;
using Newtonsoft.Json;

namespace Cryptocop.Software.API.Models.DTOs
{
    public class ExchangeDto
    {
        [JsonProperty("exchange_id")]public string Id { get; set; }
        [JsonProperty("base_asset_symbol")]public string AssetSymbol { get; set; }
        [JsonProperty("exchange_name")]public string Name { get; set; }
        [JsonProperty("exchange_slug")]public string Slug { get; set; }
        [JsonProperty("price_usd")]public Nullable<float> PriceInUsd { get; set; }
        [JsonProperty("last_trade_at")]public Nullable<DateTime> LastTrade { get; set; }
    }
}

/*ExchangeDto
• Id (int)
• Name (string)
• Slug (string)
• AssetSymbol (string)
• PriceInUsd (nullable float)
• LastTrade (nullable datetime)*/