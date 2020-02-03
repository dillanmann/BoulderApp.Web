using BoulderApp.GraphQL.DTO;
using BoulderApp.GraphQL.InputTypes;
using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using BoulderApp.Web.Types;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoulderApp.GraphQL.Mutations
{
    public class BoulderAppMutation : BoulderAppMutationBase
    {
        public BoulderAppMutation(BoulderAppContext dbContext) 
            : base(dbContext)
        {
            FieldAsync<CircuitType>(
                "createCircuit",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CircuitInputType>> { Name = "circuit" }),
                resolve: async context =>
                {
                    var center = context.GetArgument<Circuit>("circuit");
                    return await this.Create(center);
                });

            FieldAsync<CenterType>(
                "createCenter",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CenterInputType>> { Name = "center" }),
                resolve: async context =>
                {
                    var center = context.GetArgument<Center>("center");
                    return await this.Create(center);
                });

            FieldAsync<ProblemType>(
                "createProblem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ProblemInputType>> { Name = "problem" }),
                resolve: async context =>
                {
                    var problem = context.GetArgument<Problem>("problem");
                    return await this.Create(problem);
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
                        User = this.DbContext.Users.Single(u => u.Id == input.UserId),
                        Center = this.DbContext.Centers.Single(c => c.Id == input.CenterId),
                        ProblemAttempts = 
                            this.DbContext.ProblemAttempts.Where(p => input.ProblemAttemptIds.Contains(p.Id.Value)).AsEnumerable() as ICollection<ProblemAttempt>
                    };
                    return await this.Create(session);
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
                        ProblemAttempted = this.DbContext.Problems.Single(p => p.Id == input.ProblemId)
                    };
                    return await this.Create(attempt);
                });

            FieldAsync<DeleteResultGraphType>(
                "deleteProblem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    await DeleteItem<Problem>(context);
                    return new { result = "success" };
                });
        }

        private async Task DeleteItem<T>(ResolveFieldContext<object> context)
            where T : BoulderAppData
        {
            var id = context.GetArgument<Guid>("id");
            await Delete<T>(id);
        }
    }
}
