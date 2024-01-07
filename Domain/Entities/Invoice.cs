using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Invoice : BaseEntity<int>
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public int AccountId { get; set; }
        
        [ForeignKey("AccountId")]
        public Account Account { get; set; }           
        public decimal TotalAmount { get; set; }    
        public bool IsAutomated { get; set; }   

    }
}
