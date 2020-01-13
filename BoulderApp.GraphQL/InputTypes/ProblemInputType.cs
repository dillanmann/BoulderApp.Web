using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.InputTypes
{
    public class ProblemInputType : InputObjectGraphType<Problem>
    {
        public ProblemInputType()
        {
            Name = "ProblemInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<VGradeType>>("grade");
        }
    }
}
