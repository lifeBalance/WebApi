using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Interfaces;
using WebApi.Mappers;

namespace WebApi.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        // MARKED FOR REMOVAL: DBContext; delete when IStockRepository is implemented
        // in all the methods.

        // Hold an instance of ApplicationDBContext
        private readonly ApplicationDBContext _context;
        // Repository pattern
        private readonly IStockRepository _stockRepo;

        // Inject an instance of ApplicationDBContext into the controller
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            // If you want to throw an exception when context is null,
            // you can use the following line:
            // _context = context ?? throw new ArgumentNullException(nameof(context));

            // Let's keep it simple for now
            _context = context; // context may be null here
            _stockRepo = stockRepo;

        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            // Get all the stocks from the repository (db agnostic)
            var stocks = await _stockRepo.GetAllAsync();

            // Map the list of Stock models to a list of Stock DTOs
            var stocksDto = stocks
                .Select(s => new StockDto // .Select() is a LINQ method
                {
                    Id = s.Id,
                    Symbol = s.Symbol,
                    CompanyName = s.CompanyName,
                    Purchase = s.Purchase,
                    LastDiv = s.LastDiv,
                    Industry = s.Industry,
                    MarketCap = s.MarketCap
                });
            // Return the DTOs instead of the models
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStock([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            // Convert the DTO to a model
            var stockModel = new Models.Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
            // Track the model in the context
            await _context.Stocks.AddAsync(stockModel);
            // Save the changes to the database
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            // Find the stock by id
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }

            // When EF finds the stock entity, it will start tracking it in the context.
            // (full) Update the stock model with the DTO
            stockModel.Symbol = stockDto.Symbol;
            stockModel.CompanyName = stockDto.CompanyName;
            stockModel.Purchase = stockDto.Purchase;
            stockModel.LastDiv = stockDto.LastDiv;
            stockModel.Industry = stockDto.Industry;
            stockModel.MarketCap = stockDto.MarketCap;

            // Save the changes to the database
            await _context.SaveChangesAsync();
            // return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, stockModel.ToStockDto());
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            // Do not add the await to the Remove method
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}