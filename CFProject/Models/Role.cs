using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject.Models
{
    public class Role
    {
        /*
         Roles:
        1: Programmer
        2: Manager - Able to create new tasks and link users to them
        3: Admin - Manager + able to add new users
         */



        [Key]
        public int RoleId { get; set; }
        [Column(TypeName = "nvarchar(20)")] 
        [Required(ErrorMessage = "Roles must have a name")]
        public string RoleName { get; set; }
    }
}
