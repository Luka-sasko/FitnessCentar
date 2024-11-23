using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model
{
    public class Subscription : ISubscription
    {
        public Guid Id { set; get; }
        public decimal Price { set; get; }
        public string Description { set; get; }
        public string Name { set; get; }
        public DateTime StartDate { set; get; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public Guid DiscountId { get; set; }
        public int Duration { set; get; }
    }
    
}
