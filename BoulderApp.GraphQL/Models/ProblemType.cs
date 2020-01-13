using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class ProblemType : BoulderAppDataType<Problem>
    {
        public ProblemType()
            : base()
        {
            this.Field(x => x.Grade, type: typeof(VGradeType)).Description("Grade of the problem");
        }
    }
}