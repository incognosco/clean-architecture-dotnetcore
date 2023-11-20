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
    public class SpecificationRepository<T> : ISpecificationRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

       
        public SpecificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region "Specification Pattern"

        public IEnumerable<T> Find(ISpecification<T> specification = null)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }

        #endregion

    }
 }
