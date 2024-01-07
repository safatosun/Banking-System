using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account : BaseEntity<int>
    {       
        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string Name {get; set; }
        public decimal Balance { get; set; }
        public int TransferLimit { get; set; }  
        public ICollection<Invoice> Invoices { get; set; }
        
    }
}
