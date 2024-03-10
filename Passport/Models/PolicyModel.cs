using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passport.Models
{
    public class PolicyModel
    {
        public string Type { get; set; }
        public int? Days { get; set; }
        public string Country { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsSmoker { get; set; }
        public string Gender { get; set; }
        public decimal? Deductible { get; set; }
    }

}
