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
    public class ShareService : BaseLogic
    {
        //User Repository
        private readonly GenericRepository<User> _userRepo;
        private readonly CalendarService _calendarService;
        private readonly EventService _eventService;
        private readonly InvitationService _invitationService;

        public ShareService()
        {
            _userRepo = UoW.Repository<User>();
            _calendarService = new CalendarService();
            _eventService = new EventService();
            _invitationService = new InvitationService();
        }

        public async Task<ValidationResult> ShareCalendar(ShareDetails model)
        {
            try
            {
                //Check calendar exists
                bool exists = await _calendarService.CalendarExist(model.CalendarId);

                if (!exists)
                {
                    Result.Error = "Calendar was not found";
                    return Result;
                }

                //Check if the recipient is a member of the app
                var user = await _userRepo.Get(x => x.Email == model.Email).FirstOrDefaultAsync();

                //If this user isn't registered send invite email
                if (user == null)
                {
                    await SendInvitationEmail(model);
                    Result.Success = true;
                    return Result;
                }


                //Parse guid
                Guid? guid = ParseGuid(model.CalendarId);
                if (guid == null) return Result;
                
                //Check for an existing calendar invitation
                Invitation existingInvitation = await _invitationService.CheckForDuplicates(user.Id, model.SenderUserId, guid.Value, null);

                if (existingInvitation != null)
                {
                    Result.Error = "An invitation matching the specified values already exists";
                    return Result;
                }

                //Assuming we have a registered user and no existing invitation, create a new one
                Invitation inv = new Invitation
                {
                    CalendarId = guid.Value,
                    RecipientId = user.Id,
                    SenderId = model.SenderUserId,
                    TypeId = (int)LookUpEnums.InvitationTypes.Calendar
                };

                await _invitationService.AddInvitation(inv);

                string senderName =
                    await _userRepo.Get(x => x.Id == model.SenderUserId).Select(x => x.Name).FirstAsync();

                //Finally send out GCM notification
                string message = $"{senderName} wants to share their Calendar";

                //Send the notification
                AndroidGCMPushNotification.SendNotification(user.DeviceId, message);

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Error = ex.Message;
                return Result;
            }
        }

        public async Task<ValidationResult> ShareEvent(ShareDetails model)
        {
            try
            {
                //Check Event exists
                bool exists = await _eventService.EventExist(model.EventId);

                if (!exists)
                {
                    Result.Error = "Event was not found";
                    return Result;
                }

                //Check if the recipient is a member of the app
                var user = await _userRepo.Get(x => x.Email == model.Email).FirstOrDefaultAsync();

                //If this user isn't registered send invite email
                if (user == null)
                {
                    await SendInvitationEmail(model);
                    Result.Success = true;
                    return Result;
                }

                //Parse guid
                Guid? guid = ParseGuid(model.EventId);
                if (guid == null) return Result;

                //Check for an existing event invitation
                Invitation existingInvitation = await _invitationService.CheckForDuplicates(user.Id, model.SenderUserId, null, guid.Value);

                if (existingInvitation != null)
                {
                    Result.Error = "An invitation matching the specified values already exists";
                    return Result;
                }

                //Assuming we have a registered user create an invitation
                Invitation inv = new Invitation
                {
                    EventId = guid.Value,
                    RecipientId = user.Id,
                    SenderId = model.SenderUserId,
                    TypeId = (int)LookUpEnums.InvitationTypes.Event
                };

                await _invitationService.AddInvitation(inv);

                string senderName =
                    await _userRepo.Get(x => x.Id == model.SenderUserId).Select(x => x.Name).FirstAsync();

                //Finally send out GCM notification
                string message = $"{senderName} wants to share their Calendar";

                //Send the notification
                AndroidGCMPushNotification.SendNotification(user.DeviceId, message);

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Error = ex.Message;
                return Result;
            }
        }

        private async Task<bool> SendInvitationEmail(ShareDetails model)
        {
            User sender = await _userRepo.GetByIdAsync(model.SenderUserId);
            if (sender == null) return false;

            string type = model.CalendarId != null ? "calendar" : "event";
            string subject = $"{sender.Name} has invited to share their {type} with you!";
            StringBuilder emailBody = new StringBuilder();

            emailBody.Append("Hey!\n\n");
            emailBody.Append($"Your friend {sender.Name} has invited you to join and share their {type} on What's Up!\n\n");
            emailBody.Append($"You can accept this invitation and view the {type} on the What's Up application.\n\n");
            emailBody.Append("Open the following link on your android mobile device to proceed: \n\n");
            emailBody.Append("---------------------------------------------------------------------------------------------\n");
            emailBody.Append("[ DEEPLINK HERE ]\n");
            emailBody.Append("---------------------------------------------------------------------------------------------\n\n");
            emailBody.Append("See you soon!\n\n");
            emailBody.Append("What's Up Team");



            EmailHelper email = new EmailHelper();
            email.AddToAddress(model.Email);
            email.SetSubject(subject);
            email.SetBody(emailBody.ToString());

            return await email.Send();
        }
    }
}
