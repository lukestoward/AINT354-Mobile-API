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
    public class UsersController : ApiController
    {
        private readonly UserService _userService;

        public UsersController()
        {
            _userService = new UserService();
        }

        /// <summary>
        /// Use to register a new user to the application
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterUser model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            
            var result = await _userService.RegisterUser(model);

            if (result.Success)
            {
                int userId = await  _userService.GetUserId(model.FacebookId);

                if (userId == 0)
                {
                    return BadRequest("Error retrieving the UserId.");
                }

                return Ok(userId);
            }

            return BadRequest(result.Error);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
