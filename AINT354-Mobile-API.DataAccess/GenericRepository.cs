using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamingSessionApp.DataAccess
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        //Get an entity using the primary key 'id'
        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        //Get an entity using the primary key 'id' async
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        //Insert a new entity object in to the db
        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        //Delete an entity object from the db by id
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        //Delete an entity object from the db
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        //Mark the the entity object as modified/updated
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
