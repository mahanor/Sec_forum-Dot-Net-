using System;
using System.Collections.Generic;

namespace Sec_Forum.Models;

public partial class TblReply
{
    public int Id { get; set; }

    public string? UId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? CommentId { get; set; }

    public string? ReplyText { get; set; }

    public DateTime? ReplyDate { get; set; }

    public string? UserUid { get; set; }
}
