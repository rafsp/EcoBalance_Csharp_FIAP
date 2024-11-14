using EcoBalance.Database;
using EcoBalance.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EcoBalance.Repository
{
    public class OracleRepository<T> : IRepository<T> where T : class
    {
        private readonly OracleDbContext _context;

        private readonly DbSet<T> _dbSet;

        public OracleRepository(OracleDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _context.AddAsync(entity);

            _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);

            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int? id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
