using WebApi.DTO; // Import the DTO namespace so I can use the StockDto class.

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
    }
}