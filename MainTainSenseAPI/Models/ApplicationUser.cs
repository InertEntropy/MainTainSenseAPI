using Microsoft.AspNetCore.Identity;

namespace MainTainSenseAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        //DEFAULTS FROM IDENTITYUSER - FOR INFORMATION PURPOSES
        // Id: A unique identifier for the user(usually a GUID or string).
        //UserName: The user's login username.
        //NormalizedUserName: Uppercase version of UserName for case-insensitive lookups.
        //Email: The user's email address.
        //NormalizedEmail: Uppercase version of Email for case-insensitive lookups.
        //EmailConfirmed: Indicates if the email has been verified.
        //PasswordHash: Stores a securely hashed representation of the user's password.
        //SecurityStamp: A random value that changes whenever the user's password is modified or critical security information changes. Used for tracking and invalidating authentication tokens.
        //ConcurrencyStamp: A value that is updated anytime the user record is modified.Helps with optimistic concurrency handling.
        //PhoneNumber: The user's phone number.
        //PhoneNumberConfirmed: Indicates if the phone number has been confirmed.
        //TwoFactorEnabled: Indicates if two-factor authentication is enabled for the user.
        //LockoutEnd: A DateTimeOffset used to track account lockout dates, if enabled.
        //LockoutEnabled: Indicates if the account lockout feature is enabled for the user.
        //AccessFailedCount: Tracks the number of failed login attempts.

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int JobTitleId { get; set; }
        public JobTitle? JobTitle { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
