using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PackageChallenge.Model.Models
{
    public class Package
    {
        public Package()
        {
            Items = new List<PackageItem>();
        }
        [Required]
        public int Weight { get; set; }
        public List<PackageItem> Items { get; set; }
    }
}
