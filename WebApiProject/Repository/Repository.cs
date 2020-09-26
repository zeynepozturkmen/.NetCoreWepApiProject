using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiProject.DbContexts;
using WebApiProject.Entities;
using WebApiProject.Interface;

namespace WebApiProject.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly UserDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public Repository(UserDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            //await=Bu işlem bitene kadar bu satırda kalmasını saglıyor
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable().Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsQueryable()
                             .Where(predicate)
                             .Where(x => !x.IsDeleted)
                             .OrderByDescending(x => x.CreatedDate);

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsQueryable().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.AsQueryable().Where(q => q.Id == id && !q.IsDeleted).FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public T Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return entity;
        }


    }
}
