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

        public ShareService()
        {
            _userRepo = UoW.Repository<User>();
            _calendarService = new CalendarService();
        }

        public async Task<bool> ShareCalendar(ShareDetails model)
        {
            //Check calendar exists
            bool exists = await _calendarService.CalendarExist(model.CalendarId);

            if (!exists) return false;

            //Check if the recipient is a member of the app
            var user = await _userRepo.Get(x => x.Email == model.Email).FirstOrDefaultAsync();

            //If this user isn't registered send invite email
            if (user == null) return await SendInvitationEmail(model);

            //TODO: Create and send notification to recipients

            return true;
        }

        public async Task<bool> ShareEvent(ShareDetails model)
        {
            var user = await _userRepo.Get(x => x.Email == model.Email).FirstOrDefaultAsync();

            //If this user isn't registered send invite email
            if (user == null) return await SendInvitationEmail(model);


            //TODO: Create and send notification to recipients

            return true;
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
