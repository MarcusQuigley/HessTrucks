using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Entities
{
    public class Truck
    {
        public Truck()
        {
            Photos = new List<Photo>();
            Categories = new List<Category>();
        }
        public Guid TruckId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PreviousPrice { get; set; }
        public int Quantity { get; set; }
        public bool Hidden { get; set; }
        public bool Damaged { get; set; }
        public string DefaultPhotoPath { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
