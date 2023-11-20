using Scaffold.Application.Interfaces.Services;
using Dapper;
using Scaffold.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scaffold.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections;
using Scaffold.Domain.Entities.AuditTrial;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;

namespace Scaffold.Persistence.Connections
{

    public class QueryDbContext : IQueryDbContext, IDisposable
    {
        private readonly IDbConnection connection;

        private readonly SqlKata.Compilers.Compiler compiler;

        private readonly SqlKata.Execution.QueryFactory _queryFactory;

        private SqlKata.Query _query;

        public string TenantKey { get; }

        public string Schema { get; }

        private int? commandTimeout { get; }


        public QueryDbContext(IConfiguration configuration, ITenantService tenantService)
        {
            var tenantConnectionString = tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "postgresql")
                {
                    compiler = new SqlKata.Compilers.PostgresCompiler();
                    connection = new OracleConnection(tenantService.GetConnectionString());
                    //commandTimeout = ((OracleConnection)connection).CommandTimeout;
                    _queryFactory = new QueryFactory(connection, compiler);
                }
            }

            TenantKey = tenantService.GetCurrentTenant()?.TenantId;

        }

        public void Dispose()
        {
            connection?.Dispose();
        }

        
        public IQueryDbContext Where(string column, object value)
        {
            _query.Where(column.ToLower(), "=", value);
            return this;
        }

        public IQueryDbContext OrWhere(string column, object value)
        {
            _query.OrWhere(column.ToLower(), "=", value);
            return this;
        }

        public IQueryDbContext AndWhere(string column, object value)
        {           
            return this.Where(column.ToLower(), value);
        }


        public IQueryDbContext From<T>()
        {
            TableAttribute tableEntity = (TableAttribute)typeof(T).GetCustomAttributes(true)[0];
            if (_query != null) _query.NewQuery();
            _query = _queryFactory.Query(tableEntity.Name);
            return this;
        }

        public IQueryDbContext WithDefaultFilters()
        {
            _query.Where("client_key", "=", this.TenantKey);
            return this;
        }

        public async Task<IEnumerable<R>> GetAsync<R>()
        {
            return await _queryFactory.GetAsync<R>(_query,null, commandTimeout, CancellationToken.None);
        }

    }

    public class DapperDbContext : IDapperDbContext, IDisposable
    {
        private readonly IDbConnection connection;

        public string Schema { get; }

        public string TenantKey { get; }

        //   public DbSet<User> Users => throw new NotImplementedException();

        public DapperDbContext(IConfiguration configuration, ITenantService tenantService)
        {
            var tenantConnectionString = tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "oracle")
                {
                    connection = new OracleConnection(tenantService.GetConnectionString());
                }
            }
            Schema = tenantService.GetCurrentTenant()?.Schema;

            TenantKey = tenantService.GetCurrentTenant()?.TenantId;

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public SqlKata.Execution.QueryFactory FromSqlKataSql()
        {
            var qf = new SqlKata.Execution.QueryFactory(connection, new SqlKata.Compilers.PostgresCompiler());
            return qf;
        }

        private string GetDefaultSqlQuery<T>()
        {


            var tAttribute = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), true)[0];
            var tableName = tAttribute.Name;
            return $"select * from {Schema}.{tableName}";
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(GetDefaultSqlQuery<T>(), param, transaction))?.AsList();
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(sql, param, transaction)).AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleAsync<T>(sql, param, transaction);
        }


        public async Task<T> QueryFirstOrDefaultAsync<T>(object param, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(GetDefaultSqlQuery<T>(), param, transaction);
        }

        public async Task<T> QuerySingleAsync<T>(object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleAsync<T>(GetDefaultSqlQuery<T>(), param, transaction);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }

    }
}