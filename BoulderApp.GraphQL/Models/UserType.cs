using BoulderApp.Model;
using GraphQL.Types;

namespace BoulderApp.GraphQL.Models
{
    public class UserType : BoulderAppDataType<User>
    {
        public UserType()
            : base()
        {
            this.Field(x => x.Email, type: typeof(StringGraphType)).Description("The email of the user");
        }
    }
}