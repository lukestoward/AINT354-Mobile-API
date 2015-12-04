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
                new User { Id = 1, Name = "Luke Stoward", Email = "luke@email.com"}
            );
            context.SaveChanges();

            context.Calendars.AddOrUpdate(p => p.Name,
                new Calendar { Name = "Home", ColourId = 1, Description = "My calendar for home", OwnerId = 1 }
              
            );
        }
    }
}
