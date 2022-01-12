using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject.Models
{
    public class UserTask
    {
        [ForeignKey("User"), Required(ErrorMessage = "This field must be filled")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("Task"), Required(ErrorMessage = "This field must be filled")]
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
