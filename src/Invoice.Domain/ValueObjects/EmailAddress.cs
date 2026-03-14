using System.Text.RegularExpressions;
namespace Invoice.Domain.ValueObjects;

public sealed record EmailAddress 
{
    private static readonly Regex EmailRegex = new(  // for validating email format
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", // a common regex pattern. It checks for a sequence of allowed characters before the @ symbol, followed by a valid domain name and a top-level domain.
        RegexOptions.Compiled | RegexOptions.IgnoreCase); // Compiled for performance and IgnoreCase to allow uppercase letters in email addresses

        public string Value { get; }

        private EmailAddress(string value) => Value = value.ToLowerInvariant(); // store email in lowercase for consistency

        public static EmailAddress Of(string email) // factory method to create EmailAddress instances
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException($"'{email}' is not a valid email address.", nameof(email));

            return new EmailAddress(email);
        }

        public static implicit operator string(EmailAddress email) => email.Value; // allow implicit conversion to string 
        public override string ToString() => Value; // To make email readable

}