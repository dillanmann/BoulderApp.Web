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
                resolve: async context => await AddSessionIncludes(repository.Sessions).ToListAsync()
                );
            FieldAsync<ListGraphType<CircuitType>>(
                "circuits",
                resolve: async context => await AddCircuitIncludes(repository.Circuits).ToListAsync()
                );
            FieldAsync<ListGraphType<CenterType>>(
                "centers",
                resolve: async context => await AddCenterIncludes(repository.Centers).ToListAsync());
            FieldAsync<ListGraphType<ProblemType>>(
                "problems",
                resolve: async context => await repository.Problems.ToListAsync()
                );
            FieldAsync<ListGraphType<ProblemAttemptType>>(
                "problemAttempts",
                resolve: async context => await AddProblemAttemptIncludes(repository.ProblemAttempts).ToListAsync()
                );
        }

        private IIncludableQueryable<Session, User> AddSessionIncludes(DbSet<Session> sessions) =>
            sessions.Include(s => s.Center)
                .Include(s => s.ProblemAttempts)
                .ThenInclude(pa => pa.ProblemAttempted)
                .Include(s => s.User);

        private IIncludableQueryable<Circuit, ICollection<Problem>> AddCircuitIncludes(DbSet<Circuit> circuits)
            => circuits.Include(c => c.Problems);

        private IIncludableQueryable<Center, ICollection<Problem>> AddCenterIncludes(DbSet<Center> centers)
            => centers.Include(c => c.Circuits).ThenInclude(circ => circ.Problems);

        private IIncludableQueryable<ProblemAttempt, Problem> AddProblemAttemptIncludes(DbSet<ProblemAttempt> problems)
            => problems.Include(p => p.ProblemAttempted);
    }
}
