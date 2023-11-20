using Microsoft.EntityFrameworkCore;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Scaffold.Application.Interfaces.Repositories
{
    public interface IQueryDbContext
    {
        IQueryDbContext From<T>();

        IQueryDbContext WithDefaultFilters();

        IQueryDbContext Where(string column, object value);

        IQueryDbContext AndWhere(string column, object value);

        IQueryDbContext OrWhere(string column, object value);

        Task<IEnumerable<R>> GetAsync<R>();
    }

    public interface ISqlKataDbContext
    {
        Query Where(string column, string op, object value);
        Query WhereNot(string column, string op, object value);
        Query OrWhere(string column, string op, object value);
        Query OrWhereNot(string column, string op, object value);
        Query Where(string column, object value);
        Query WhereNot(string column, object value);
        Query OrWhere(string column, object value);
        Query OrWhereNot(string column, object value);
        Query Where(object constraints);

        Query Where(IEnumerable<KeyValuePair<string, object>> values);
        Query WhereRaw(string sql, params object[] bindings);
        Query OrWhereRaw(string sql, params object[] bindings);
        Query Where(Func<Query, Query> callback);
        Query WhereNot(Func<Query, Query> callback);
        Query OrWhere(Func<Query, Query> callback);
        Query OrWhereNot(Func<Query, Query> callback);

        Query WhereColumns(string first, string op, string second);

        Query OrWhereColumns(string first, string op, string second);
        Query WhereNull(string column);

        Query WhereNotNull(string column);
        Query OrWhereNull(string column);

        Query OrWhereNotNull(string column);

        Query WhereTrue(string column);
        Query OrWhereTrue(string column);

        Query WhereFalse(string column);

        Query OrWhereFalse(string column);

        Query WhereLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null);
        Query WhereNotLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query OrWhereLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query OrWhereNotLike(string column, object value, bool caseSensitive = false, string escapeCharacter = null);
        Query WhereStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null);
        Query WhereNotStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query OrWhereStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query OrWhereNotStarts(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query WhereEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null);
        Query WhereNotEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null);
        Query OrWhereEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query OrWhereNotEnds(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query WhereContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null);
        Query WhereNotContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query OrWhereContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query OrWhereNotContains(string column, object value, bool caseSensitive = false, string escapeCharacter = null);

        Query WhereBetween<T>(string column, T lower, T higher);
        Query OrWhereBetween<T>(string column, T lower, T higher);
        Query WhereNotBetween<T>(string column, T lower, T higher);
        Query OrWhereNotBetween<T>(string column, T lower, T higher);
        Query WhereIn<T>(string column, IEnumerable<T> values);
        Query OrWhereIn<T>(string column, IEnumerable<T> values);
        Query WhereNotIn<T>(string column, IEnumerable<T> values);
        Query OrWhereNotIn<T>(string column, IEnumerable<T> values);
        Query WhereIn(string column, Query query);
        Query WhereIn(string column, Func<Query, Query> callback);

        Query OrWhereIn(string column, Query query);
        Query OrWhereIn(string column, Func<Query, Query> callback);
        Query WhereNotIn(string column, Query query);
        Query WhereNotIn(string column, Func<Query, Query> callback);
        Query OrWhereNotIn(string column, Query query);
        Query OrWhereNotIn(string column, Func<Query, Query> callback);
        Query Where(string column, string op, Func<Query, Query> callback);

        Query Where(string column, string op, Query query);
        Query WhereSub(Query query, object value);
        Query WhereSub(Query query, string op, object value);
        Query OrWhereSub(Query query, object value);
        Query OrWhereSub(Query query, string op, object value);
        Query OrWhere(string column, string op, Query query);
        Query OrWhere(string column, string op, Func<Query, Query> callback);
        Query WhereExists(Query query);
        Query WhereExists(Func<Query, Query> callback);
        Query WhereNotExists(Query query);
        Query WhereNotExists(Func<Query, Query> callback);
        Query OrWhereExists(Query query);
        Query OrWhereExists(Func<Query, Query> callback);
        Query OrWhereNotExists(Query query);
        Query OrWhereNotExists(Func<Query, Query> callback);
        Query WhereDatePart(string part, string column, string op, object value);
        Query WhereNotDatePart(string part, string column, string op, object value);
        Query OrWhereDatePart(string part, string column, string op, object value);
        Query OrWhereNotDatePart(string part, string column, string op, object value);
        Query WhereDate(string column, string op, object value);
        Query WhereNotDate(string column, string op, object value);
        Query OrWhereDate(string column, string op, object value);
        Query OrWhereNotDate(string column, string op, object value);

        Query WhereTime(string column, string op, object value);
        Query WhereNotTime(string column, string op, object value);
        Query OrWhereTime(string column, string op, object value);
        Query OrWhereNotTime(string column, string op, object value);
        Query WhereDatePart(string part, string column, object value);
        Query WhereNotDatePart(string part, string column, object value);
        Query OrWhereDatePart(string part, string column, object value);
        Query OrWhereNotDatePart(string part, string column, object value);
        Query WhereDate(string column, object value);
        Query WhereNotDate(string column, object value);
        Query OrWhereDate(string column, object value);
        Query OrWhereNotDate(string column, object value);
        Query WhereTime(string column, object value);
        Query WhereNotTime(string column, object value);
        Query OrWhereTime(string column, object value);
        Query OrWhereNotTime(string column, object value);
    }
}