using System;
using System.Collections.Generic;

namespace Sec_Forum.Models;

public partial class TblUserMaster
{
    public int Id { get; set; }

    public string? UId { get; set; }

    public string? OrgId { get; set; }

    public string? RoleId { get; set; }

    public string? Name { get; set; }

    public string? MobileNumber { get; set; }

    public string? Email { get; set; }

    public string? Designation { get; set; }

    public string? CreatedBy { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Languages { get; set; }

    public string? EducationalQualification { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? UploadDocument { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }
}
