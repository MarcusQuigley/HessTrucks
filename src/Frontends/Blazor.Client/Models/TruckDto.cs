using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Client.Models
{
    public class TruckDto
    {
        public Guid TruckId { get; set; }
        [Required]
        [StringLength(50,ErrorMessage ="Name is too long")]
        public string Name { get; set; }
        [Required]
     
        public int Year { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Description is too long")]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal PreviousPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        public bool Hidden { get; set; }
        public bool Damaged { get; set; }
        public string DefaultPhotoPath { get; set; }

        public virtual ICollection<PhotoDto> Photos { get; set; }
        public virtual ICollection<CategoryDto> Categories { get; set; }
    }
}
