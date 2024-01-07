using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<List<Account>> GetAllAsync(bool trackChanges);
        Task<Account> GetOneByIdAsync(int id, bool trackChanges);
        void CreateOne(Account account);
        void UpdateOne(Account account);
        void DeleteOne(Account account);
        Task<Account> GetByUserIdAndAccountNameWithUserDetailsAsync(string userId,string accountName,bool trackChanges);
    }
}
