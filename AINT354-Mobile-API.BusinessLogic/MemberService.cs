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
        private readonly GenericRepository<EventMember> _memberRepo;
        private readonly GenericRepository<Event> _eventRepo;

        public MemberService()
        {
            _memberRepo = UoW.Repository<EventMember>();
            _eventRepo = UoW.Repository<Event>();
        }

        public async Task<List<MemberDTO>> GetEventMembers(Guid id)
        {
            try
            {
                //First load the event
                var members = await _memberRepo.Get(x => x.EventId == id)
                    .Include(x => x.User)
                    .ToListAsync();

                List<MemberDTO> memberDTOs = members.Select(m => new MemberDTO
                {
                    Id = m.User.Id,
                    Name = m.User.Name
                }).ToList();

                return memberDTOs;
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
                //Load the event
                Event ev = await _eventRepo.Get(x => x.Id == model.EventId)
                    .Include(x => x.Members)
                    .FirstOrDefaultAsync();

                if (ev == null) return AddError("404: Event not found!");

                //Clear the list of members
                ev.Members.Clear();

                //Create the new list of members
                foreach (var id in model.MemberIds)
                {
                    ev.Members.Add(new EventMember{ UserId = id });
                }

                if (ev.Members.Count < 1) return AddError("Unable to update event members");

                //Update db
                _eventRepo.Update(ev);
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
