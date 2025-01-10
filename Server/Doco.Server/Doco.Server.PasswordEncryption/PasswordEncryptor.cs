using System.Text;
using Konscious.Security.Cryptography;

namespace Doco.Server.PasswordEncryption;

public static class PasswordEncryptor
{
    public static (byte[] encryptedPassword, byte[] salt) Encrypt(string password)
    {
        var salt = GenerateSalt();
        
        var encryptedPassword = Encrypt(password, salt);
        return (encryptedPassword, salt);
    }
    
    private static byte[] Encrypt(string password, byte[] salt)
    {
        var passwordBytes = GetUTF8Bytes(password);
        
        var argon2 = new Argon2id(passwordBytes);
        argon2.DegreeOfParallelism = 1;
        argon2.MemorySize = 19456;//19mib
        argon2.Iterations = 2;
        argon2.Salt = salt;

        var hash = argon2.GetBytes(128);
        return hash;
    }

    public static bool ComparePasswords(
        string inputPassword,
        byte[] encryptedPassword,
        byte[] passwordSalt)
    {
        var inputEncrypted = Encrypt(inputPassword, passwordSalt);
        
        return inputEncrypted.AsSpan().SequenceEqual(encryptedPassword);
    }

    private static byte[] GenerateSalt()
    {
        var guid = Guid.NewGuid();
        var bytes = guid.ToByteArray();
        return bytes;
    }

    // ReSharper disable once InconsistentNaming
    private static byte[] GetUTF8Bytes(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }
}