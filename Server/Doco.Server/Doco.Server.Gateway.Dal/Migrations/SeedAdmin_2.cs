using Doco.Server.PasswordEncryption;
using FluentMigrator;

namespace Doco.Server.Gateway.Dal.Migrations;

[Migration(2)]
public sealed class SeedAdmin_2: Migration
{
    public override void Up()
    {
        var password = "admin";
        var (encryptedPass, salt) = PasswordEncryptor.Encrypt(password);
        
        Insert.IntoTable("users")
            .Row(new
            {
                id = Guid.CreateVersion7(), 
                name = "admin",
                email = "admin",
                hashed_password = encryptedPass,
                hash_password_salt = salt,
                is_admin = true
            });
    }

    public override void Down()
    {
        //empty, not used
    }
}