using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SupportRequest : BaseEntity<int>
    {
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public string Text { get; set; }
        public int Priority { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
