using BoulderApp.GraphQL.DTO;
using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.InputTypes
{
    public class ProblemAttemptInputType : InputObjectGraphType<ProblemAttemptDto>
    {
        public ProblemAttemptInputType()
        {
            Name = "ProblemAttemptInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<IdGraphType>>("problemId");
            Field<NonNullGraphType<BooleanGraphType>>("sent");
        }
    }
}
