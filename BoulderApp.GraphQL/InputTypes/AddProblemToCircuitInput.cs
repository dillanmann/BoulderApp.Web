using GraphQL.Types;

namespace BoulderApp.GraphQL.InputTypes
{
    public class AddProblemToCircuitInput : InputObjectGraphType
    {
        public AddProblemToCircuitInput()
        {
            Field<NonNullGraphType<IdGraphType>>("circuitId");
            Field<NonNullGraphType<IdGraphType>>("problemId");
        }
    }
}
