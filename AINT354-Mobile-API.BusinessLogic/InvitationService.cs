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
    public class InvitationService : BaseLogic
    {
        private readonly GenericRepository<Invitation> _inviteRepo;
        private readonly GenericRepository<Calendar> _calendarRepo;
        private readonly GenericRepository<Event> _eventRepo;

        public InvitationService()
        {
            _inviteRepo = UoW.Repository<Invitation>();
            _calendarRepo = UoW.Repository<Calendar>();
            _eventRepo = UoW.Repository<Event>();
        }


        public async Task<List<InvitationDTO>> GetUserInvitations(int id)
        {
            try
            {
                var invites = await _inviteRepo.Get(x => x.RecipientId == id)
                    .Where(x => x.Responded == false)
                    .OrderBy(x => x.CreatedDate)
                    .Select(x => new InvitationDTO
                    {
                        Id = x.Id.ToString(),
                        From = x.Sender.Name,
                        DateSent = x.CreatedDate.ToString(),
                        Type = x.Type.Name,
                        DisplayMessage = x.DisplayMessage
                    })
                    .ToListAsync();

                //Format the dates ready to send
                foreach (var inv in invites)
                {
                    inv.DateSent = FormatDateString(inv.DateSent);
                }

                return invites;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<ValidationResult> HandleInviteResponse(InviteRSVP model)
        {
            try
            {
                //Parse guid
                Guid? guid = ParseGuid(model.Id);
                if (guid == null)
                {
                    Result.Error = "Failed to parse invitation id";
                    return Result;
                }

                //Attempt to load the Invitation from the db
                Invitation inv = await _inviteRepo.GetByIdAsync(guid.Value);

                if (inv == null)
                {
                    Result.Error = "Invitation does not exist for id : " + model.Id;
                    return Result;
                }

                //Make sure we aren't processing a previously processed invite
                if (inv.Responded)
                {
                    Result.Error = "Invitation has already been processed";
                    return Result;
                }

                //If we are accepting the invitation
                if (model.Accept)
                {
                    //Handle the type of invitation
                    switch (inv.TypeId)
                    {
                        //If calendar add member to calendar
                        case (int)LookUpEnums.InvitationTypes.Calendar:
                            Calendar cal = await _calendarRepo.Get(x => x.Id == inv.CalendarId.Value)
                                .Include(x => x.Members).FirstOrDefaultAsync();

                            if (cal == null)
                                return AddError("Calendar no longer exists. The owner may have deleted the calendar");

                            //Only add the user if they aren't already a member
                            if (cal.Members.All(x => x.UserId != inv.RecipientId))
                            {
                                cal.Members.Add(new CalendarMember { UserId = inv.RecipientId});

                                _calendarRepo.Update(cal);
                            }

                            break;

                        //If Event add member to event
                        case (int)LookUpEnums.InvitationTypes.Event:

                            //Check we have a valid destination calendar to add the event too.
                            Guid? destCalId = ParseGuid(model.DestCalendarId);

                            if (destCalId == null)
                                return AddError("Failed to parse the destination calendar id");

                            Event evnt = await _eventRepo.Get(x => x.Id == inv.EventId.Value)
                                .Include(x => x.Members).FirstOrDefaultAsync();

                            if (evnt == null)
                                return AddError("Event no longer exists. The owner may have deleted the event");

                            //Only add the user if they aren't already a member
                            if (evnt.Members.All(x => x.UserId != inv.RecipientId))
                            {
                                evnt.Members.Add(new EventMember { UserId = inv.RecipientId });

                                _eventRepo.Update(evnt);
                            }

                            //Check the calendar exists
                            Calendar destCalendar = await _calendarRepo.GetByIdAsync(destCalId.Value);

                            if (destCalendar == null)
                                return AddError("404 : Destination Calendar not found");

                            //Add the dest calendar to the events calendars
                            if (evnt.Calendars.All(x => x.Id != destCalId.Value))
                            {
                                evnt.Calendars.Add(destCalendar);
                                _eventRepo.Update(evnt);
                            }
                            break;
                    }
                }

                //Set our invite to responded.
                inv.Responded = true;

                //Update database record
                _inviteRepo.Update(inv);
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

        public async Task<bool> AddInvitation(Invitation inv)
        {
            try
            {
                _inviteRepo.Insert(inv);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Invitation> CheckForDuplicates(int recipientId, int senderId, Guid? calId, Guid? eventId)
        {
            try
            {
                //Initial query
                IQueryable<Invitation> query = _inviteRepo.Get(x => x.RecipientId == recipientId &&
                                                 x.SenderId == senderId &&
                                                 x.Responded == false);

                if (calId.HasValue)
                {
                    query = query.Where(x => x.CalendarId == calId);
                }
                else if (eventId.HasValue)
                {
                    query = query.Where(x => x.EventId == eventId);
                }

                //Execute query
                Invitation inv = await query.FirstOrDefaultAsync();

                return inv;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
