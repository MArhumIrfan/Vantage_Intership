using System;
using System.ComponentModel.DataAnnotations; // Add this namespace

namespace Lib
{
    public class HistoryEntry
    {
        [Key] 
        public int HistoryId { get; set; }
        public string Action { get; set; } = string.Empty;   
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
