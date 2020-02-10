using BoulderApp.Model;
using System;

namespace BoulderApp.GraphQL.DTO
{
    public class CreateCircuitInCenterDto
    {
        public Guid CenterId { get; set; }

        public Circuit Circuit { get; set; }
    }
}
