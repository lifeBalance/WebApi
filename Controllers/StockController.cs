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

        [HttpPost]
        public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto)
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
            _context.Stocks.Add(stockModel);
            // Save the changes to the database
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("api/{id}")]
        public IActionResult UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            // Find the stock by id
            var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);
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
            _context.SaveChanges();
            // return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, stockModel.ToStockDto());
            return Ok(stockModel.ToStockDto());
        }
    }
}