using System;
using System.Collections.Generic;

namespace Sec_Forum.Models;

public partial class TblDocumentMaster
{
    public int Id { get; set; }

    public string UId { get; set; } = null!;

    public string? Document { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
