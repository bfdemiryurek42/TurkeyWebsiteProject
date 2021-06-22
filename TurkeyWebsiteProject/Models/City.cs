using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkeyWebsiteProject.Models
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [Range(1, 7)]
        [Display(Name = "Territory")]
        public int TerritoryId { get; set; }
        [Display(Name = "Food")]
        public int FoodId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public Territory Territory { get; set; }
        public Food Food { get; set; }
    }
}
