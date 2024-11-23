using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Common
{
    public class SubscriptionFilter
    {
        public string SearchQuery { get; set; }
        public int? Duration { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? Price { set; get; }

    }
}
