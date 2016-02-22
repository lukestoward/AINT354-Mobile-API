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
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime tomorrow = today.AddDays(1);

            //Search the calendar members table and pull out the users calendars
            var calendars = await _calendarMemberRepo.Get(x => x.UserId == id)
                .Select(x => new CalendarDTO
                {
                    Id = x.CalendarId.ToString(),
                    Name = x.Calendar.Name,
                    OwnerId = x.Calendar.OwnerId,
                    ColourId = x.Calendar.ColourId,
                    Description = x.Calendar.Description,
                    Shared = x.Calendar.Members.Count > 1,
                    TodaysEventCount = x.Calendar.Events.Count(e => e.StartDateTime >= today && e.StartDateTime <= tomorrow)
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

        public async Task<ValidationResult> UpdateCalendar(CalendarDTO model)
        {
            try
            {
                //Parse guid
                Guid? guid = ParseGuid(model.Id);
                if (guid == null) return Result;

                //Find calendar in db
                var calendar = await _calendarRepo.GetByIdAsync(guid.Value);

                if (calendar == null)
                {
                    Result.Error = "Calendar not found!";
                    return Result;
                }

                calendar.ColourId = model.ColourId;
                calendar.Description = model.Description;
                calendar.Name = model.Name;

                _calendarRepo.Update(calendar);
                await SaveChangesAsync();

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Error = ex.Message;
                return Result;
            }
        }

        public async Task<ValidationResult> DeleteCalendar(string id)
        {
            try
            {
                //Parse guid
                Guid? guid = ParseGuid(id);
                if (guid == null) return AddError("Invalid Id provided");

                var calendar = await _calendarRepo.Get(x => x.Id == guid.Value)
                    .Include(x => x.Events).FirstOrDefaultAsync();

                if (calendar == null) return AddError("404 : Calendar not found");

                //Delete associated calendars
                calendar.Events.Clear();

                _calendarRepo.Delete(calendar);
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
