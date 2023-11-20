using Scaffold.Application.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);

        IEnumerable<T> Find(Expression<Func<T, bool>> expression, Expression<Func<T, bool>> orderByexpression);

        R FindMax<R>(Expression<Func<T, bool>> expression, Expression<Func<T, R>> maxExpression);

        
        void Add(T entity);

        public Task<int> AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);
        
        Task<int> UpdateAsync(T entity);
        
        void UpdateRange(IEnumerable<T> entities);

        DbSet<T> Entity { get; set; }
    }

    public interface ISpecificationRepository<T> where T : class
    {
        IEnumerable<T> Find(ISpecification<T> specification = null);
    }
}
