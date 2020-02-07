using GraphQL.Types;

namespace BoulderApp.GraphQL.InputTypes
{
    public class CreateProblemInCircuitInput : InputObjectGraphType
    {
        public CreateProblemInCircuitInput()
        {
            Field<IdGraphType>("circuitId");
            Field<ProblemInputType>("problem");
        }
    }
}
