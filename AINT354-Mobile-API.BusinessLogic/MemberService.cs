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
                //First load the members
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
                //First load the members
                var members = await _memberRepo.Get(x => x.EventId == model.EventId)
                    .ToListAsync();

                //Create a new list of members
                List<EventMember> newMembersList = model.MemberIds.Select(id => new EventMember
                {
                    EventId = model.EventId,
                    UserId = id
                }).ToList();

                foreach (var m in members)
                {
                    _memberRepo.Delete(m);
                }

                //First check we have at least the owner member
                if (newMembersList.Count < 1) return AddError("Unable to update event members");

                //Insert the rebuilt list of users
                foreach (var newMem in newMembersList)
                {
                    _memberRepo.Insert(newMem);
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
    }
}
