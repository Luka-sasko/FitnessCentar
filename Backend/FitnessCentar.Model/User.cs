﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Contact { get; set; }
        public DateTime Birthday { get; set; }

        public decimal Weight { get; set; }

        public decimal Height { get; set; }
        public Guid CoachId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        public Guid RoleId { get; set; }
        public Guid SubscriptionId { get; set; }



    }
}
