using System.ComponentModel.DataAnnotations;

namespace Invoice.Application.Common.Settings;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";  // This constant is used to identify the section in the configuration file (e.g., appsettings.json) where the JWT settings are defined. It allows for easy retrieval of these settings when configuring JWT authentication in the application.

    [Required]
    public string Key { get; init; } = string.Empty; // The secret key used to sign the JWT tokens. It should be a long, random string to ensure the security of the tokens.

    [Required]
    public string Issuer { get; init; } = string.Empty; // The issuer of the JWT token, typically the name of the application or service that generates the token.

    [Required]
    public string Audience { get; init; } = string.Empty; // The intended audience for the JWT token, usually the name of the application or service that will consume the token.

    public int AccessTokenExpiryMinutes { get; init; } = 30; // The expiration time for access tokens in minutes. After this time, the token will no longer be valid and the user will need to re-authenticate to obtain a new token.

    public int RefreshTokenExpiryDays { get; init; } = 7; // The expiration time for refresh tokens in days. Refresh tokens are used to obtain new access tokens without requiring the user to re-authenticate, and they typically have a longer lifespan than access tokens.
}


// get; -> anyone can read 
// set; -> anyone can write anytime
// private set; -> only this class can write
// protected set; -> only this class and subclasses can write
// init; -> anyone can write but only during object construction — frozen after that


/*
public string Name { get; set; }           // anyone can read, anyone can write anytime
public string Name { get; private set; }   // anyone can read, only this class can write
public string Name { get; protected set; } // anyone can read, only this class and subclasses can write
public string Name { get; init; }          // anyone can read, anyone can write but only during object construction — frozen after that
public string Name { get; }                // anyone can read, only settable in constructor or field initializer — truly readonly
*/

/* Diffrence between get; set; and get; init; is that get; set; allows the property to be modified at any time after the object is created, while get; init; allows the property to be set only during object initialization (e.g., in a constructor or object initializer) and prevents further modifications after that. This makes get; init; ideal for immutable objects where you want to ensure that the properties cannot be changed once the object is created. */



/*************** Difference between get; init; and get;

// get; only — must be set inside the class constructor
public class Person
{
    public string Name { get; }

    public Person(string name)
    {
        Name = name; // only valid place to set it
    }
}

var p = new Person("Alice");        // works
var p = new Person { Name = "Alice" }; // compile error — can't set from outside

get; — the class controls what value goes in, through its own constructor. Callers cannot choose. Used when the class needs to compute or validate the value itself.



// get; init — set from outside using object initializer syntax
public class Person
{
    public string Name { get; init; }
}

var p = new Person { Name = "Alice" }; // works
p.Name = "Bob";                         // compile error — frozen after construction

get; init; — the caller chooses the value at construction time, then it freezes. Used when you want the flexibility of object initializer syntax but still want immutability after that. This is why your JwtSettings uses it

*/
