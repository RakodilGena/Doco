using System.Data;
using FluentMigrator;
using FluentMigrator.Postgres;

namespace Doco.Server.FileService.Dal.Migrations;

[Migration(1)]
public sealed class Initial_1 : Migration
{
    // folders:
    // Guid Id
    // string Name
    // timestamptz createdAt
    // Guid creator
    // Guid? nullable parentId - if null then root
    public override void Up()
    {
        #region folders

        Create.Table("folders")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString().NotNullable()

            .WithColumn("created_at").AsCustom("timestamp with time zone")
            .NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("creator_id").AsGuid().NotNullable()
            .WithColumn("parent_folder_id").AsGuid().Nullable()

            .WithColumn("path").AsCustom("uuid[]").NotNullable();

        //one user can't have folders at same level with equal names
        //user here is required since
        //users can have root folders with no parent with equal names
        Create.UniqueConstraint("constraint_folder_name_parent_creator")
            .OnTable("folders")
            .Columns("name", "parent_folder_id", "creator_id");

        Create.Index()
            .OnTable("folders")
            .OnColumn("name")
            .Ascending();

        Create.Index()
            .OnTable("folders")
            .OnColumn("creator_id")
            .Ascending();

        Create.ForeignKey("FK_folders_folder_to_parent")
            .FromTable("folders").ForeignColumn("parent_folder_id")
            .ToTable("folders").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);

        #endregion

        #region folder permissions

        Create.Table("folder_permissions")
            .WithColumn("folder_id").AsGuid().NotNullable()
            .WithColumn("permitted_user_id").AsGuid().NotNullable()

            .WithColumn("creator_id").AsGuid().NotNullable()
            .WithColumn("created_at").AsCustom("timestamp with time zone")
            .NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);


        Create.PrimaryKey("PK_folder_permissions_id")
            .OnTable("folder_permissions")
            .Columns("folder_id", "permitted_user_id");

        Create.ForeignKey("FK_folder_permissions_folders_folder_id")
            .FromTable("folder_permissions").ForeignColumn("folder_id")
            .ToTable("folders").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);

        #endregion


        #region files

        // Guid Id
        // string Name
        // string contentType
        // int sizeBytes
        //
        // string nullable pathToFile - if on disk
        // fk Guid fileAtDb nullable - if in db
        //
        //     timestamptz createdAt
        //     Guid creator
        //     timestamptz lastAccessedAt (by download)
        // fk Guid? folderId - if null then root
        // Guid[] materializedPath


        Create.Table("files")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("content_type").AsString().NotNullable()
            .WithColumn("size_bytes").AsInt32().NotNullable()

            .WithColumn("path_to_file").AsString().Nullable()

            .WithColumn("created_at").AsCustom("timestamp with time zone")
            .NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("creator_id").AsGuid().NotNullable()

            .WithColumn("last_accessed_at").AsCustom("timestamp with time zone")

            .WithColumn("folder_id").AsGuid().NotNullable()

            .WithColumn("path").AsCustom("uuid[]").NotNullable();

        //folder can't have files with equal names
        Create.UniqueConstraint("constraint_file_name_folder")
            .OnTable("files")
            .Columns("name", "folder_id");

        Create.Index()
            .OnTable("files")
            .OnColumn("name")
            .Ascending();

        Create.Index()
            .OnTable("files")
            .OnColumn("creator_id")
            .Ascending();

        Create.ForeignKey("FK_files_folder_folder_id")
            .FromTable("files").ForeignColumn("folder_id")
            .ToTable("folders").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);

        #endregion

        #region file bytes in db

        Create.Table("files_in_db")
            .WithColumn("file_id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("data").AsBinary().NotNullable();

        Create.ForeignKey("FK_files_in_db_files_file_id")
            .FromTable("files_in_db").ForeignColumn("file_id")
            .ToTable("files").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);

        #endregion

        #region file permissions

        Create.Table("file_permissions")
            .WithColumn("file_id").AsGuid().NotNullable()
            .WithColumn("permitted_user_id").AsGuid().NotNullable()

            .WithColumn("creator_id").AsGuid().NotNullable()
            .WithColumn("created_at").AsCustom("timestamp with time zone")
            .NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);


        Create.PrimaryKey("PK_file_permissions_id")
            .OnTable("file_permissions")
            .Columns("file_id", "permitted_user_id");

        Create.ForeignKey("FK_file_permissions_files_file_id")
            .FromTable("file_permissions").ForeignColumn("file_id")
            .ToTable("files").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);

        #endregion
    }

    public override void Down()
    {
        Delete.Table("folder_permissions");
        Delete.Table("folders");

        Delete.Table("file_permissions");
        Delete.Table("files");
    }
}