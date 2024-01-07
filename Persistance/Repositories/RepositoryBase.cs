using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
        
    {
        protected readonly BankingSystemDbContext _dbContext;

        public RepositoryBase(BankingSystemDbContext context)
        {
            _dbContext = context;
        }

        public void Create(T entity) => _dbContext.Set<T>().Add(entity);

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
            _dbContext.Set<T>().AsNoTracking() :
            _dbContext.Set<T>();


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
            _dbContext.Set<T>().Where(expression).AsNoTracking() :
            _dbContext.Set<T>().Where(expression);

        public void Update(T entity) => _dbContext.Set<T>().Update(entity);
    }
}
