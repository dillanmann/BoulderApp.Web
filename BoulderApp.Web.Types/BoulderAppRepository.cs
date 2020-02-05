using BoulderApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoulderApp.Web.Types
{
    public class BoulderAppRepository
    {
        private readonly BoulderAppContext _context;

        public BoulderAppRepository(BoulderAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            where T : BoulderAppData
        {
            return await AddIncludes(_context.Set<T>()).ToListAsync();
        }

        public async Task<T> GetItemByIdAsync<T>(Guid id)
            where T : BoulderAppData
        {
            return await AddIncludes(_context.Set<T>()).FirstAsync(s => s.Id == id);
        }

        private IQueryable<T> AddIncludes<T>(DbSet<T> items)
            where T : BoulderAppData
        {
            if (typeof(T) == typeof(Session))
            {
                return AddSessionIncludes(items as DbSet<Session>) as IQueryable<T>;
            }
            else if (typeof(T) == typeof(Circuit))
            {
                return AddCircuitIncludes(items as DbSet<Circuit>) as IQueryable<T>;
            }
            else if (typeof(T) == typeof(Center))
            {
                return AddCenterIncludes(items as DbSet<Center>) as IQueryable<T>;
            }
            else if (typeof(T) == typeof(ProblemAttempt))
            {
                return AddProblemAttemptIncludes(items as DbSet<ProblemAttempt>) as IQueryable<T>;
            }
            else if (typeof(T) == typeof(Problem))
            {
                return AddProblemIncludes(items as DbSet<Problem>) as IQueryable<T>;
            }

            throw new InvalidOperationException($"No EFCore includes registered for type {typeof(T)}");
        }

        public async Task<T> CreateAsync<T>(T item)
            where T : BoulderAppData
        {
            if (item.Id == default)
            {
                item.Id = Guid.NewGuid();
            }

            _context.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<T> UpdateAsync<T>(T item)
            where T : BoulderAppData
        {
            if (!Exists<T>(item.Id.Value))
            {
                throw new InvalidOperationException(
                    $"Can't update item of type `{typeof(T).Name}` as it doesn't exist. ");
            }

            var set = _context.Set<T>();
            var existingItem = set.Find(item.Id.Value);
            _context.Entry(existingItem).CurrentValues.SetValues(item);

            var rows = await _context.SaveChangesAsync();

            return await set.SingleAsync(i => i.Id == item.Id);
        }

        public bool Exists<T>(Guid id)
            where T : BoulderAppData => _context.Find(typeof(T), id) != null;

        public async Task DeleteAsync<T>(Guid id)
            where T : BoulderAppData
        {
            if (!Exists<T>(id))
            {
                throw new InvalidOperationException(
                    $"Can't delete item of type `{typeof(T).Name}` as it doesn't exist. ");
            }

            var set = _context.Set<T>();
            var existingItem = set.Find(id);

            set.Remove(existingItem);

            var rows = await _context.SaveChangesAsync();
        }

        private IQueryable<Session> AddSessionIncludes(DbSet<Session> sessions) =>
            sessions.Include(s => s.Center)
                .Include(s => s.ProblemAttempts)
                .ThenInclude(pa => pa.ProblemAttempted)
                .Include(s => s.User);

        private IQueryable<Circuit> AddCircuitIncludes(DbSet<Circuit> circuits)
            => circuits.Include(c => c.Problems);

        private IQueryable<Center> AddCenterIncludes(DbSet<Center> centers)
            => centers.Include(c => c.Circuits).ThenInclude(circ => circ.Problems);

        private IQueryable<ProblemAttempt> AddProblemAttemptIncludes(DbSet<ProblemAttempt> attempts)
            => attempts.Include(p => p.ProblemAttempted);

        private IQueryable<Problem> AddProblemIncludes(DbSet<Problem> problems)
            => problems.AsQueryable();
    }
}
