using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AINT354_Mobile_API.BusinessLogic;
using AINT354_Mobile_API.ModelDTOs;

namespace AINT354_Mobile_API.Controllers
{
    public class EventsController : ApiController
    {
        private readonly EventService _eventService;

        public EventsController()
        {
            _eventService = new EventService();
        }

        /// <summary>
        /// Returns a list of simplified events for a specified calendar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof (List<EventDTO>))]
        public async Task<IHttpActionResult> CalendarEvents(string id)
        {
            if (id == string.Empty) return BadRequest();

            List<EventDTO> events = await _eventService.GetCalendarEvents(id);

            return Ok(events);
        }

        /// <summary>
        /// Returns the full details of an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(EventDetailsDTO))]
        public async Task<IHttpActionResult> EventDetails(string id)
        {
            if (id == string.Empty) return BadRequest();

            EventDetailsDTO eventDetails = await _eventService.GetEventsDetails(id);

            return Ok(eventDetails);
        }

        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Create(EventCreateDTO model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var success = await _eventService.CreateEvent(model);

            if (success)
            {
                return Ok();
            }

            return BadRequest("Unable to create the event! :(");
        }

        /// <summary>
        /// Deletes an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string id)
        {
            if (id == string.Empty) { return BadRequest(); }

            var success = await _eventService.DeleteEvent(id);

            if (success)
            {
                return Ok();
            }

            return BadRequest("Unable to delete event ID:" + id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}