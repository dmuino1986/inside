using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inside.Domain.Core;

namespace Inside.Domain.Entities
{
    public class ParkingCategory : BaseEntity
    {
        public string Category { get; set; }
        public double Price { get; set; }
        public double CoinPrice { get; set; }

        public List<Parking> Parkings { get; set; }
    }
}