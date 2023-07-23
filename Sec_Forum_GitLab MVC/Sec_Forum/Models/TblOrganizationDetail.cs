using System;
using System.Collections.Generic;

namespace Sec_Forum.Models;

public partial class TblOrganizationDetail
{
    public int Id { get; set; }

    public string UId { get; set; } = null!;

    public string? OrgName { get; set; }

    public string? OrgCode { get; set; }

    public string? FieldName { get; set; }

    public string? OfficeAddress { get; set; }

    public string? EmailId { get; set; }

    public string? PhoneNo { get; set; }

    public string? RoleId { get; set; }

    public string? UserId { get; set; }

    public string? State { get; set; }

    public string? District { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
