using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkeyWebsiteProject.Models
{
    public class Territory
    {
        [Range(1, 7)]
        public int Id { get; set; }
        public string Name { get; set; }


    }
}
