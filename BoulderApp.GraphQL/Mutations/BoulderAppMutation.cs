using BoulderApp.GraphQL.DTO;
using BoulderApp.GraphQL.InputTypes;
using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using BoulderApp.Web.Types;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoulderApp.GraphQL.Mutations
{
    public class BoulderAppMutation : ObjectGraphType
    {
        public BoulderAppMutation(BoulderAppRepository repository)
        {
            FieldAsync<CircuitType>(
                "createCircuit",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CircuitInputType>> { Name = "circuit" }),
                resolve: async context =>
                {
                    var circuit = context.GetArgument<Circuit>("circuit");
                    return await repository.CreateAsync(circuit);
                });

            FieldAsync<CenterType>(
                "createCenter",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CenterInputType>> { Name = "center" }),
                resolve: async context =>
                {
                    var center = context.GetArgument<Center>("center");
                    return await repository.CreateAsync(center);
                });

            FieldAsync<ProblemType>(
                "createProblem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ProblemInputType>> { Name = "problem" }),
                resolve: async context =>
                {
                    var problem = context.GetArgument<Problem>("problem");
                    return await repository.CreateAsync(problem);
                });

            FieldAsync<SessionType>(
                "createSession",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SessionInputType>> { Name = "input" }),
                resolve: async context =>
                {
                    var input = context.GetArgument<SessionDto>("input");
                    var session = new Session
                    {
                        Name = input.Name,
                        Id = input.Id,
                        Date = input.Date,
                        User = (await repository.GetAllAsync<User>()).Single(u => u.Id == input.UserId),
                        Center = (await repository.GetAllAsync<Center>()).Single(c => c.Id == input.CenterId),
                        ProblemAttempts =
                            (await repository.GetAllAsync<ProblemAttempt>())
                            .Where(p => input.ProblemAttemptIds.Contains(p.Id.Value))
                            .AsEnumerable() as ICollection<ProblemAttempt>
                    };
                    return await repository.CreateAsync(session);
                });

            FieldAsync<ProblemAttemptType>(
                "createProblemAttempt",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ProblemAttemptInputType>> { Name = "input" }),
                resolve: async context =>
                {
                    var input = context.GetArgument<ProblemAttemptDto>("input");
                    var attempt = new ProblemAttempt
                    {
                        Name = input.Name,
                        Id = input.Id,
                        Sent = input.Sent,
                        ProblemAttempted = (await repository.GetAllAsync<Problem>()).Single(p => p.Id == input.ProblemId)
                    };
                    return await repository.CreateAsync(attempt);
                });

            FieldAsync<DeleteResultGraphType>(
                "deleteProblem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    await repository.DeleteAsync<Problem>(context.GetArgument<Guid>("id"));
                    return new { result = "success" };
                });

            FieldAsync<CircuitType>(
                "addProblemToCircuit",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AddProblemToCircuitInput>> { Name = "input" }),
                resolve: async context =>
                {
                    var input = context.GetArgument<AddProblemToCircuitDto>("input");
                    var circuit = (await repository.GetAllAsync<Circuit>()).First(c => c.Id == input.CircuitId);
                    var problem = (await repository.GetAllAsync<Problem>()).First(p => p.Id == input.ProblemId);
                    circuit.Problems.Add(problem);

                    return await repository.UpdateAsync(circuit);
                });
            FieldAsync<CircuitType>(
                "createProblemInCircuit",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CreateProblemInCircuitInput>> { Name = "input" }),
                resolve: async context =>
                {
                    var input = context.GetArgument<CreateProblemInCircuitDto>("input");
                    var circuit = (await repository.GetAllAsync<Circuit>()).First(c => c.Id == input.CircuitId);

                    var problem = await repository.CreateAsync(input.Problem);
                    circuit.Problems.Add(problem);
                    return await repository.UpdateAsync(circuit);
                });
            FieldAsync<CenterType>(
                "createCircuitInCenter",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CreateCircuitInCenterInput>> { Name = "input" }),
                resolve: async context =>
                {
                    var input = context.GetArgument<CreateCircuitInCenterDto>("input");
                    var center = (await repository.GetAllAsync<Center>()).First(c => c.Id == input.CenterId);

                    var circuit = await repository.CreateAsync(input.Circuit);
                    center.Circuits.Add(circuit);
                    return await repository.UpdateAsync(center);
                });
        }
    }
}
