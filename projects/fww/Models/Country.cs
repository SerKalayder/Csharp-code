using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fww.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<Region> Regions { get; set; }
    }

    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}