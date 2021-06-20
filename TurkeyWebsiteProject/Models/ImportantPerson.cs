using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkeyWebsiteProject.Models
{
    public class ImportantPerson
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public string Image { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }

        public City City { get; set; }
    }
}
