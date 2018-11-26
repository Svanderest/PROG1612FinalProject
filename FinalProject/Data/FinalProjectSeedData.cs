using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.Data
{
    public static class FinalProjectSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FinalProjectContext(serviceProvider.GetRequiredService<DbContextOptions<FinalProjectContext>>()))
            {
                if(!context.Position.Any())
                {
                    context.Position.AddRange
                        (
                         new Position { Title = "Position 1" },
                         new Position { Title = "Position 2" },
                         new Position { Title = "Position 3"}
                        );
                }
                if(!context.Assignment.Any())
                {
                    context.Assignment.AddRange
                        (
                         new Assignment { Name = "Assignment 1"},
                         new Assignment { Name = "Assignment 2"},
                         new Assignment { Name = "Assignment 3"}
                        );
                }
                context.SaveChanges();
                if (!context.Member.Any())
                {
                    context.Member.AddRange
                        (
                         new Member
                         {
                             FirstName = "John",
                             LastName = "Doe",
                             eMail = "john@doe",
                             Phone = 1234567890,
                             AssignmentID = context.Assignment.FirstOrDefault(a => a.Name.Contains("1")).ID
                         },
                         new Member
                         {
                             FirstName = "Jane",
                             LastName = "Doe",
                             eMail = "jane@doe",
                             Phone = 1234867890,
                             AssignmentID = context.Assignment.FirstOrDefault(a => a.Name.Contains("2")).ID
                         },
                         new Member
                         {
                             FirstName = "Jane",
                             LastName = "Smith",
                             eMail = "jane@smith",
                             Phone = 1234867894,
                             AssignmentID = context.Assignment.FirstOrDefault(a => a.Name.Contains("3")).ID
                         }
                        );
                }
                context.SaveChanges();
                if (!context.Shift.Any())
                {
                    Random random = new Random();
                    context.Shift.AddRange
                        (
                         new Shift
                         {
                             Date = new DateTime(random.Next(int.MinValue,int.MaxValue)),
                             AssignmentID = context.Assignment.FirstOrDefault(a => a.Name.Contains("1")).ID,
                             MemberID = context.Member.FirstOrDefault(m => m.AssignmentID == context.Assignment.FirstOrDefault(a => a.Name.Contains("1")).ID).ID
                         },
                         new Shift
                         {
                             Date = new DateTime(random.Next(int.MinValue, int.MaxValue)),
                             AssignmentID = context.Assignment.FirstOrDefault(a => a.Name.Contains("2")).ID,
                             MemberID = context.Member.FirstOrDefault(m => m.AssignmentID == context.Assignment.FirstOrDefault(a => a.Name.Contains("2")).ID).ID
                         },
                         new Shift
                         {
                             Date = new DateTime(random.Next(int.MinValue, int.MaxValue)),
                             AssignmentID = context.Assignment.FirstOrDefault(a => a.Name.Contains("1")).ID,
                             MemberID = context.Member.FirstOrDefault(m => m.AssignmentID == context.Assignment.FirstOrDefault(a => a.Name.Contains("2")).ID).ID
                         }
                        );
                }
                context.SaveChanges();
                if(!context.MemberPosition.Any())
                {
                    context.MemberPosition.AddRange
                        (
                            new MemberPosition
                            {
                                MemberID = context.Member.FirstOrDefault(m => m.ID is int).ID,
                                PositionID = context.Position.FirstOrDefault(p => p.ID is int).ID
                            },
                            new MemberPosition
                            {
                                MemberID = context.Member.LastOrDefault(m => m.ID is int).ID,
                                PositionID = context.Position.LastOrDefault(p => p.ID is int).ID
                            }
                        );
                }
            }
        }
    }
}