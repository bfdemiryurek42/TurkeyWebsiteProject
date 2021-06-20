using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkeyWebsiteProject.Models
{
    public class Food
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Image { get; set; }
        public string Description { get; set; }

        public City City { get; set; }

    }
}
