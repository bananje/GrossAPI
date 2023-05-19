namespace GrossAPI.Utils
{
    public class Crypter
    {
        public static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return isPasswordValid;
        }
    }
}
