using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Scaffold.Application.Interfaces.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> IsSatisfiedBy { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
    }
}
