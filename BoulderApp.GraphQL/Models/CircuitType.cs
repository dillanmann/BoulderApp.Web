using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class CircuitType : BoulderAppDataType<Circuit>
    {
        public CircuitType()
            : base()
        {
            this.Field(x => x.DateUp, type: typeof(DateTimeGraphType)).Description("Date the circuit went up");
            this.Field(x => x.DateDown, type: typeof(DateTimeGraphType)).Description("Date the circuit went down or will go down");
            this.Field(x => x.Problems, type: typeof(ListGraphType<ProblemType>)).Description("Problems within the circuit");
        }
    }
}