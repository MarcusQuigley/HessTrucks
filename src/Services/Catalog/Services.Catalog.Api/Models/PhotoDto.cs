using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Models
{
    public class PhotoDto
    {
        public Guid PhotoId { get; set; }
        public string PhotoPath { get; set; }
        public Guid TruckId { get; set; }
    }
}
