using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public String FirstName { get; set; }
       
        public String LastName { get; set; }

        public String? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpireTime { get; set; }

        public ICollection<Account> Accounts { get; set; }
        public ICollection<Credit> Credits { get; set; }
        public ICollection<SupportRequest> SupportRequests { get; set; }

    }
}
