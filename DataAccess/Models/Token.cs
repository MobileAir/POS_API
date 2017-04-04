using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public partial class Token
    {
        public int TokenId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(250)]
        public string AuthToken { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime IssuedOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ExpiresOn { get; set; }

        public int Request { get; set; }

        public virtual User User { get; set; }
    }
}
