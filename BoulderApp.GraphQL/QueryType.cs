using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using BoulderApp.Web.Types;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;

namespace BoulderApp.GraphQL
{
    public class QueryType : ObjectGraphType
    {        
        public QueryType(BoulderAppContext repository)
        {
            Name = "SessionsQuery";

            FieldAsync<ListGraphType<SessionType>>(
                "sessions",
                resolve: async context => await repository.Sessions.ToListAsync()
                );
            FieldAsync<ListGraphType<CircuitType>>(
                "circuits",
                resolve: async context =>
                {
                    return await repository.Circuits.Include(c => c.Problems).ToListAsync();
                });

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
