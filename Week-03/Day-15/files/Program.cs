/*
--
--UserNames & Password for all types of users--
--
--Admin: UserName: "Admin", Password: "admin123".--
--
--User: Member ID required (e.g., 39393), Password: "User123".--
--
--Guest: No UserName or Password Required, just have to be 18 and above.--
--
*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace Lib
{
    class Program
    {
        static void Main(string[] args)
        {
            UI.RegisteredUsers.Add(39393, "Muhammad Arhum Irfan");
            UI.RegisteredUsers.Add(39425, "Ghayyur Abbas");
            UI.RegisteredUsers.Add(40142, "Wazir Muzzamil Hussain");
            UI.RegisteredUsers.Add(39358, "Muhammad Whahaj");
            UI.RegisteredUsers.Add(39859, "Insfalullah Khan");

            LibraryDatabase.LoadFromFile();

            while (true)
            {
                UI.ClearAndHeader("Library Management System");

                UI.DisplayRandomQuote();

                Console.WriteLine("Identify your role (Admin/User/Guest) or type -'exit'- to quit: ");
                string inputRole = Console.ReadLine() ?? "";

                if (inputRole.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    LibraryDatabase.SaveToFile();
                    UI.PrintWarning("Closing execution thread session. Goodbye!");
                    Thread.Sleep(1000);
                    break;
                }

                SystemRole chosenRole = SystemRole.Unknown;
                try
                {
                    if (string.IsNullOrWhiteSpace(inputRole) || !Enum.TryParse<SystemRole>(inputRole, true, out chosenRole) || chosenRole == SystemRole.Unknown)
                    {
                        throw new ArgumentException("Invalid Role selection format. Please try Admin, User, or Guest");
                    }
                }
                catch (ArgumentException ex)
                {
                    UI.PrintError("[Security Exception Caught]" + ex.Message);
                    UI.Pause();
                    continue;
                }

                Login userSession = null;

                if (chosenRole == SystemRole.Admin)
                {
                    try
                    {
                        VerifyAdmin admin = new VerifyAdmin();
                        Console.WriteLine("\n Enter the Admin Username ");
                        string adminUser = Console.ReadLine() ?? "";

                        if (string.IsNullOrWhiteSpace(adminUser)) throw new ArgumentException("Admin Username cannot be empty!");
                        admin.AdminUserName = adminUser;

                        Console.Write("Enter the Admin Password: ");
                        string adminPass = Console.ReadLine() ?? "";
                        if (string.IsNullOrWhiteSpace(adminPass)) throw new ArgumentException("Admin password cannot be empty.");
                        admin.AdminPassword = adminPass;

                        Thread.Sleep(500);
                        userSession = admin;
                    }
                    catch (Exception ex)
                    {
                        UI.PrintError("Admin Authentication halted" + ex.Message);
                        UI.Pause();
                        continue;
                    }
                }
                else if (chosenRole == SystemRole.User)
                {
                    try
                    {
                        int memberId = UI.ReadInt32("\nEnter your Member ID ");

                        if (UI.RegisteredUsers.ContainsKey(memberId))
                        {
                            VerifyUser user = new VerifyUser();
                            user.Username = UI.RegisteredUsers[memberId];

                            UI.PrintSuccess("Welcome!, " + user.Username + " ! ");
                            Console.WriteLine("Enter the passowrd ");
                            string userPass = Console.ReadLine() ?? "";
                            if (string.IsNullOrWhiteSpace(userPass)) throw new ArgumentException("Password cannot be empty! ");
                            user.UserPassword = userPass;

                            Thread.Sleep(500);
                            userSession = user;
                        }
                        else
                        {
                            throw new KeyNotFoundException("Member ID [" + memberId + "] not recoginzed in the system.");
                        }
                    }
                    catch (Exception ex)
                    {
                        UI.PrintError("User login error " + ex.Message);
                        UI.Pause();
                        continue;
                    }
                }
                else if (chosenRole == SystemRole.Guest)
                {
                    GuestUser guest = new GuestUser();
                    guest.Age = UI.ReadInt32("\nEnter your age to proceed: ");

                    Thread.Sleep(500);
                    userSession = guest;
                }
                else
                {
                    UI.PrintError("Invalid system chosen! Try again.");
                    UI.Pause();
                    continue;
                }

                if (userSession != null)
                {
                    userSession.ExecuteRoleActions();
                }
            }
        }
    }
}
