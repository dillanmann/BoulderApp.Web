using BoulderApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoulderApp.GraphQL.DTO
{
    public class ProblemAttemptDto : BoulderAppData
    {
        public bool Sent { get; set; }

        public Guid ProblemId { get; set; }
    }
}
