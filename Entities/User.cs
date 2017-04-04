namespace Entities
{
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Tokens = new HashSet<Token>();
        }
    
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int RequestAllowed { get; set; }
        public bool IsSuperUser { get; set; }
    
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
