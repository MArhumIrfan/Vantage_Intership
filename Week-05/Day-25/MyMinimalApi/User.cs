using System.ComponentModel.DataAnnotations.Schema;

namespace Lib
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId {get;set;}
        public string Name {get;set;} = string.Empty;
        public string Role {get;set;} = "User";
    }
}