namespace Doco.Server.Gateway.Dal.Descriptions.Users;

public static class UserDbDescription
{
    /// <summary>
    /// users
    /// </summary>
    public const string Table = "users";
    
    /// <summary>
    /// id
    /// </summary>
    public const string Id = "id";
    
    /// <summary>
    /// name
    /// </summary>
    public const string Name = "name";
    
    /// <summary>
    /// email
    /// </summary>
    public const string Email = "email";
    
    /// <summary>
    /// hashed_password
    /// </summary>
    public const string HashedPassword = "hashed_password";
    
    /// <summary>
    /// hash_password_salt
    /// </summary>
    public const string HashPasswordSalt = "hash_password_salt";
    
    /// <summary>
    /// is_admin
    /// </summary>
    public const string IsAdmin = "is_admin";
    
    /// <summary>
    /// created_at
    /// </summary>
    public const string CreatedAt = "created_at";
    
    /// <summary>
    /// deleted_at
    /// </summary>
    public const string DeletedAt = "deleted_at";

    /// <summary>
    /// idx_user_email
    /// </summary>
    public const string IdxEmail = "idx_user_email";

    /// <summary>
    /// constraint_user_name
    /// </summary>
    public const string ConstrName = "constraint_user_name";
}