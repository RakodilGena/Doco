using FluentMigrator;

namespace Doco.Server.Gateway.Dal.Migrations;

[Migration(1)]
public sealed class Initial_1 : Migration
{
    // Guid Id
    // string Name
    // string Mail
    // bool IsAdmin
    // timestamptz DeletedAt nullable
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("mail").AsString().NotNullable()
            .WithColumn("hashed_password").AsString().NotNullable()
            .WithColumn("hash_password_salt").AsString().NotNullable()
            .WithColumn("is_admin").AsBoolean().NotNullable()
            .WithColumn("deleted_at").AsCustom("timestamp with time zone").Nullable();

        Create.Index("idx_user_mail")
            .OnTable("users")
            .OnColumn("mail")
            .Ascending()
            .WithOptions()
            .Unique();
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}