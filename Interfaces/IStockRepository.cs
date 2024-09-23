using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync();
        Task<Stock?> GetStockByIdAsync(int id);
        Task<Stock> CreateStockAsync(Stock stockModel);
        Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteStockAsync(int id);
        Task<bool> StockExistsAsync(int id);
    }
}   