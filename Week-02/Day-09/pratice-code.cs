using System;

// 1. Enum Example
public enum UserRole
{
    Admin,
    Member,
    Guest
}

// 2. Record Example
public record UserProfile(string Username, UserRole Role, string? Bio);

public class Program
{
    // 3. Static Member Example
    private static int totalUsersCreated = 0;

    public static int TotalUsersCreated
    {
        get { return totalUsersCreated; }
    }

    public static void Main()
    {
        // Increment static counter
        totalUsersCreated++;

        // 4. Nullable Types Example
        int? optionalAge = null; // Nullable value type
        string? optionalNickname = "Coder123"; // Nullable reference type

        // Create a record instance
        UserProfile user = new UserProfile("Arhum", UserRole.Admin, "Loves C# programming.");

        // Output results
        Console.WriteLine($"User: {user.Username}");
        Console.WriteLine($"Role: {user.Role}");
        Console.WriteLine($"Bio: {user.Bio ?? "No bio provided"}");
        Console.WriteLine($"Age: {optionalAge.GetValueOrDefault(23)}"); // Fallback to 18
        Console.WriteLine($"Nickname: {optionalNickname}");
        Console.WriteLine($"Total Users (Static Counter): {TotalUsersCreated}");
    }
}
