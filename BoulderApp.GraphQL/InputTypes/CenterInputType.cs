using BoulderApp.GraphQL.Models;
using BoulderApp.Model;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoulderApp.GraphQL.InputTypes
{
    public class CenterInputType : InputObjectGraphType<Center>
    {
        public CenterInputType()
        {
            Name = "CenterInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<ListGraphType<CircuitInputType>>("circuits");
        }
    }
}
