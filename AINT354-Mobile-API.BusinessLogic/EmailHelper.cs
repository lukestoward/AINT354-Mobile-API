using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SendGrid;

namespace AINT354_Mobile_API.BusinessLogic
{
    public class EmailHelper
    {
        private SendGridMessage email;

        public EmailHelper()
        {
            email = new SendGridMessage();
        }

        public void AddFromAddress(string address, string displayName)
        {
            email.From = new MailAddress(address, displayName);
        }

        public void AddToAddress(string address)
        {
            email.AddTo(address);
        }

        public void AddToAddress(List<string> addressList)
        {
            email.AddTo(addressList);
        }

        public void SetSubject(string subject)
        {
            email.Subject = subject;
        }

        public void SetBody(string body)
        {
            email.Text = body;
        }

        public async Task<bool> Send()
        {
            try
            {
                //Set default from address if null
                if (email.From == null) email.From = new MailAddress("noreply@whatsup.com", "What's Up Invitation");

                if (email.To == null || email.Cc == null || email.Bcc == null) return false;


                // Create a Web transport, using API Key
                var transportWeb = new Web("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");

                // Send the email.
                await transportWeb.DeliverAsync(email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
