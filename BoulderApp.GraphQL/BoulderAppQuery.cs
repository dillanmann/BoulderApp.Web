using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using BoulderApp.Web.Types;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoulderApp.GraphQL
{
    public class BoulderAppQuery : ObjectGraphType
    {
        public BoulderAppQuery(BoulderAppContext repository)
        {
            Name = "BoulderAppQueries";

            FieldAsync<ListGraphType<SessionType>>(
                "sessions",
                resolve: async context => await AddIncludes(repository.Sessions).ToListAsync()
                );
            FieldAsync<ListGraphType<CircuitType>>(
                "circuits",
                resolve: async context => await AddIncludes(repository.Circuits).ToListAsync()
                );
            FieldAsync<ListGraphType<CenterType>>(
                "centers",
                resolve: async context => await AddIncludes(repository.Centers).ToListAsync());
            FieldAsync<ListGraphType<ProblemType>>(
                "problems",
                resolve: async context => await AddIncludes(repository.Problems).ToListAsync()
                );
            FieldAsync<ListGraphType<ProblemAttemptType>>(
                "problemAttempts",
                resolve: async context => await AddIncludes(repository.ProblemAttempts).ToListAsync()
                );
            FieldAsync<SessionType>(
            "session",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await GetItemById(context, repository.Sessions));
            FieldAsync<CenterType>(
            "center",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await GetItemById(context, repository.Centers));
            FieldAsync<CircuitType>(
            "circuit",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await GetItemById(context, repository.Circuits));
            FieldAsync<ProblemType>(
            "problem",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await GetItemById(context, repository.Problems));
            FieldAsync<ProblemAttemptType>(
            "problemAttempt",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await GetItemById(context, repository.ProblemAttempts));
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

        private async Task<T> GetItemById<T>(ResolveFieldContext<object> context, DbSet<T> items) 
            where T : BoulderAppData
        {
            var id = context.GetArgument<Guid>("id");
            return await AddIncludes(items).FirstAsync(s => s.Id == id);
        }
    }
}
