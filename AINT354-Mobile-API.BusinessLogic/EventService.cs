using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AINT354_Mobile_API.ModelDTOs;
using AINT354_Mobile_API.Models;
using GamingSessionApp.DataAccess;

namespace AINT354_Mobile_API.BusinessLogic
{
    public class EventService : BaseLogic
    {
        //Session Repository
        private readonly GenericRepository<Event> _eventRepo;

        public EventService()
        {
            _eventRepo = UoW.Repository<Event>();
        }

        public async Task<List<EventDTO>> GetCalendarEvents(string calId)
        {
            //Parse guid
            Guid? guid = ParseGuid(calId);
            if (guid == null) return null;

            var events = await _eventRepo.Get(x => x.CalendarId == guid.Value)
                .Select(x => new EventDTO
                {
                    Id = x.Id.ToString(),
                    CalendarId = x.CalendarId.ToString(),
                    Title = x.Title,
                    Location = x.Location,
                    StartDateTime = x.StartDateTime.ToString(),
                    EndDateTime = x.EndDateTime.ToString(),
                    AllDay = x.AllDay
                }).ToListAsync();

            foreach (var e in events)
            {
                e.StartDateTime = FormatDateString(e.StartDateTime);
                e.EndDateTime = FormatDateString(e.EndDateTime);
            }

            return events;
        }

        public async Task<EventDetailsDTO> GetEventsDetails(string evId)
        {
            //Parse guid
            Guid? guid = ParseGuid(evId);
            if (guid == null) return null;

            var eventDetails = await _eventRepo.Get(x => x.Id == guid.Value)
                .Select(x => new EventDetailsDTO
                {
                    Id = x.Id.ToString(),
                    CalendarId = x.CalendarId.ToString(),
                    CreatorName = x.Creator.Name,
                    CreatedDate = x.CreatedDate,
                    Title = x.Title,
                    Body = x.Body,
                    Location = x.Location,
                    AllDay = x.AllDay,
                    StartDateTime = x.StartDateTime.ToString(),
                    EndDateTime = x.EndDateTime.ToString(),
                }).FirstOrDefaultAsync();

            //Convert the dates in to specific formats
            eventDetails.StartDateTime = FormatDateString(eventDetails.StartDateTime);
            eventDetails.EndDateTime = FormatDateString(eventDetails.EndDateTime);

            return eventDetails;
        }

        public async Task<bool> CreateEvent(EventCreateDTO model)
        {
            try
            {
                //Parse guid(s)
                Guid? id = ParseGuid(model.Id);
                if (id == null) return false;

                Guid? calId = ParseGuid(model.CalendarId);
                if (calId == null) return false;

                Event newEvent = new Event
                {
                    Id = id.Value,
                    CalendarId = calId.Value,
                    CreatorId = model.CreatorId,
                    Title = model.Title,
                    Body = model.Body,
                    Location = model.Location,
                    AllDay = model.AllDay,
                    StartDateTime = DateTime.Parse(model.StartDateTime),
                    EndDateTime = DateTime.Parse(model.EndDateTime)
                };

                _eventRepo.Insert(newEvent);
                await SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEvent(string id)
        {
            try
            {
                //Parse guid
                Guid? guid = ParseGuid(id);
                if (guid == null) return false;

                var evnt = await _eventRepo.GetByIdAsync(guid.Value);

                if (evnt == null) return false;

                _eventRepo.Delete(evnt);
                await SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
