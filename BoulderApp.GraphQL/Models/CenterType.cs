using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class CenterType : BoulderAppDataType<Center>
    {
        public CenterType()
            : base()
        {
            this.Field(x => x.Circuits, type: typeof(ListGraphType<CircuitType>)).Description("The circuits in the center");
        }
    }
}
