using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class SessionType : BoulderAppDataType<Session>
    {
        public SessionType()
            : base()
        {
            this.Field(x => x.Center, type: typeof(CenterType)).Description("The center the session was performed at");
            this.Field(x => x.Date, type: typeof(DateTimeGraphType)).Description("The date of the session");
            this.Field(x => x.ProblemAttempts, type: typeof(ListGraphType<ProblemAttemptType>)).Description("The problem attempts of the session");
            this.Field(x => x.User, type: typeof(ListGraphType<UserType>)).Description("The user attempting the session");
        }
    }
}
