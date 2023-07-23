using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sec_Forum.Models;

public partial class SecForumContext : DbContext
{
    public SecForumContext()
    {
    }

    public SecForumContext(DbContextOptions<SecForumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblActivityMaster> TblActivityMasters { get; set; }

    public virtual DbSet<TblAddPostDetail> TblAddPostDetails { get; set; }

    public virtual DbSet<TblDocumentMaster> TblDocumentMasters { get; set; }

    public virtual DbSet<TblOrganizationDetail> TblOrganizationDetails { get; set; }

    public virtual DbSet<TblReply> TblReplys { get; set; }

    public virtual DbSet<TblRoleMaster> TblRoleMasters { get; set; }

    public virtual DbSet<TblSystemMaster> TblSystemMasters { get; set; }

    public virtual DbSet<TblUserMaster> TblUserMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Data Source=5.182.33.48;Port=3307;Initial Catalog=sec_forum;User ID=dbadmin;Password=#Aispl#A112023;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblActivityMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_activity_master");

            entity.HasIndex(e => e.UId, "seq_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommentDate)
                .HasColumnType("timestamp")
                .HasColumnName("comment_date");
            entity.Property(e => e.CommentId)
                .HasMaxLength(255)
                .HasColumnName("comment_id");
            entity.Property(e => e.CommentsText).HasColumnName("comments_text");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.Dislike)
                .HasMaxLength(45)
                .HasColumnName("dislike");
            entity.Property(e => e.Likes)
                .HasMaxLength(45)
                .HasColumnName("likes");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.PostId)
                .HasMaxLength(250)
                .HasColumnName("post_id");
            entity.Property(e => e.ReplyDate)
                .HasColumnType("timestamp")
                .HasColumnName("reply_date");
            entity.Property(e => e.ReplyText)
                .HasColumnType("text")
                .HasColumnName("reply_text");
            entity.Property(e => e.ReplyUid)
                .HasMaxLength(255)
                .HasColumnName("reply_uid");
            entity.Property(e => e.Share)
                .HasMaxLength(45)
                .HasColumnName("share");
            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UserUid)
                .HasMaxLength(255)
                .HasColumnName("user_uid");
        });

        modelBuilder.Entity<TblAddPostDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_add_post_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.LongDescription)
                .HasMaxLength(255)
                .HasColumnName("long_description");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.ProjectBody).HasColumnName("project_body");
            entity.Property(e => e.ProjectTitle)
                .HasMaxLength(455)
                .HasColumnName("project_title");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(255)
                .HasColumnName("short_description");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
            entity.Property(e => e.Tags)
                .HasMaxLength(45)
                .HasColumnName("tags");
            entity.Property(e => e.UId)
                .HasMaxLength(255)
                .HasColumnName("u_id");
            entity.Property(e => e.UploadDocument)
                .HasMaxLength(1000)
                .HasColumnName("upload_document");
            entity.Property(e => e.UploadFile)
                .HasMaxLength(1000)
                .HasColumnName("upload_file");
            entity.Property(e => e.UploadId)
                .HasMaxLength(45)
                .HasColumnName("upload_id");
            entity.Property(e => e.UserUid)
                .HasMaxLength(255)
                .HasColumnName("user_uid");
        });

        modelBuilder.Entity<TblDocumentMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_document_master");

            entity.HasIndex(e => e.UId, "seq_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.Document)
                .HasMaxLength(45)
                .HasColumnName("document");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.UId).HasColumnName("u_id");
        });

        modelBuilder.Entity<TblOrganizationDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_organization_details");

            entity.HasIndex(e => e.UId, "seq_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.District)
                .HasMaxLength(45)
                .HasColumnName("district");
            entity.Property(e => e.EmailId)
                .HasMaxLength(45)
                .HasColumnName("email_id");
            entity.Property(e => e.FieldName)
                .HasMaxLength(45)
                .HasColumnName("field_name");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.OfficeAddress)
                .HasMaxLength(255)
                .HasColumnName("office_address");
            entity.Property(e => e.OrgCode)
                .HasMaxLength(45)
                .HasColumnName("org_code");
            entity.Property(e => e.OrgName)
                .HasMaxLength(45)
                .HasColumnName("org_name");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(15)
                .HasColumnName("phone_no");
            entity.Property(e => e.RoleId)
                .HasMaxLength(255)
                .HasColumnName("role_id");
            entity.Property(e => e.State)
                .HasMaxLength(45)
                .HasColumnName("state");
            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<TblReply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_replys");

            entity.HasIndex(e => e.UId, "seq_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommentId)
                .HasMaxLength(255)
                .HasColumnName("comment_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.ReplyDate)
                .HasColumnType("timestamp")
                .HasColumnName("reply_date");
            entity.Property(e => e.ReplyText).HasColumnName("reply_text");
            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UserUid)
                .HasMaxLength(255)
                .HasColumnName("user_uid");
        });

        modelBuilder.Entity<TblRoleMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_role_master");

            entity.HasIndex(e => e.UId, "seq_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.RoleName)
                .HasMaxLength(45)
                .HasColumnName("role_name");
            entity.Property(e => e.UId).HasColumnName("u_id");
        });

        modelBuilder.Entity<TblSystemMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_system_master");

            entity.HasIndex(e => e.UId, "seq_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.UId).HasColumnName("u_id");
        });

        modelBuilder.Entity<TblUserMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbl_user_master");

            entity.HasIndex(e => e.UId, "seq_id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConfirmPassword)
                .HasMaxLength(45)
                .HasColumnName("confirmPassword");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp")
                .HasColumnName("created_date");
            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(45)
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Designation)
                .HasMaxLength(45)
                .HasColumnName("designation");
            entity.Property(e => e.EducationalQualification)
                .HasMaxLength(155)
                .HasColumnName("educational_qualification");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.FromDate)
                .HasColumnType("timestamp")
                .HasColumnName("from_date");
            entity.Property(e => e.Languages)
                .HasMaxLength(155)
                .HasColumnName("languages");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(15)
                .HasColumnName("mobile_number");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(45)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp")
                .HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.OrgId)
                .HasMaxLength(255)
                .HasColumnName("org_id");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.ProfileImage)
                .HasMaxLength(255)
                .HasColumnName("profile_image");
            entity.Property(e => e.RoleId)
                .HasMaxLength(255)
                .HasColumnName("role_id");
            entity.Property(e => e.ToDate)
                .HasColumnType("timestamp")
                .HasColumnName("to_date");
            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UploadDocument)
                .HasMaxLength(255)
                .HasColumnName("upload_document");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
