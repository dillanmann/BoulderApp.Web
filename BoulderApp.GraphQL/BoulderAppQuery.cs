using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using BoulderApp.Web.Types;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BoulderApp.GraphQL
{
    public class BoulderAppQuery : ObjectGraphType
    {        
        public BoulderAppQuery(BoulderAppContext repository)
        {
            Name = "SessionsQuery";

            FieldAsync<ListGraphType<SessionType>>(
                "sessions",
                resolve: async context => await repository.Sessions
                .Include(s => s.Center)
                .Include(s => s.ProblemAttempts)
                .ThenInclude(pa => pa.ProblemAttempted)
                .Include(s => s.User)
                .ToListAsync()
                );
            FieldAsync<ListGraphType<CircuitType>>(
                "circuits",
                resolve: async context => await repository.Circuits.Include(c => c.Problems).ToListAsync()
                );
            FieldAsync<ListGraphType<CenterType>>(
                "centers",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "id" }
                    ),
                resolve: async context => 
                {
                    var centers = repository.Centers.Include(c => c.Circuits).ThenInclude(circ => circ.Problems);
                    var id = context.GetArgument<Guid>("id");
                    if (id != default)
                        return await centers.Where(c => c.Id == id).ToListAsync();

                    return await repository.Centers.Include(c => c.Circuits).ThenInclude(circ => circ.Problems).ToListAsync();                    
                });
            FieldAsync<ListGraphType<ProblemType>>(
                "problems",
                resolve: async context => await repository.Problems.ToListAsync()
                );
            FieldAsync<ListGraphType<ProblemAttemptType>>(
                "problemAttempts",
                resolve: async context => await repository.ProblemAttempts.Include(p => p.ProblemAttempted).ToListAsync()
                );

            FieldAsync<ListGraphType<BoulderAppDataGraphType>>(
                "boulderAppData",
                arguments: new QueryArguments(
                    new QueryArgument<BoulderAppDataTypeEnumGraphType> { Name = "dataType" }
                    ),
                resolve: async context =>
                {
                    var dataType = context.GetArgument<BoulderAppDataType>("dataType");
                    switch (dataType)
                    {
                        case BoulderAppDataType.Center:
                            return await repository.Centers
                            .Include(c => c.Circuits)
                            .ThenInclude(c => c.Problems)
                            .ToListAsync();
                        case BoulderAppDataType.Circuit:
                            return await repository.Circuits
                            .Include(c => c.Problems)
                            .ToListAsync();
                        case BoulderAppDataType.ProblemAttempt:
                            return await repository.ProblemAttempts.ToListAsync();
                        case BoulderAppDataType.Problem:
                            return await repository.Problems.ToListAsync();
                        case BoulderAppDataType.Session:
                            return await repository.Sessions
                            .Include(c => c.Center.Circuits)
                            .ThenInclude(c => c.Problems)
                            .ToListAsync();
                        case BoulderAppDataType.User:
                            return repository.Users;
                        default:
                            throw new InvalidOperationException("Failed to fetch data for unrecognised data type");
                    }
                });
        }
    }
}
