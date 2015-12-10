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
    public class UserService : BaseLogic
    {
        //Session Repository
        private readonly GenericRepository<User> _userRepo;

        public UserService()
        {
            _userRepo = UoW.Repository<User>();
        }

        public async Task<ValidationResult> RegisterUser(RegisterUser model)
        {
            try
            {
                //Check user doesn't already exist
                var user = await _userRepo.Get(x => x.Email == model.Email)
                    .FirstOrDefaultAsync();

                //If the user already exists return the error
                if (user != null)
                {
                    if (user.Email == model.Email)
                        Result.Error = $"A user with the email {model.Email} already exists";
                    //else if(user.DeviceId == model.DeviceId)
                    //    Result.Error = $"A user with the deviceId {model.DeviceId} already exists";

                    Result.Success = false;
                    return Result;
                }

                User newUser = new User
                {
                    Email = model.Email,
                    Name = model.FullName,
                    DeviceId = model.DeviceId,
                    FacebookId = model.FacebookId
                };

                _userRepo.Insert(newUser);
                await SaveChangesAsync();

                //Send Welcome Email?
                //EmailService.SendWelcomeEmail(newUser)

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Error = ex.Message;
                return Result;
            }
        }

        public async Task<int> GetUserId(long facebookId)
        {
            try
            {
                var user = await _userRepo.Get(x => x.FacebookId == facebookId).FirstOrDefaultAsync();

                return user?.Id ?? 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
