using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model.Common
{
    public interface ISubscription
    {
        Guid Id { set; get; }
        decimal Price { set; get; }
        string Description { set; get; }
        string Name { set; get; }
        int Duration { set; get; }
        DateTime StartDate { set; get; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
        Guid DiscountId { get; set; } 

    }
}
