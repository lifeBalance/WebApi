using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
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
        public Task<List<Stock>> GetAllAsync()
        {
            // Get all the stocks from the database (using an EF Core method)
            return _context.Stocks.ToListAsync();
        }
    }
}