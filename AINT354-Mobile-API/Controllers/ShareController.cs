using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AINT354_Mobile_API.BusinessLogic;
using AINT354_Mobile_API.ModelDTOs;

namespace AINT354_Mobile_API.Controllers
{
    public class ShareController : ApiController
    {
        private readonly ShareService _shareService;

        public ShareController()
        {
            _shareService = new ShareService();
        }

        /// <summary>
        /// Share a calendar with a single specified user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Calendar(ShareDetails model)
        {
            if (model.CalendarId == null) ModelState.AddModelError("CalendarId", "CalendarId is required");

            if (!ModelState.IsValid) { return BadRequest("Missing required information"); }

            var result = await _shareService.ShareCalendar(model);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Share an event with a single specified user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Event(ShareDetails model)
        {
            if(model.EventId == null) ModelState.AddModelError("EventId", "EventId is required");

            if (!ModelState.IsValid) { return BadRequest("Missing required information"); }

            var result = await _shareService.ShareEvent(model);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _shareService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
