using GraphQL.Types;

namespace BoulderApp.GraphQL.Mutations
{
    public class DeleteResultGraphType : ObjectGraphType
    {
        public DeleteResultGraphType()
        {
            Field(typeof(StringGraphType), "result");
        }
    }
}
