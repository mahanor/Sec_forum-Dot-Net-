using System;
using System.Collections.Generic;

namespace Sec_Forum.Models;

public partial class TblActivityMaster
{
    public int Id { get; set; }

    public string? UId { get; set; }

    public string? Likes { get; set; }

    public string? CommentsText { get; set; }

    public string? Share { get; set; }

    public string? Dislike { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? PostId { get; set; }

    public string? CommentId { get; set; }

    public DateTime? CommentDate { get; set; }

    public string? ReplyUid { get; set; }

    public string? ReplyText { get; set; }

    public DateTime? ReplyDate { get; set; }

    public string? UserUid { get; set; }
}
