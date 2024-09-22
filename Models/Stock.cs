using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]   // Purchase is money: 18 digits, 2 decimal places
        public decimal Purchase { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]   // Purchase is money: 18 digits, 2 decimal places
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

        // One-to-Many relationship: Stock to Comments (ONE Stock can have MANY Comments)
        public List<Comment> Comments { get; set; } = new List<Comment>(); // = []
    }
}