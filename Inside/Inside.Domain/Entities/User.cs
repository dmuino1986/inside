using Inside.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Inside.Domain.Entities
{
    public class User : IdentityUser<int>,IBaseEntity {

        public string Name { get; set; }
        public string Lastname { get; set;}

        public string Address { get; set; }
        public string State { get; set; }
        public string CarPlate { get; set; }

        public double Coins { get; set; }
        public List<Parking> Parkings { get; set; }
        public List<Order> Orders { get; set; }

    }
}