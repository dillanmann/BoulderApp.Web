using BoulderApp.GraphQL.DTO;
using GraphQL.Types;

namespace BoulderApp.GraphQL.InputTypes
{
    public class SessionInputType : InputObjectGraphType<SessionDto>
    {
        public SessionInputType()
        {
            Name = "SessionInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<DateGraphType>>("date");
            Field<NonNullGraphType<IdGraphType>>("centerId");
            Field<NonNullGraphType<IdGraphType>>("userId");
            Field<ListGraphType<IdGraphType>>("problemAttemptIds");
        }
    }
}
