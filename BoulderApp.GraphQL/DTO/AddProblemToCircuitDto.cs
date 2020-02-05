using System;

namespace BoulderApp.GraphQL.DTO
{
    public class AddProblemToCircuitDto
    {
        public Guid CircuitId { get; set; }

        public Guid ProblemId { get; set; }
    }
}
