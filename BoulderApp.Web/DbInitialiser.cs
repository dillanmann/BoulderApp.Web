using BoulderApp.Model;
using BoulderApp.Web.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoulderApp.Web
{
    public static class DbInitialiser
    {
        public static async Task InitialiseAsync(BoulderAppContext context)
        {
            context.Database.EnsureCreated();

            if (context.Centers.Any())
            {
                return;
            }

            var problems = new List<Problem>();
            for(var i = 0; i < 20; ++i)
            {
                problems.Add(new Problem
                {
                    Id = Guid.NewGuid(),
                    Name = $"Orange {i}",
                    Grade = (VGrade)new Random().Next((int)VGrade.V0, (int)VGrade.V2)
                });
            }
            context.Problems.AddRange(problems);
            await context.SaveChangesAsync();

            var circuits = new[]
            {
                new Circuit
                {
                    Id = Guid.NewGuid(),
                    Name = "Orange",
                    DateUp = new DateTime(2020, 01, 01),
                    DateDown = new DateTime(2020, 02, 01),
                    Problems = problems
                }
            };
            context.Circuits.AddRange(circuits);
            await context.SaveChangesAsync();

            var center = new Center
            {
                Id = Guid.NewGuid(),
                Name = "Eden Rock",
                Circuits = circuits
            };
            context.Centers.Add(center);
            await context.SaveChangesAsync();
        }
    }
}
