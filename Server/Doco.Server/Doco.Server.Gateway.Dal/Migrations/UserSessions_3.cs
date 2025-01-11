using System.Data;
using FluentMigrator;

namespace Doco.Server.Gateway.Dal.Migrations;

[Migration(3)]
public sealed class UserSessions_3: Migration
{
    public override void Up()
    {
        Create.Table("user_sessions")
            .WithColumn("user_id").AsGuid().NotNullable()
            .WithColumn("refresh_token").AsString().NotNullable()
            .WithColumn("jwt_token").AsString().NotNullable();
        
        Create.PrimaryKey("PK_user_sessions_Id")
            .OnTable("user_sessions")
            .Columns("user_id", "refresh_token");
        
        Create.ForeignKey("FK_user_sessions_users_user_id")
            .FromTable("user_sessions").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id")
            .OnDeleteOrUpdate(Rule.Cascade);
        
        Create.Index("idx_user_sessions_jwt_token")
            .OnTable("user_sessions")
            .OnColumn("jwt_token")
            .Ascending();
    }

    public override void Down()
    {
        Delete.Table("user_sessions");
    }
}