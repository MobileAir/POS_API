using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public partial class User
    {
        
        public User()
        {
            Tokens = new HashSet<Token>();
        }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(64)]
        public string Salt { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public int RequestAllowed { get; set; }

        public bool IsSuperUser { get; set; }
        
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
