using Scaffold.Application.Extentions;
using Scaffold.Application.Interfaces.Specifications;
using Scaffold.Domain.Common;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Scaffold.Application.Handlers
{


    public class BaseSpecifcation<T> : ISpecification<T> where T : class
    {
        public BaseSpecifcation()
        {
        }

        public BaseSpecifcation(Expression<Func<T, bool>> criteria)
        {
            IsSatisfiedBy = criteria;
            IncludeStrings = new List<string>();
        }

        public Expression<Func<T, bool>> IsSatisfiedBy { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; }

        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        public Expression<Func<T, bool>> And(Expression<Func<T, bool>> query)
        {
            return IsSatisfiedBy = IsSatisfiedBy == null ? query : IsSatisfiedBy.And(query);
        }

        public BaseSpecifcation<T> And(BaseSpecifcation<T> spec)
        {
            var query = spec.IsSatisfiedBy;
            IsSatisfiedBy = IsSatisfiedBy == null ? query : IsSatisfiedBy.And(query);
            return this;
        }

        public BaseSpecifcation<T> Or(BaseSpecifcation<T> spec)
        {
            var query = spec.IsSatisfiedBy;
            IsSatisfiedBy = IsSatisfiedBy == null ? query : IsSatisfiedBy.Or(query);
            return this;
        }

        public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query)
        {
            return IsSatisfiedBy = IsSatisfiedBy == null ? query : IsSatisfiedBy.Or(query);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
    }
}
