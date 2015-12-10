using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using AINT354_Mobile_API.Models;

public class ApplicationDbContext : DbContext
{
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, please use data migrations.
    // For more information refer to the documentation:
    // http://msdn.microsoft.com/en-us/data/jj591621.aspx

    public ApplicationDbContext() : base("DefaultConnection")
    {
    }
    
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        //one-to-many for Session => SessionMessage
        //modelBuilder.Entity<Event>()
        //    .HasRequired(s => s.Creator)
        //    .WithMany(s => s.Events)
        //    .HasForeignKey(s => s.CreatorId)
        //    .WillCascadeOnDelete(false);

        //Calendar & Event many-to-many mapping
        modelBuilder.Entity<Event>()
               .HasMany<Calendar>(s => s.Calendars)
               .WithMany(c => c.Events)
               .Map(cs =>
               {
                   cs.MapLeftKey("EventId");
                   cs.MapRightKey("CalendarId");
                   cs.ToTable("EventCalendars");
               });


        modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Calendar> Calendars { get; set; }

    public DbSet<CalendarMember> CalendarMembers { get; set; }
    
    public DbSet<Colour> Colours { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<EventMember> EventMembers { get; set; }
    
    public DbSet<EventComment> EventComments { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Invitation> Invitations { get; set; }

    public DbSet<InvitationType> InvitationTypes { get; set; }

}
