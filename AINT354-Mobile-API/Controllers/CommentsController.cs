using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AINT354_Mobile_API.BusinessLogic;
using AINT354_Mobile_API.ModelDTOs;

namespace AINT354_Mobile_API.Controllers
{
    public class CommentsController : ApiController
    {
        private readonly CommentService _commentService;

        public CommentsController()
        {
            _commentService = new CommentService();
        }

        [HttpGet]
        [ResponseType(typeof(List<CommentDTO>))]
        public async Task<IHttpActionResult> EventComments(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            List<CommentDTO> comments = await _commentService.GetEventComments(id);

            return Ok(comments);
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateComment(NewCommentDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _commentService.CreateComment(model);

            if(result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteComment(DeleteCommentDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _commentService.DeleteComment(model);

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
                _commentService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}