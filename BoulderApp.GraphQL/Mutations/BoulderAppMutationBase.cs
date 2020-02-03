using BoulderApp.Model;
using BoulderApp.Web.Types;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BoulderApp.GraphQL.Mutations
{
    public abstract class BoulderAppMutationBase : ObjectGraphType
    {
        protected BoulderAppMutationBase(BoulderAppContext context)
        {
            this.DbContext = context;
        }

        protected BoulderAppContext DbContext { get; }

        public async Task<T> Create<T>(T item)
            where T : BoulderAppData
        {
            if (item.Id == default)
            {
                item.Id = Guid.NewGuid();
            }

            DbContext.Add(item);
            await DbContext.SaveChangesAsync();

            return item;
        }

        public async Task<T> Update<T>(T item)
            where T : BoulderAppData
        {
            if (!Exists<T>(item.Id.Value))
            {
                throw new InvalidOperationException(
                    $"Can't update item of type `{typeof(T).Name}` as it doesn't exist. ");
            }

            var set = DbContext.Set<T>();
            var existingItem = set.Find(item.Id.Value);
            DbContext.Entry(existingItem).CurrentValues.SetValues(item);

            var rows = await DbContext.SaveChangesAsync();

            return await set.SingleAsync(i => i.Id == item.Id);
        }

        public bool Exists<T>(Guid id)
            where T : BoulderAppData => DbContext.Find(typeof(T), id) != null;

        public async Task Delete<T>(Guid id)
            where T : BoulderAppData
        {
            if (!Exists<T>(id))
            {
                throw new InvalidOperationException(
                    $"Can't delete item of type `{typeof(T).Name}` as it doesn't exist. ");
            }

            var set = DbContext.Set<T>();
            var existingItem = set.Find(id);

            set.Remove(existingItem);

            var rows = await DbContext.SaveChangesAsync();
        }
    }
}
