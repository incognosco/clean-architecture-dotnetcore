using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Application.Interfaces.Repositories
{
    public interface IDBContext
    {

        void AddRange(params object[] entities);

        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);

       

    }
}
