using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class StockRepository : IStockRepository
    {
        // Hold an instance of ApplicationDBContext
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            // We need to inject the ApplicationDBContext here
            _context = context; // context may be null here
        }

        // We need to implement the methods defined in the interface.
        public async Task<List<Stock>> GetAllStocksAsync()
        {
            // Get all the stocks from the database (using an EF Core method)
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            // Get a stock by its ID
            return await _context.Stocks.FindAsync(id).AsTask();
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            // Add a stock to the database
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }
        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto stockDto)
        {
            // Get the stock by its ID
            var existingStockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStockModel == null)
            {
                return null;
            }
            // Update the stock with the new values
            existingStockModel.Symbol = stockDto.Symbol;
            existingStockModel.CompanyName = stockDto.CompanyName;
            existingStockModel.Purchase = stockDto.Purchase;
            existingStockModel.LastDiv = stockDto.LastDiv;
            existingStockModel.Industry = stockDto.Industry;
            existingStockModel.MarketCap = stockDto.MarketCap;
            // Save the changes to the database
            await _context.SaveChangesAsync();

            return existingStockModel;
        }
        public async Task<Stock?> DeleteStockAsync(int id)
        {
            // Get the stock by its ID
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            // Remove the stock from the database
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }
    }
}