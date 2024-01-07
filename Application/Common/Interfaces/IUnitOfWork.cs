using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        IInvoiceRepository Invoice { get; }
        ISupportRequestRepository SupportRequest { get; }
        ICreditRepository Credit { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsyncWithPessimisticLocking();
        Task TransactionCommitAsync();
    }
}
