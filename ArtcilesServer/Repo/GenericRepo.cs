using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArtcilesServer.Repo
{
  
        public class GenericRepository<T> : IGenericRepository<T> where T : class
        {
            private readonly DbConn _context;
            private readonly DbSet<T> _dbSet;

            public GenericRepository(DbConn context)
            {
                _context = context;
                _dbSet = context.Set<T>();
            }


            public async Task AddAsync(T entity)
            {
                await _dbSet.AddAsync(entity);
                await SaveAsync();
            }


            public async Task Delete(T entity)
            {
                _dbSet.Remove(entity);
                await SaveAsync();
            }



            public async Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.ToListAsync();
            }

            public async Task<T?> GetByIdAsync(int id)
            {
                return await _dbSet.FindAsync(id);
            }
          

           

            public async Task SaveAsync()
            {
                await _context.SaveChangesAsync();
            }


            public async Task Update(T entity)
            {
                _dbSet.Update(entity);
                await SaveAsync();
            }


        }
    }

