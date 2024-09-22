using WebApi.DTO;
using WebApi.Models; // Import the DTO namespace so I can use the StockDto class.

namespace WebApi.Mappers
{
    public static class StockMappers
    {
        // This method will convert a Stock model to a Stock DTO.
        public static StockDto ToStockDto(this Models.Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap
            };
        }

        public static Stock FromDtoToStock(this StockDto stockDto)
        {
            return new Models.Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
    }
}