using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Mappers;

namespace WebApi.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        // Hold an instance of ApplicationDBContext
        private readonly ApplicationDBContext _context;

        // Inject an instance of ApplicationDBContext into the controller
        public StockController(ApplicationDBContext context)
        {
            // If you want to throw an exception when context is null,
            // you can use the following line:
            // _context = context ?? throw new ArgumentNullException(nameof(context));

            // Let's keep it simple for now
            _context = context; // context may be null here

        }

        [HttpGet]
        public IActionResult GetStocks()
        {
            // Return the DTOs instead of the models
            var stocks = _context.Stocks.ToList()
            // .Select is like mapping the list of Stock models to a list of Stock DTOs
                .Select(s => new StockDto
                {
                    Id = s.Id,
                    Symbol = s.Symbol,
                    CompanyName = s.CompanyName,
                    Purchase = s.Purchase,
                    LastDiv = s.LastDiv,
                    Industry = s.Industry,
                    MarketCap = s.MarketCap
                });
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetStock([FromRoute] int id)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
    }
}