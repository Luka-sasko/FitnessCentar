using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model
{
    public class User:IUser
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        
        public string Contact { get; set; }
        public DateTime Birthdate { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }
        public Guid? CoachId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatedUpdated { get; set; }
        public bool? IsActive { get; set; }
        public Guid RoleId { get; set; }
        public Guid SubscriptionId { get; set; }
       
    }
}
