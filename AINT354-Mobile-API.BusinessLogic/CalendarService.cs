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

        public CalendarService()
        {
            _calendarRepo = UoW.Repository<Calendar>();
        }

        public async Task<List<CalendarDTO>> GetUserCalendars(int id)
        {
            var calendars = await _calendarRepo.Get(x => x.OwnerId == id)
                .Include(x => x.Colour)
                .Select(x => new CalendarDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    OwnerId = x.OwnerId,
                    ColourId = x.Colour.Id,
                    Description = x.Description
                })
                .ToListAsync();

            return calendars;
        }

        public async Task<bool> CreateCalendar(CalendarDTO dto)
        {
            try
            {
                Calendar calendar = new Calendar
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    OwnerId = dto.OwnerId,
                    ColourId = dto.ColourId
                }; 

                _calendarRepo.Insert(calendar);
                await SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCalendar(int id)
        {
            try
            {
                var calendar = await _calendarRepo.GetByIdAsync(id);

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
    }
}
