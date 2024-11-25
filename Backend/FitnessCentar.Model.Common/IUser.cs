using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model.Common
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Password { get; set; }
        string Salt { get; set; }
        string Email { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set; }

        string Contact { get; set; }
        DateTime Birthdate { get; set; }

        double Weight { get; set; }

        double Height { get; set; }
        Guid? CoachId { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DatedUpdated { get; set; }
        bool? IsActive { get; set; }
        Guid RoleId { get; set; }
        Guid SubscriptionId { get; set; }
    }
}
