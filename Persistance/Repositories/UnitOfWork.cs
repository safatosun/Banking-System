using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly BankingSystemDbContext _dbContext;
        private IDbContextTransaction _dbTransaction;
        private readonly IAccountRepository _accountRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISupportRequestRepository _supportRequestRepository;
        private readonly ICreditRepository _creditRepository;

        public UnitOfWork(BankingSystemDbContext dbContext, IAccountRepository accountRepository, IInvoiceRepository invoiceRepository, ISupportRequestRepository supportRequestRepository, ICreditRepository creditRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _invoiceRepository = invoiceRepository;
            _supportRequestRepository = supportRequestRepository;
            _creditRepository = creditRepository;
        }

        public IAccountRepository Account => _accountRepository;

        public IInvoiceRepository Invoice => _invoiceRepository;

        public ISupportRequestRepository SupportRequest => _supportRequestRepository;

        public ICreditRepository Credit => _creditRepository;

        public async Task BeginTransactionAsyncWithPessimisticLocking()
        {
            _dbTransaction = await  _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }

        public async Task TransactionCommitAsync()
        {
            try
            {
                await _dbTransaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await _dbTransaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
            finally
            {
                await _dbTransaction.DisposeAsync();
            }
        }
    }
}
