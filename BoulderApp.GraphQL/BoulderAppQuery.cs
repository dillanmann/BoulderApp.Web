using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using BoulderApp.Web.Types;
using GraphQL.Types;
using System;

namespace BoulderApp.GraphQL
{
    public class BoulderAppQuery : ObjectGraphType
    {
        public BoulderAppQuery(BoulderAppRepository repository)
        {
            Name = "BoulderAppQueries";

            FieldAsync<ListGraphType<SessionType>>(
                "sessions",
                resolve: async context => await repository.GetAllAsync<Session>()
                );
            FieldAsync<ListGraphType<CircuitType>>(
                "circuits",
                resolve: async context => await repository.GetAllAsync<Circuit>()
                );
            FieldAsync<ListGraphType<CenterType>>(
                "centers",
                resolve: async context => await repository.GetAllAsync<Center>()
                );
            FieldAsync<ListGraphType<ProblemType>>(
                "problems",
                resolve: async context => await repository.GetAllAsync<Problem>()
                );
            FieldAsync<ListGraphType<ProblemAttemptType>>(
                "problemAttempts",
                resolve: async context => await repository.GetAllAsync<ProblemAttempt>()
                );
            FieldAsync<SessionType>(
            "session",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await repository.GetItemByIdAsync<Session>(context.GetArgument<Guid>("id")));
            FieldAsync<CenterType>(
            "center",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await repository.GetItemByIdAsync<Center>(context.GetArgument<Guid>("id")));
            FieldAsync<CircuitType>(
            "circuit",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await repository.GetItemByIdAsync<Circuit>(context.GetArgument<Guid>("id")));
            FieldAsync<ProblemType>(
            "problem",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await repository.GetItemByIdAsync<Problem>(context.GetArgument<Guid>("id")));
            FieldAsync<ProblemAttemptType>(
            "problemAttempt",
            arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id" }),
            resolve: async context => await repository.GetItemByIdAsync<ProblemAttempt>(context.GetArgument<Guid>("id")));
        }        
    }
}
