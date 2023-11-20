using Scaffold.Domain.Entities;
using Scaffold.Domain.Entities.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {

        Task<IDbContextTransaction> BeginTransactionAsync();

        void CommitTransactionAsync();

        IDapperDbContext Dapper { get; set; }

        IQueryDbContext Query { get; set; }


        DbSet<T> Set<T>() where T: class;

        DbSet<T> SetRoot<T>() where T : class;

        bool IgnorePreview { get; set; }

        string TenantKey { get; set; }
        
        IGenericRepository<T> SetRepository<T>() where T : class;

        ISpecificationRepository<T> SetSpecification<T>() where T : class;

        Task<int> CompleteAsync();

    }
}
