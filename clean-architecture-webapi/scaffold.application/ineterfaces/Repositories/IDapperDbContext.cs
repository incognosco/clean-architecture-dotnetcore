using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Scaffold.Application.Interfaces.Repositories
{
    public interface IDapperDbContext
    {
        string Schema { get; }
        string TenantKey { get; }

        Task<IReadOnlyList<T>> QueryAsync<T>(object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<T> QueryFirstOrDefaultAsync<T>(object param, IDbTransaction transaction, CancellationToken cancellationToken);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<T> QuerySingleAsync<T>(object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
    }
}