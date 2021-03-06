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
        [Display(Name = "City")]
        public int CityId { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]

        public string LastName { get; set; }
        public string Image { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Date Of Death")]
        public DateTime DateOfDeath { get; set; }

        public City City { get; set; }
    }
}
