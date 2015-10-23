using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingSessionApp.DataAccess
{
    public interface IRepository<T>
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
