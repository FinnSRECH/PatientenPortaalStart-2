using System.Security.Cryptography;

namespace Hospital.PatientPortal.Services;

public class PasswordService
{
    public (string Hash, string Salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool Verify(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var attemptedHash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(attemptedHash, Convert.FromBase64String(hash));
    }
}
