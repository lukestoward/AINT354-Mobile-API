using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AINT354_Mobile_API.ModelDTOs;
using GamingSessionApp.DataAccess;
using AINT354_Mobile_API.Models;
using System.Data.Entity;

namespace AINT354_Mobile_API.BusinessLogic
{
    public class CommentService : BaseLogic
    {
        private readonly GenericRepository<Event> _eventRepo;
        private readonly GenericRepository<EventComment> _commentsRepo;

        public CommentService()
        {
            _eventRepo = UoW.Repository<Event>();
            _commentsRepo = UoW.Repository<EventComment>();
        }

        public async Task<List<CommentDTO>> GetEventComments(Guid id)
        {
            try
            {
                //Try and find the Event
                Event evnt = await _eventRepo.Get(x => x.Id == id)
                    .Include(x => x.Comments)
                    .Include(x => x.Creator)
                    .FirstOrDefaultAsync();

                if (evnt == null) return null;

                List<CommentDTO> comments = new List<CommentDTO>();

                //Map comments to DTO objects
                foreach (var comment in evnt.Comments)
                {
                    CommentDTO c = new CommentDTO
                    {
                        Id = comment.Id,
                        Author = comment.Creator.Name,
                        CreatedDate = comment.CreatedDate.ToShortDateTimeString(),
                        Body = comment.Body,
                        OrderNo = comment.OrderNo
                    };

                    comments.Add(c);
                }

                //Order correctly ready to be returned
                comments.OrderBy(x => x.OrderNo);

                return comments;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ValidationResult> DeleteComment(DeleteCommentDTO model)
        {
            try
            {
                //Find and delete the comment
                EventComment comment = await _commentsRepo.GetByIdAsync(model.CommentId);

                if (comment == null)
                    return AddError("Comment could not be found");

                if(comment.EventId != model.EventId && comment.CreatorId != model.UserId)
                    return AddError("No Comment matching the EventId and/or the UserId could be found");
                
                _commentsRepo.Delete(comment);
                await SaveChangesAsync();

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                return AddError(ex.Message);            
            }
           
        }

        public async Task<ValidationResult> CreateComment(NewCommentDTO model)
        {
            try
            {
                //Firstly check the Event exists
                Event evnt = await _eventRepo.Get(x => x.Id == model.EventId)
                    .Include(x => x.Comments)
                    .Include(x => x.Members.Select(u => u.User))
                    .FirstOrDefaultAsync();

                if (evnt == null)
                    return AddError("Event does not exist");

                //Make sure the creator is a member of the event
                bool isMember = evnt.Members.Any(x => x.UserId == model.CreatorId);

                if(!isMember)
                    return AddError("The creator provided isn't a member of the event!");

                //Create the new Comment entity
                EventComment comment = new EventComment
                {
                    CreatorId = model.CreatorId,
                    EventId = evnt.Id,
                    Body = model.Body,
                    OrderNo = 0
                };

                //Set the correct order number
                int nextNo = 0;
                foreach (var c in evnt.Comments)
                {
                    nextNo = c.OrderNo > nextNo ? c.OrderNo : nextNo;
                }

                comment.OrderNo = nextNo + 1;

                //Attached comment to event
                evnt.Comments.Add(comment);

                //Update database
                _eventRepo.Update(evnt);
                await SaveChangesAsync();

                //Get members deviceIds
                List<string> deviceIds = new List<string>();

                foreach (var member in evnt.Members)
                {
                    if(member.UserId != model.CreatorId)
                        deviceIds.Add(member.User.DeviceId);
                }

                if (deviceIds.Any())
                {
                    //Finally send out notifications to members
                    string authorName = evnt.Members.Where(x => x.UserId == model.CreatorId)
                        .First().User.Name;

                    string message = $"{authorName} has posted a comment on \"{evnt.Title}\"";

                    AndroidGCMPushNotification.SendNotification(deviceIds, message);
                }

                Result.Success = true;
                return Result;
            }
            catch (Exception ex)
            {
                return AddError(ex.Message);
            }
        }
    }
}
