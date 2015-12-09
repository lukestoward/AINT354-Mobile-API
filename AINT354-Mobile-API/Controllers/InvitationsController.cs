using System;
using System.Collections.Generic;
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
    public class InvitationsController : ApiController
    {
        private readonly InvitationService _invitationService;

        public InvitationsController()
        {
            _invitationService = new InvitationService();
        }

        /// <summary>
        /// Returns a list of calendars for the specified user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<InvitationDTO>))]
        public async Task<IHttpActionResult> UserInvitations(int id)
        {
            if (id == 0) return BadRequest("Id is invalid");

            var invites = await _invitationService.GetUserInvitations(id);

            return Ok(invites);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Respond(InviteRSVP model)
        {
            if (!ModelState.IsValid) return BadRequest("Data provided is invalid");

            var result = await _invitationService.HandleInviteResponse(model);

            if (result.Success)
                return Ok();

            return BadRequest(result.Error);

        } 
    }
}
