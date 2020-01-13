using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class BoulderAppDataGraphType : InterfaceGraphType<BoulderAppData>
    {
        public BoulderAppDataGraphType()
        {
            Name = "BoulderAppData";
            Field(e => e.Id, type: typeof(IdGraphType)).Description("Id of the item");
            Field(e => e.Name, type: typeof(StringGraphType)).Description("Name of the item");
        }
    }
}
