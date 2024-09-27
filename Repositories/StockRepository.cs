using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Mappers;
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
        public async Task<List<Stock>> GetAllStocksAsync(QueryObject queryObject)
        {
            // Get all the stocks from the database (using an EF Core method)
            // return await _context.Stocks.Include(c => c.Comments).ToListAsync();
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(queryObject.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(queryObject.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = queryObject.IsSortDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                if (queryObject.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = queryObject.IsSortDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }
            // Pagination
            // If page number is 1, we skip 0 records(because 1 - 1 = 0).
            // If page number is 2, we skip 1 * pageSize records.
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

            return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            // Get a stock by its ID
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
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

        public async Task<bool> StockExistsAsync(int id)
        {
            // Check if a stock exists by its ID
            return await _context.Stocks.AnyAsync(e => e.Id == id);
        }
    }
}