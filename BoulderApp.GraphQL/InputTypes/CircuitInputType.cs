using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.InputTypes
{
    public class CircuitInputType : InputObjectGraphType<Circuit>
    {
        public CircuitInputType()
        {
            Name = "CircuitInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<DateGraphType>>("dateUp");
            Field<NonNullGraphType<DateGraphType>>("dateDown");
            Field<ListGraphType<ProblemInputType>>("problems");
        }
    }
}