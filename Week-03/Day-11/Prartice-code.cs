using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // 1. Initialization
        List<string> fruits = new List<string>();                  // Empty list
        List<int> numbers = new List<int> { 10, 20, 30 };          // Collection initializer
        List<string> names = "Alice"; "Bob"; "Charlie";          // C# 12+ collection expression

        // 2. Adding Elements
        fruits.Add("Apple");                                       // Adds to the end
        fruits.Add("Banana");
        fruits.AddRange(new[] { "Cherry", "Date" });               // Adds a collection
        fruits.Insert(1, "Blueberry");                             // Inserts at index 1

        // 3. Accessing Elements
        string firstFruit = fruits[0];                             // Zero-based index access
        int totalItems = fruits.Count;                             // Gets total element count

        // 4. Searching & Checking
        bool hasApple = fruits.Contains("Apple");                  // Returns true
        int index = fruits.IndexOf("Banana");                      // Returns index or -1

        // 5. Removing Elements
        fruits.Remove("Banana");                                   // Removes first match
        fruits.RemoveAt(0);                                        // Removes item at index 0

        // 6. Iterating
        foreach (var fruit in fruits)
        {
            Console.WriteLine(fruit);
        }

        // 7. Utility Methods
        fruits.Sort();                                             // Sorts elements alphabetically/numerically
        fruits.Clear();                                            // Removes all elements
    }
}
