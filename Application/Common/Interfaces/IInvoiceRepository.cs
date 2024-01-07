using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IInvoiceRepository : IRepositoryBase<Invoice>
    {
        Task<List<Invoice>> GetAllAsync(bool trackChanges);
        Task<List<Invoice>> GetAllAutoPaysWithDetailsAsync();
        Task<Invoice> GetOneByIdAsync(int id, bool trackChanges);
        void CreateOne(Invoice invoice);
        void UpdateOne(Invoice invoice);
        void DeleteOne(Invoice invoice);
        Task<List<Invoice>> GetAllByAccountIdAsync(int accountId);
        Task<Invoice> GetByIdWithDetailsAsync(int id );
    }
}
