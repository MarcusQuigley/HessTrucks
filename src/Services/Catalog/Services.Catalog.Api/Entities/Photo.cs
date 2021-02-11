using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Entities
{
    public class Photo
    {
        public Guid PhotoId { get; set; }
        public string PhotoPath { get; set; }

        public Guid TruckId { get; set; }
        public Truck Truck { get; set; }
    }
}
