using System;


public enum UserRole
{
    Admin,
    Member,
    Guest
}


public record UserProfile(string Username, UserRole Role, string? Bio);

public class Program
{
   
    private static int totalUsersCreated = 0;

    public static int TotalUsersCreated
    {
        get { return totalUsersCreated; }
    }

    public static void Main()
    {
        
        totalUsersCreated++;

       
        int? optionalAge = null; 
        string? optionalNickname = "Coder123"; 

        UserProfile user = new UserProfile("Arhum", UserRole.Admin, "Loves C# programming.");

       
        Console.WriteLine($"User: {user.Username}");
        Console.WriteLine($"Role: {user.Role}");
        Console.WriteLine($"Bio: {user.Bio ?? "No bio provided"}");
        Console.WriteLine($"Age: {optionalAge.GetValueOrDefault(23)}"); 
        Console.WriteLine($"Nickname: {optionalNickname}");
        Console.WriteLine($"Total Users (Static Counter): {TotalUsersCreated}");
    }
}
