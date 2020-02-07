using BoulderApp.Model;
using System;

namespace BoulderApp.GraphQL.DTO
{
    public class CreateProblemInCircuitDto
    {
        public Guid CircuitId { get; set; }

        public Problem Problem { get; set; }
    }
}
