﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkeyWebsiteProject.Models
{
    public class City
    {
        [Range(1,81)]
        public int Id { get; set; }
        [Required]
        [Range(1, 7)]
        public int TerritoryId { get; set; }
        public int FoodId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Image { get; set; }
        public string Description { get; set; }

        public Territory Territory { get; set; }
        public Food Food { get; set; }
    }
}