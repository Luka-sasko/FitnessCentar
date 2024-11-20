using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Common
{
    public class DiscountFilter
    {
        public string SearchQuery {get; set;}
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Amount { get; set; }
    }
}
