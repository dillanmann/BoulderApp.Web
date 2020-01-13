using BoulderApp.GraphQL.Mutations;
using GraphQL;
using GraphQL.Types;

namespace BoulderApp.GraphQL
{
    public class BoulderAppSchema : Schema
    {
        public BoulderAppSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<QueryType>();
            Mutation = resolver.Resolve<BoulderAppMutation>();
        }
    }
}
