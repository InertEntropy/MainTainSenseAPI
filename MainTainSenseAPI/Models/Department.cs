﻿using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models
{
    public class Department : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the Department.")]
        public string Name { get; set; } = "";

        public ICollection<ApplicationUser>? Users { get; set; } // Navigation property
    }
}
