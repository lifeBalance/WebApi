using System.ComponentModel.DataAnnotations;
using WebApi.Models;

namespace WebApi.DTO
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol must be at most 10 characters long")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "Company name must be at most 10 characters long")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1_000_000_000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Industry must be at most 10 characters long")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(1, 5_000_000_000)]
        public long MarketCap { get; set; }

        public Stock ToStock()
        {
            return new Stock
            {
                Symbol = this.Symbol,
                CompanyName = this.CompanyName,
                Purchase = this.Purchase,
                LastDiv = this.LastDiv,
                Industry = this.Industry,
                MarketCap = this.MarketCap
            };
        }
    }
}