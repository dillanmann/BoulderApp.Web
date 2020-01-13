using BoulderApp.Model;
using System;
using System.Collections.Generic;

namespace BoulderApp.GraphQL.DTO
{
    public class SessionDto : BoulderAppData
    {
        public DateTime Date { get; set; }
        
        public Guid UserId { get; set; }

        public Guid CenterId { get; set; }

        public IEnumerable<Guid> ProblemAttemptIds { get; set; }
    }
}
