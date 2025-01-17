﻿using FluentMigrator;

namespace Doco.Server.Gateway.Dal.Migrations;

[Migration(1)]
public sealed class Initial_1 : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("email").AsString().NotNullable()
            .WithColumn("hashed_password").AsBinary().NotNullable()
            .WithColumn("hash_password_salt").AsBinary().NotNullable()
            .WithColumn("is_admin").AsBoolean().NotNullable()
            .WithDefaultValue(false)

            .WithColumn("created_at").AsCustom("timestamp with time zone")
            .NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("deleted_at").AsCustom("timestamp with time zone").Nullable();

        Create.Index("idx_user_email")
            .OnTable("users")
            .OnColumn("email")
            .Ascending()
            .WithOptions().Unique();
        
        Create.Index("idx_user_created_at")
            .OnTable("users")
            .OnColumn("created_at")
            .Descending();
        
        Create.UniqueConstraint("constraint_user_name")
            .OnTable("users")
            .Columns("name");
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}