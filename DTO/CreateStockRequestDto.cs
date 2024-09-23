using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DTO
{
    public class CreateStockRequestDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
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