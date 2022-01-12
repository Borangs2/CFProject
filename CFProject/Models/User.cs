using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Required(ErrorMessage = "First name missing")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Required(ErrorMessage = "Last name missing")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Password missing")]
        public string Password { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; } //Nullable
        public virtual User Manager { get; set; }

        [ForeignKey("Role")]
        [Required(ErrorMessage = "Role missing")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
