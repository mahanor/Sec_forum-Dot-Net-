using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Sec_Forum.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

using System.Data.Entity;
using System.ComponentModel.Design;


namespace Sec_Forum.Controllers
{
    public class HomePageController : Controller
    {
        private readonly SecForumContext _context;
        string uid = Guid.NewGuid().ToString();

        public HomePageController(SecForumContext context)
        {
            _context = context;
        }




        #region Index
        public async Task<IActionResult> Index(string searchString, string sortFilter, string id)
        {

            string Username = HttpContext.Session.GetString("Username");
            if (id != null)
            {
                var query = _context.TblAddPostDetails
                .Join(
                    _context.TblUserMasters,
                    addPost => addPost.UserUid,
                    user => user.UId,

                    (addPost, user) => new TblAddPost
                    {
                        TblAddPosts = addPost,
                        TblUser = user,

                    }
                )
                .Where(joinResult => joinResult.TblAddPosts.UId == id)
                .Select(joined => new ResultModel
                {
                    CreatedDate = (DateTime)joined.TblAddPosts.CreatedDate,
                    Id = joined.TblUser.Id,
                    UId = joined.TblUser.UId,
                    Name = joined.TblUser.Name,
                    ProfileImage = joined.TblUser.ProfileImage,
                    Designation = joined.TblUser.Designation,
                    ProjectBody = joined.TblAddPosts.ProjectBody,
                    PostUid = joined.TblAddPosts.UId,
                    Tags = joined.TblAddPosts.Tags,
                    ProjectTitle = joined.TblAddPosts.ProjectTitle,
                    UploadDocument = joined.TblAddPosts.UploadDocument,
                    UploadFile = joined.TblAddPosts.UploadFile,

                })
                .ToList(); // Fetch the initial project details


                string userId = HttpContext.Session.GetString("User_uid");
                var Comments = _context.TblActivityMasters
                    .Join(
                            _context.TblUserMasters,
                            activity => activity.UserUid,
                            user => user.UId,

                            (activity, user) => new TblAddPost
                            {
                                TblActivity = activity,
                                TblUser = user
                            }
                        )
                    .Where(T => T.TblActivity.PostId == id).ToList();
                List<CommentModel> commentModels = new List<CommentModel>();
                foreach (var comment in Comments)
                {
                    CommentModel commentModel = new CommentModel();
                    commentModel.commentId = comment.TblActivity.UId;
                    commentModel.CommentsText = comment.TblActivity.CommentsText;
                    commentModel.Username = comment.TblUser.Username; // Include the user name from TblUserMasters
                    commentModel.Name = comment.TblUser.Name;
                    commentModel.ProfileImage = comment.TblUser.ProfileImage;
                    commentModel.Designation = comment.TblUser.Designation;
                    //CreatedDate = (DateTime)(joinResult.TblActivity.CreatedDate.HasValue ? (DateTime?)joinResult.TblActivity.CreatedDate.Value : null),
                    // CreatedDate = (DateTime)joinResult.TblActivity.CreatedDate,
                    commentModel.CreatedDate = (DateTime)comment.TblActivity.CreatedDate;

                    List<ReplyModel> replyModels = new List<ReplyModel>();
                    commentModel.Replies = replyModels;

                    var replys = _context.TblReplys
                        .Join(
                            _context.TblUserMasters,
                            reply => reply.UserUid,
                            user => user.UId,

                            (reply, user) => new TblAddPost
                            {
                                TblReply = reply,
                                TblUser = user
                            }
                        ).Where(T => T.TblReply.CommentId == commentModel.commentId).ToList();


                    foreach (var reply in replys)
                    {
                        ReplyModel replyModel = new ReplyModel();
                        replyModel.ReplyCommentId = reply.TblReply.CommentId;
                        replyModel.ReplyText = reply.TblReply.ReplyText;
                        replyModel.Username = reply.TblUser.Username;
                        replyModel.Name = reply.TblUser.Name;
                        replyModel.Designation = reply.TblUser.Designation;
                        replyModel.CreatedDate = (DateTime)reply.TblReply.CreatedDate;
                        replyModel.ProfileImage = reply.TblUser.ProfileImage;
                        replyModels.Add(replyModel);
                    }



                    commentModels.Add(commentModel);
                }
                foreach (var query1 in query)
                {
                    if (commentModels != null)
                    {
                        query1.Comments = commentModels;
                    }
                }


                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(item => item.Name.ToLower().Contains(searchString.ToLower()) || item.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
                }

                if (sortFilter == "Date")
                {
                    query = query.OrderByDescending(item => item.CreatedDate).ToList();
                }

                // Rest of the code...

                return View(query);
            }

            else
            {
                var query = _context.TblAddPostDetails
                .Join(
                    _context.TblUserMasters,
                    addPost => addPost.UserUid,
                    user => user.UId,

                    (addPost, user) => new TblAddPost
                    {
                        TblAddPosts = addPost,
                        TblUser = user
                    }
                )

                .Select(joined => new ResultModel
                {
                    CreatedDate = (DateTime)joined.TblAddPosts.CreatedDate,
                    Id = joined.TblUser.Id,
                    UId = joined.TblUser.UId,
                    Name = joined.TblUser.Name,
                    ProfileImage = joined.TblUser.ProfileImage,
                    Designation = joined.TblUser.Designation,
                    ProjectBody = joined.TblAddPosts.ProjectBody,
                    PostUid = joined.TblAddPosts.UId,
                    Tags = joined.TblAddPosts.Tags,
                    ProjectTitle = joined.TblAddPosts.ProjectTitle,
                    Username = joined.TblUser.Username,


                })
                .ToList(); // Fetch the initial project details

                /* var comment_id = await _context.TblActivityMasters.FindAsync(id1);*/

                string userId = HttpContext.Session.GetString("User_uid");
                var Comments = _context.TblActivityMasters.Where(T => T.PostId == id && T.PostId != null).ToList();
                List<CommentModel> commentModels = new List<CommentModel>();
                foreach (var comment in Comments)
                {
                    CommentModel commentModel = new CommentModel();
                    commentModel.commentId = comment.UId;
                    commentModel.CommentsText = comment.CommentsText;
                    commentModel.Username = HttpContext.Session.GetString("Username"); // Include the user name from TblUserMasters
                    commentModel.Name = HttpContext.Session.GetString("Username");
                    commentModel.ProfileImage = HttpContext.Session.GetString("ProfileImage");
                    commentModel.Designation = HttpContext.Session.GetString("Designation");
                    //CreatedDate = (DateTime)(joinResult.TblActivity.CreatedDate.HasValue ? (DateTime?)joinResult.TblActivity.CreatedDate.Value : null),
                    // CreatedDate = (DateTime)joinResult.TblActivity.CreatedDate,
                    commentModel.CreatedDate = DateTime.Now;

                    List<ReplyModel> replyModels = new List<ReplyModel>();
                    commentModel.Replies = replyModels;
                    var replys = _context.TblActivityMasters.Where(T => T.UId == T.CommentId).ToList();
                    foreach (var reply in replys)
                    {
                        ReplyModel replyModel = new ReplyModel();
                        replyModel.ReplyCommentId = reply.CommentId;
                        replyModel.ReplyText = reply.ReplyText;
                        replyModel.Username = HttpContext.Session.GetString("Username");
                        replyModels.Add(replyModel);

                    }
                }
                foreach (var query1 in query)
                {
                    if (commentModels != null)
                    {
                        query1.Comments = commentModels;
                    }
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(item => item.Name.ToLower().Contains(searchString.ToLower()) || item.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
                }

                if (sortFilter == "Date")
                {
                    query = query.OrderByDescending(item => item.CreatedDate).ToList();
                }

                // Rest of the code...

                return View(query);
            }


        }


        [HttpPost]
        public async Task<IActionResult> AddComment(/*string userId,*/ string newComment, string PostUid)
        {
            string userId = HttpContext.Session.GetString("User_uid");
            if (userId != null)
            {
                // Create a new activity record with the comment
                var activity = new TblActivityMaster
                {
                    CommentsText = newComment,
                    UserUid = userId,
                    UId = uid,
                    PostId = PostUid,
                    CreatedBy = HttpContext.Session.GetString("User_Mobile"),
                    CreatedDate = DateTime.Now,
                };

                _context.TblActivityMasters.Add(activity);
                await _context.SaveChangesAsync();

                // Return the comment ID
                /*    int commentId = activity.Id;*/


                // Redirect back to the index action with the comment ID as a query parameter
                return RedirectToAction("Index", new { id = PostUid });
            }

            // Redirect back to the index action if there's no new comment
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddReplys(string commentId, string newReply, string postId)
        {

            //string username = HttpContext.Session.GetString("Username");
            //HttpContext.Session.GetString("Designation");
            //HttpContext.Session.GetString("User_uid");
            //HttpContext.Session.GetString("User_Mobile");
            //HttpContext.Session.GetString("ProfileImage");
            string userID = HttpContext.Session.GetString("User_uid");


            if (!string.IsNullOrEmpty(commentId) && !string.IsNullOrEmpty(newReply))
            {
                var reply = new TblReply
                {
                    UId = uid,
                    CommentId = commentId,
                    ReplyText = newReply,
                    UserUid = userID,
                    CreatedDate = DateTime.Now,
                    ReplyDate = DateTime.Now,
                    CreatedBy = HttpContext.Session.GetString("User_Mobile"),
                };

                _context.TblReplys.Add(reply);
                await _context.SaveChangesAsync();
            }
            /*var User = await _context.TblUserMasters
                         .Where(T => T.UId == userID && T.UId != null).ToListAsync();*/

            return RedirectToAction("Index", "HomePage", new { id = postId });
        }

        /*
                public IActionResult DownloadFile(string fileName)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);

                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "application/octet-stream", fileName);
                }*/


        public IActionResult DownloadFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);

            try
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (DirectoryNotFoundException)
            {
                // Handle directory not found error
                // You can return an appropriate response or throw a custom exception
                // For example:
                return NotFound("The specified directory does not exist.");
            }
            catch (FileNotFoundException)
            {
                // Handle file not found error
                // You can return an appropriate response or throw a custom exception
                // For example:
                return NotFound("The specified file does not exist.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // You can log the error or return an appropriate response
                // For example:
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public IActionResult DownloadImage(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            try
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (DirectoryNotFoundException)
            {
                // Handle directory not found error
                // You can return an appropriate response or throw a custom exception
                // For example:
                return NotFound("The specified directory does not exist.");
            }
            catch (FileNotFoundException)
            {
                // Handle file not found error
                // You can return an appropriate response or throw a custom exception
                // For example:
                return NotFound("The specified file does not exist.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // You can log the error or return an appropriate response
                // For example:
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        #endregion

        #region home page
        [Authorize]
        [ServiceFilter(typeof(CustomAuthorizeFilter))]


        public async Task<IActionResult> HomePage(string searchString, string sortFilter, int page = 1)
        {
            int pageSize = 5; // Number of posts to display per page

            var query = _context.TblAddPostDetails
                .Join(
                    _context.TblUserMasters,
                    addPost => addPost.UserUid,
                    user => user.UId,
                    (addPost, user) => new TblAddPost
                    {
                        TblAddPosts = addPost,
                        TblUser = user
                    }
                )
                .Select(joined => new ResultModel
                {
                    CreatedDate = (DateTime)joined.TblAddPosts.CreatedDate,
                    Id = joined.TblUser.Id,
                    UId = joined.TblUser.UId,
                    Name = joined.TblUser.Name,
                    ProfileImage = joined.TblUser.ProfileImage,
                    Designation = joined.TblUser.Designation,
                    ProjectBody = joined.TblAddPosts.ProjectBody,
                    UploadDocument = joined.TblAddPosts.UploadDocument,
                    PostUid = joined.TblAddPosts.UId,
                    Tags = joined.TblAddPosts.Tags,
                    ProjectTitle = joined.TblAddPosts.ProjectTitle,
                    tblpostid = joined.TblAddPosts.UId
                })
                .ToList(); // Fetch the initial project details

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(item => item.Name.ToLower().Contains(searchString.ToLower()) || item.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            if (sortFilter == "option2")
            {
                query = query.OrderByDescending(item => item.CreatedDate).ToList();
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(); // Apply paging

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(query);
        }




        #endregion


    }
}