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
            Query = resolver.Resolve<BoulderAppQuery>();
            Mutation = resolver.Resolve<BoulderAppMutation>();
        }
    }
}
