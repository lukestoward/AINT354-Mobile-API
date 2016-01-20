using System.Collections.Generic;
using AINT354_Mobile_API.Models;

namespace AINT354_Mobile_API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        
        protected override void Seed(ApplicationDbContext context)
        {
            context.Colours.AddOrUpdate(p => p.ColourName,
                new Colour { ColourName = "Red" },
                new Colour { ColourName = "Green" },
                new Colour { ColourName = "Blue" },
                new Colour { ColourName = "Orange" }
            );
            context.SaveChanges();

            context.Users.AddOrUpdate(p => p.Email,
                new User { Id = 1, Name = "Luke Stoward", Email = "luke@email.com", DeviceId = "34jk2kjh2" , FacebookId = 123},
                new User { Id = 2, Name = "Thomas Kraaijeveld", Email = "thomaskraaijeveld@hotmail.com", DeviceId = "654asfsf5", FacebookId = 4235564436}
            );
            context.SaveChanges();

            if (!context.Calendars.Any())
            {
                Guid calendarId = Guid.NewGuid();

                Calendar cal = new Calendar { Id = calendarId, Name = "Home", ColourId = 1, Description = "My calendar for home", OwnerId = 1 };
                CalendarMember calMember = new CalendarMember { UserId = 1 };

                cal.Members.Add(calMember);

                context.Calendars.AddOrUpdate(p => p.Id, cal);

                context.SaveChanges();

                //Create event data

                Event event1 = new Event { Id = Guid.NewGuid(), CreatorId = 1, CreatedDate = DateTime.Now, Title = "My First Event", Body = "This is the body text",
                    Location = "My House", AllDay = false, StartDateTime = DateTime.Now.AddDays(1), EndDateTime = DateTime.Now.AddDays(1).AddHours(2), ColourId = 1 };

                Event event2 = new Event { Id = Guid.NewGuid(), CreatorId = 1, CreatedDate = DateTime.Now.AddHours(1), Title = "My Second Event", Body = "This is the body text",
                    Location = "Uni", AllDay = false, StartDateTime = DateTime.Now.AddHours(2), EndDateTime = DateTime.Now.AddHours(2).AddMinutes(30), ColourId = 1
                };

                Event event3 = new Event { Id = Guid.NewGuid(), CreatorId = 1, CreatedDate = DateTime.Now.AddDays(2), Title = "My Third Event", Body = "This is the body text",
                    Location = "4 Allendale Road", AllDay = false, StartDateTime = DateTime.Now.AddDays(3), EndDateTime = DateTime.Now.AddDays(3).AddHours(4), ColourId = 1
                };

                //Attach the events to the seeded calendar
                event1.Calendars.Add(cal);
                event2.Calendars.Add(cal);
                event3.Calendars.Add(cal);

                //Attach the user to the seeded events
                event1.Members.Add(new EventMember { UserId = 1 });
                event2.Members.Add(new EventMember { UserId = 1 });
                event3.Members.Add(new EventMember { UserId = 1 });
                
                //Add sample events
                context.Events.AddOrUpdate(p => p.StartDateTime, event1, event2, event3);

                context.SaveChanges();

            }

            context.InvitationTypes.AddOrUpdate(x => x.Name, 
                new InvitationType { Name = "Calendar" },
                new InvitationType { Name = "Event" } );

            context.SaveChanges();
            
        }
    }
}
