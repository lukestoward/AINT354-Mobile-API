using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        private readonly GenericRepository<Calendar> _calendarRepo;

        public EventService()
        {
            _eventRepo = UoW.Repository<Event>();
            _calendarRepo = UoW.Repository<Calendar>();
        }

        public async Task<List<EventDTO>> GetCalendarEvents(string calId)
        {
            //Parse guid
            Guid? guid = ParseGuid(calId);
            if (guid == null) return null;

            var events = await _calendarRepo.Get(x => x.Id == guid.Value)
                .SelectMany(x => x.Events)
                .Select(x => new EventDTO
                {
                    Id = x.Id.ToString(),
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
                    CreatorId = x.CreatorId,
                    CreatorName = x.Creator.Name,
                    CreatedDate = x.CreatedDate,
                    Title = x.Title,
                    Body = x.Body,
                    Location = x.Location,
                    AllDay = x.AllDay,
                    StartDateTime = x.StartDateTime.ToString(),
                    EndDateTime = x.EndDateTime.ToString(),
                    MembersCount = x.Members.Count,
                }).FirstOrDefaultAsync();

            //Convert the dates in to specific formats
            eventDetails.StartDateTime = FormatDateString(eventDetails.StartDateTime);
            eventDetails.EndDateTime = FormatDateString(eventDetails.EndDateTime);

            return eventDetails;
        }

        public async Task<ValidationResult> CreateEvent(EventCreateDTO model)
        {
            try
            {
                //Parse guid(s)
                Guid? id = ParseGuid(model.Id);
                if (id == null)
                {
                    Result.Error = "Failed to parse provided Id";
                    return Result;
                }

                Guid? calId = ParseGuid(model.CalendarId);
                if (calId == null)
                {
                    Result.Error = "Failed to parse provided CalendarId";
                    return Result;
                }

                //Load the calendar
                Calendar cal = await _calendarRepo.GetByIdAsync(calId.Value);

                if (cal == null)
                {
                    Result.Error = "404 : Calendar not found";
                    return Result;
                }

                Event newEvent = new Event
                {
                    Id = id.Value,
                    CreatorId = model.CreatorId,
                    Title = model.Title,
                    Body = model.Body,
                    Location = model.Location,
                    AllDay = model.AllDay,
                    StartDateTime = ParseUKDate(model.StartDateTime),
                    EndDateTime = ParseUKDate(model.EndDateTime)
                };
                
                //Add the creator as a member
                EventMember member = new EventMember { UserId = model.CreatorId };
                newEvent.Members.Add(member);

                //Add the parent calendar
                newEvent.Calendars.Add(cal);

                //If all day event override start/end times
                if (newEvent.AllDay)
                {
                    newEvent.StartDateTime = newEvent.StartDateTime.Date; //Trims the time off the date
                    newEvent.EndDateTime= newEvent.EndDateTime.Date.AddDays(1);//Rounds to the start of the next day
                }

                _eventRepo.Insert(newEvent);
                await SaveChangesAsync();

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Error = ex.Message;

                if (ex.InnerException != null)
                Result.Error += "\nInner exception: " + ex.InnerException;

                return Result;
            }
        }

        public async Task<ValidationResult> UpdateEvent(EventUpdateDTO model)
        {
            try
            {
                //Parse guid(s)
                Guid? eId = ParseGuid(model.Id);
                if (eId == null)
                {
                    Result.Error = "Failed to parse provided Id";
                    return Result;
                }

                //Guid? calId = ParseGuid(model.CalendarId);
                //if (calId == null)
                //{
                //    Result.Error = "Failed to parse provided CalendarId";
                //    return Result;
                //}

                //Load the event from the db
                Event evnt = await _eventRepo.GetByIdAsync(eId.Value);

                if (evnt == null)
                {
                    Result.Error = "404 : Event not found";
                    return Result;
                }
                
                //Update properties
                evnt.Title = model.Title;
                evnt.Body = model.Body;
                evnt.Location = model.Location;
                evnt.AllDay = model.AllDay;
                evnt.StartDateTime = ParseUKDate(model.StartDateTime);
                evnt.EndDateTime = ParseUKDate(model.EndDateTime);

                //If all day event override start/end times
                if (evnt.AllDay)
                {
                    evnt.StartDateTime = evnt.StartDateTime.Date; //Trims the time off the date
                    evnt.EndDateTime = evnt.EndDateTime.Date.AddDays(1);//Rounds to the start of the next day
                }

                _eventRepo.Update(evnt);
                await SaveChangesAsync();

                Result.Success = true;
                return Result;

            }
            catch (Exception ex)
            {
                Result.Error = ex.Message;

                if (ex.InnerException != null)
                    Result.Error += "\nInner exception: " + ex.InnerException;

                return Result;
            }
        }

        public async Task<ValidationResult> DeleteEvent(string id)
        {
            try
            {
                //Parse guid
                Guid? guid = ParseGuid(id);
                if (guid == null) return AddError("Invalid Id provided");

                var evnt = await _eventRepo.Get(x => x.Id == guid.Value)
                    .Include(x => x.Calendars).FirstOrDefaultAsync();

                if (evnt == null) return AddError("404 : Event not found");

                //Delete associated calendars
                evnt.Calendars.Clear();

                _eventRepo.Delete(evnt);
                await SaveChangesAsync();

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Error = ex.Message;

                if (ex.InnerException != null)
                    Result.Error += "\nInner exception: " + ex.InnerException;

                return Result;
            }
        }

        public async Task<bool> EventExist(string eventId)
        {
            //Parse guid
            Guid? guid = ParseGuid(eventId);
            if (guid == null) return false;

            var evnt = await _eventRepo.GetByIdAsync(guid.Value);

            //Return true if we have a calendar
            return evnt != null;
        }
    }
}
