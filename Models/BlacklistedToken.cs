// Models/BlacklistedToken.cs
namespace LibrarySystem.Models
{
    public class BlacklistedToken
    {
        public string JwtId { get; set; } = string.Empty; // Matches 'jti' claim
        public DateTime Expiration { get; set; } // When the token would naturally expire
    }
}