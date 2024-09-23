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
            var stocks = await _stockRepo.GetAllStocksAsync();

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
            var stock = await _stockRepo.GetStockByIdAsync(id);
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
            var stockModel = stockDto.ToStock();
            // Add the stock to the database using the repository
            await _stockRepo.CreateStockAsync(stockModel);
            return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            // Find the stock by id
            var stockModel = await _stockRepo.UpdateStockAsync(id, stockDto);
            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stockModel = await _stockRepo.DeleteStockAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}