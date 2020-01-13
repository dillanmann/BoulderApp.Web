using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class ProblemAttemptType : BoulderAppDataType<ProblemAttempt>
    {
        public ProblemAttemptType()
            : base()
        {
            this.Field(x => x.ProblemAttempted, type: typeof(ProblemType)).Description("The problem attempted");
            this.Field(x => x.Sent, type: typeof(BooleanGraphType)).Description("Whether the attempt was succesful or not");
        }
    }
}