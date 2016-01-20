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
    public class MembersController : ApiController
    {
        private readonly MemberService _memberService;

        public MembersController()
        {
            _memberService = new MemberService();
        }
        
        [HttpGet]
        [ResponseType(typeof(List<MemberDTO>))]
        public async Task<IHttpActionResult> EventMembers(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var members = await _memberService.GetEventMembers(id);

            return Ok(members);
        }

        [HttpPut]
        public async Task<IHttpActionResult> EventMembers(EventMembersDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _memberService.UpdateEventMembers(model);

            if(result.Success)
                return Ok();

            return BadRequest(result.Error);
        }

        [HttpGet]
        [ResponseType(typeof(List<MemberDTO>))]
        public async Task<IHttpActionResult> CalendarMembers(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var members = await _memberService.GetCalendarMembers(id);

            return Ok(members);
        }

        [HttpPut]
        public async Task<IHttpActionResult> CalendarMembers(CalendarMembersDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();

            ValidationResult result = await _memberService.UpdateCalendarMembers(model);

            if (result.Success)
                return Ok();

            return BadRequest(result.Error);
        }

    }
}
