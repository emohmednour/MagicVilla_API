using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DBSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //_db.villaNumbers.Include(u => u.Villa).ToList();
            this.DBSet=_db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await DBSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            DBSet.Remove(entity);
            await SaveAsync();

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> quary = DBSet;
            if (!tracked)
            {
                quary = quary.AsNoTracking();
            }
            if (filter != null)
            {
                quary = quary.Where(filter);
            }
            if(includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
                {
                    quary = quary.Include(includeProp);
                }
            }
            return await quary.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            int sizepage = 0 , int pageNumber = 1)
        {
            IQueryable<T> quary = DBSet;
            if (filter != null)
            {
                quary = quary.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    quary = quary.Include(includeProp);
                }
            }

            if (sizepage > 0 )
            {
                if(sizepage > 100)
                {
                    sizepage = 100;
                }
                quary = quary.Skip(sizepage * (pageNumber - 1)).Take(sizepage);

            }


            return await quary.ToListAsync();



        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
