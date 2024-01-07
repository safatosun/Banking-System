using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(BankingSystemDbContext context) : base(context)
        {
        }


        public async Task<List<Invoice>> GetAllAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(x => x.Id).ToListAsync();
        }

        public void CreateOne(Invoice invoice)
        {
            Create(invoice);
        }

        public void DeleteOne(Invoice invoice)
        {
            Delete(invoice);
        }
       

        public void UpdateOne(Invoice invoice)
        {
            Update(invoice);
        }

        public async Task<List<Invoice>> GetAllByAccountIdAsync(int accountId)
        {
           return await _dbContext.Invoices.Include(i => i.Account).Where(i => i.AccountId == accountId).ToListAsync();
        }

        public async Task<Invoice> GetByIdWithDetailsAsync(int id )
        {
            return await _dbContext.Invoices.Include(i => i.Account).SingleOrDefaultAsync(i=>i.Id==id);
        }

        public async Task<Invoice> GetOneByIdAsync(int id, bool trackChanges)
        {
            var invoice = await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

            return invoice;
        }

        public async Task<List<Invoice>> GetAllAutoPaysWithDetailsAsync()
        {
          return await _dbContext.Invoices.Include(i => i.Account).Where(i=>i.IsAutomated==true).ToListAsync();
        }
    }
}
