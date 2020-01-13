using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class BoulderAppDataType<T> : ObjectGraphType<T>
        where T : BoulderAppData
    {
        public BoulderAppDataType()
        {
            this.Field(x => x.Id, type: typeof(IdGraphType)).Description("Id of the item");
            this.Field(x => x.Name, type: typeof(StringGraphType)).Description("The name of the item");
            this.Interface<BoulderAppDataGraphType>();
        }
    }
}
