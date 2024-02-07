using eLearn360.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class Repository<TKey, T> : IRepository<TKey, T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task Delete(TKey id, T entity)
        {
            //T tmpEntity = await _entities.AsNoTracking().FindAsync(id);
            if (entity == null)
            {
                throw new Exception("Entity not found");
            }
            try
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> Get()
        {
            return await _entities.ToListAsync();
        }

        public async virtual Task<T> GetById(TKey id)
        {
            return await _entities.FindAsync(id);
        }

        public async virtual Task<T> Post(T entity)
        {
            try
            {
                var state = await _entities.AddAsync(entity);
                var res = await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }



        public async Task Put(TKey id, T entity)
        {
            try
            {
                _entities.Update(entity);
                _context.Entry(entity).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }

        private bool AlreadyExists(TKey id)
        {
            return _entities.Find(id) != null;
        }
    }
}
