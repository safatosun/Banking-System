using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Credit : BaseEntity<int>
    {
        public string Name { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int RequestedAmount { get; set; } //istenilen para
        public int Maturity { get; set; } //vade
        public decimal InstallmentAmount { get; set; } //taksit tutarı
        public int? InstallmentDueDay { get; set; } // 2      
        public bool? IsProcessed { get; set; }

        public bool? Approved { get; set; } //onaylama
        public int? PaidInstallmentCount {get;set;} //ödenen taksit
        public bool? Paid { get; set; }  //ödendi mi
    }
}
