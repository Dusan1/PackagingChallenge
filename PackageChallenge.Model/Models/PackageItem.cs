using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PackageChallenge.Model.Models
{
    public class PackageItem
    {
        [Required]
        public int Index { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
