using System;
using System.Collections.Generic;

namespace Sec_Forum.Models;

public partial class TblAddPostDetail
{
    public int Id { get; set; }

    public string? UId { get; set; }

    public string? ProjectTitle { get; set; }

    public string? ProjectBody { get; set; }

    public string? UploadDocument { get; set; }

    public string? UploadId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Tags { get; set; }

    public string? Status { get; set; }

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public string? UserUid { get; set; }

    public string? UploadFile { get; set; }
}
