using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class VGradeType : EnumerationGraphType<VGrade>
    {
        public VGradeType()
            : base()
        {
        }
    }
}
