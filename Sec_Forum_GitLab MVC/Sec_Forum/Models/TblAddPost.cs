using System;
using System.Collections.Generic;
namespace Sec_Forum.Models

{
    public class TblAddPost
    {
        public TblUserMaster TblUser { get; set; }
        public TblAddPostDetail TblAddPosts { get; set; }
        public TblReply TblReply { get; set; }
        public TblActivityMaster TblActivity { get; set; }


    }

    public class ResultModel
    {
        public int Id { get; set; }

        public string? UId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ProjectBody { get; set; }
        public string? ProjectTitle { get; set; }
        public string? tblpostid { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? ProfileImage { get; set; }
        public string? Designation { get; set; }
        public string? Likes { get; set; }

        public string? PostUid { get; set; }

        public string? Tags { get; set; }
        public string? Share { get; set; }
        public string? PostId { get; set; }

        /*  public int? CommentId { get; set; }*/

        public DateTime? CommentDate { get; set; }

        public string? ReplyId { get; set; }

        public string? ReplyText { get; set; }

        public DateTime? ReplyDate { get; set; }
        public string? ProjectImage { get; set; }
        public string? UploadDocument { get; set; }
        public string? UploadFile { get; set; }

        /*  public string? CommentsText { get; set; }*/

        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }


    public class CommentModel
    {

        public string? Name { get; set; }
        public string? ProfileImage { get; set; }
        public string? Designation { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? Username { get; set; }
        public string? commentId { get; set; }
        public string? PostId { get; set; }
        public string? CommentsText { get; set; }
        public string? tblpostid { get; set; }
        public List<ReplyModel> Replies { get; set; } = new List<ReplyModel>();
    }


    public class ReplyModel
    {
        public string? ReplyCommentId { get; set; }
        public string? Username { get; set; }
        public string? ReplyText { get; set; }
        public string? Name { get; set; }
        public string? ProfileImage { get; set; }
        public string? Designation { get; set; }
        public DateTime CreatedDate { get; set; }

    }

}