namespace EFTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
