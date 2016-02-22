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
    public class MemberService : BaseLogic
    {
        private readonly GenericRepository<EventMember> _eventMemberRepo;
        private readonly GenericRepository<CalendarMember> _calendarMemberRepo;
        private readonly GenericRepository<Event> _eventRepo;
        private readonly GenericRepository<Calendar> _calendarRepo;

        public MemberService()
        {
            _eventMemberRepo = UoW.Repository<EventMember>();
            _calendarMemberRepo = UoW.Repository<CalendarMember>();
            _eventRepo = UoW.Repository<Event>();
            _calendarRepo = UoW.Repository<Calendar>();
        }

        public async Task<List<MemberDTO>> GetEventMembers(Guid id)
        {
            try
            {
                //First load the members
                List<MemberDTO> members = await _eventMemberRepo.Get(x => x.EventId == id)
                    .Select(x => new MemberDTO
                    {
                        Id = x.UserId,
                        FacebookId = x.User.FacebookId,
                        Name = x.User.Name
                    })
                    .ToListAsync();

                return members;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ValidationResult> UpdateEventMembers(EventMembersDTO model)
        {
            try
            {
                //First load the members
                var members = await _eventMemberRepo.Get(x => x.EventId == model.EventId)
                    .ToListAsync();

                //Split the Ids string and create list of integers
                List<int> ids = model.MemberIds.Split(',').Select(int.Parse).ToList();

                //Create a new list of members
                List<EventMember> newMembersList = ids.Select(id => new EventMember
                {
                    EventId = model.EventId,
                    UserId = id
                }).ToList();

                foreach (var m in members)
                {
                    _eventMemberRepo.Delete(m);
                }

                //First check we have at least the owner member
                if (newMembersList.Count < 1) return AddError("Unable to update event members");

                //Insert the rebuilt list of users
                foreach (var newMem in newMembersList)
                {
                    _eventMemberRepo.Insert(newMem);
                }
                
                await SaveChangesAsync();

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                return AddError(ex.Message);
            }
        }

        public async Task<List<MemberDTO>> GetCalendarMembers(Guid id)
        {
            try
            {
                //Load the members, transforming the data to the DTO list
                List<MemberDTO> members = await _calendarMemberRepo.Get(x => x.CalendarId == id)
                    .Select(m => new MemberDTO
                    {
                        Id = m.UserId,
                        Name = m.User.Name,
                        FacebookId = m.User.FacebookId
                    })
                    .ToListAsync();

                return members;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ValidationResult> UpdateCalendarMembers(CalendarMembersDTO model)
        {
            try
            {
                //First load the member ids
                List<int> memberIds = await _calendarMemberRepo.Get(x => x.CalendarId == model.CalendarId)
                    .Select(x => x.UserId)
                    .ToListAsync();

                //Split the Ids string and create list of integers
                List<int> ids = model.MemberIds.Split(',').Select(int.Parse).ToList();

                //Calculate the members set for removal
                List<int> removedMemberIds = memberIds.Except(ids).ToList();

                foreach (var id in removedMemberIds)
                {
                    //Get all of the members events for the calendar
                    var events = await _calendarRepo.Get(x => x.Id == model.CalendarId)
                        .SelectMany(x => x.Events)
                        .SelectMany(x => x.Members)
                        .Where(m => m.UserId == id)
                        .ToListAsync();

                    //Remove the member from those events
                    foreach (var e in events)
                    {
                        _eventMemberRepo.Delete(e);
                    }

                    //Unlink the member(s) from the calendars
                    CalendarMember calMem =
                        await _calendarMemberRepo.Get(x => x.CalendarId == model.CalendarId && x.UserId == id)
                            .FirstOrDefaultAsync();
                    _calendarMemberRepo.Delete(calMem);
                }

                //Check we have at least the owner member
                if (removedMemberIds.Count >= memberIds.Count) return AddError("Unable to update calendar members");

                await SaveChangesAsync();

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                return AddError(ex.Message);
            }
        }
    }
}
