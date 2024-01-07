using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IBankingSystemDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Invoice> Invoices { get; set; }   
        DbSet<Credit> Credits { get; set; }
        DbSet<SupportRequest> SupportRequests { get; set; }

        Task<int> SaveChangesAsync();
    }
}
