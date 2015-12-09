using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AINT354_Mobile_API.ModelDTOs;
using AINT354_Mobile_API.Models;
using GamingSessionApp.DataAccess;
using static AINT354_Mobile_API.BusinessLogic.LookUpEnums;

namespace AINT354_Mobile_API.BusinessLogic
{
    public class CalendarService : BaseLogic
    {
        //Session Repository
        private readonly GenericRepository<Calendar> _calendarRepo;
        private readonly GenericRepository<CalendarMember> _calendarMemberRepo;

        public CalendarService()
        {
            _calendarRepo = UoW.Repository<Calendar>();
            _calendarMemberRepo = UoW.Repository<CalendarMember>();
        }

        public async Task<List<CalendarDTO>> GetUserCalendars(int id)
        {
            //Search the calendar members table and pull out the users calendars
            var calendars = await _calendarMemberRepo.Get(x => x.UserId == id)
                .Select(x => new CalendarDTO
                {
                    Id = x.CalendarId.ToString(),
                    Name = x.Calendar.Name,
                    OwnerId = x.Calendar.OwnerId,
                    ColourId = x.Calendar.Colour.Id,
                    Description = x.Calendar.Description
                }).ToListAsync();

            return calendars;
        }

        public async Task<bool> CreateCalendar(CalendarDTO dto)
        {
            try
            {
                //Parse guid
                Guid? guid = ParseGuid(dto.Id);
                if (guid == null) return false;

                Calendar calendar = new Calendar
                {
                    Id = guid.Value,
                    Name = dto.Name,
                    Description = dto.Description,
                    OwnerId = dto.OwnerId,
                    ColourId = dto.ColourId
                };
                
                //Add creator as a member of the calendar
                calendar.Members.Add(new CalendarMember
                {
                    UserId = calendar.OwnerId
                });

                _calendarRepo.Insert(calendar);
                await SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCalendar(string id)
        {
            try
            {
                //Parse guid
                Guid? guid = ParseGuid(id);
                if (guid == null) return false;

                var calendar = await _calendarRepo.GetByIdAsync(guid.Value);

                if (calendar == null) return false;

                _calendarRepo.Delete(calendar);
                await SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CalendarExist(string id)
        {
            //Parse guid
            Guid? guid = ParseGuid(id);
            if (guid == null) return false;

            var cal = await _calendarRepo.GetByIdAsync(guid.Value);

            //Return true if we have a calendar
            return cal != null;
        }
    }
}
