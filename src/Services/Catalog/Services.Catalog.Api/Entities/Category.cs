using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Entities
{
    public class Category
    {
        public Category()
        {
            Trucks = new List<Truck>();
        }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsMiniTruck { get; set; }
        public int Order { get; set; }
        public virtual ICollection<Truck> Trucks { get; set; }
    }
}
