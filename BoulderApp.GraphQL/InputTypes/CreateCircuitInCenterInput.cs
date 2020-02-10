using GraphQL.Types;

namespace BoulderApp.GraphQL.InputTypes
{
    public class CreateCircuitInCenterInput : InputObjectGraphType
    {
        public CreateCircuitInCenterInput()
        {
            Field<IdGraphType>("centerId");
            Field<CircuitInputType>("circuit");
        }
    }
}
