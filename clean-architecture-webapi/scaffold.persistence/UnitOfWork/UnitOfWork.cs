using Scaffold.Application.Interfaces.Repositories;
using Scaffold.Domain.Entities;
using Scaffold.Domain.Entities.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Scaffold.Persistence.Connections;
using Scaffold.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {


        private readonly ApplicationDbContext _context;

        
        public TenantDbContext TenantDB { get; set; }


        public string TenantKey{ get; set; }

        public bool IgnorePreview { get; set; }

        public UnitOfWork(ApplicationDbContext context, IDapperDbContext dapper,
            TenantDbContext tdbContext, IQueryDbContext queryContext)
        {
            _context = context;

            TenantDB = tdbContext;

            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            Dapper = dapper;

            Query = queryContext;



        }

        public IDapperDbContext Dapper { get; set; }

        public IQueryDbContext Query { get; set; }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();

        }

        public void AttachRange<T>(List<T> entities)
        {
            _context.AttachRange(entities.ToArray());
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public DbSet<T> Set<T>() where T : class
        {

            if (this.IgnorePreview == false && !String.IsNullOrEmpty(_context.PreviewVersion))
            {
                _context.TenantKey= $"{_context.TenantId}_prev_{_context.PreviewVersion}";
            }
            else 
            {
                _context.TenantKey= _context.TenantId;
            }
            this.TenantKey= _context.TenantKey;
            return _context.Set<T>();
        }

        public DbSet<T> SetRoot<T>() where T : class
        {
            _context.TenantKey= _context.TenantId;
            this.TenantKey= _context.TenantKey;
            return _context.Set<T>();
        }

        public ISpecificationRepository<T> SetSpecification<T>() where T : class
        {
            if (this.IgnorePreview == false && !String.IsNullOrEmpty(_context.PreviewVersion))
            {
                _context.TenantKey= $"{_context.TenantId}_prev_{_context.PreviewVersion}";
            }
            else
            {
                _context.TenantKey= _context.TenantId;
            }
            return new Persistence.Repositories.SpecificationRepository<T>(_context);
        }

        public IGenericRepository<T> SetRepository<T>() where T : class
        {
            if (this.IgnorePreview == false && !String.IsNullOrEmpty(_context.PreviewVersion))
            {
                _context.TenantKey= $"{_context.TenantId}_prev_{_context.PreviewVersion}";
            }
            else
            {
                _context.TenantKey= _context.TenantId;
            }
            return new Persistence.Repositories.GenericRepository<T>(_context);
        }

        private string TrackChanges()
        {
            StringBuilder sb = new StringBuilder();
            var entries = _context.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                sb.AppendLine($"Entity Name: {entry.Entity.GetType().FullName}");
                sb.AppendLine($"Status: {entry.State}");
            }
            return sb.ToString();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void CommitTransactionAsync()
        {
            _context.Database.CommitTransactionAsync();
        }
       
    }
}
