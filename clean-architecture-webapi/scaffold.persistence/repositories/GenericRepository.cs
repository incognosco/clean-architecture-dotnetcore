using Microsoft.EntityFrameworkCore;
using Scaffold.Persistence.Contexts;
using Scaffold.Persistence.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Application.Interfaces.Repositories;
using Scaffold.Application.Interfaces.Specifications;
using System.Linq.Expressions;
using System.Linq;
using Scaffold.Domain.Common;

namespace Scaffold.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public DbSet<T> Entity { get ; set ; }

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            Entity = _context.Set<T>();
        }


        #region "Generic Repository"

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public Task<int> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            return Task.FromResult(0);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression, Expression<Func<T, bool>> orderByExpression)
        {
            return _context.Set<T>().Where(expression).OrderBy(orderByExpression);
        }


        public R FindMax<R>(Expression<Func<T, bool>> expression, Expression<Func<T, R>> maxExpression)
        {
            return _context.Set<T>().Where(expression).Max(maxExpression);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity) 
        {
            _context.Update(entity);
        }


        public Task<int> UpdateAsync(T entity)
        {
            _context.Update(entity);
            return Task.FromResult(0);

        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        #endregion

     
    }
}
