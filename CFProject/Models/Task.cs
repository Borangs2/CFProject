using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        [Column(TypeName = "nvarchar(40)")]
        [Required(ErrorMessage = "Tasks must have a title")]
        public string Title { get; set; }
        public string Description { get; set; }
        
        [ForeignKey("Company")]
        [Required(ErrorMessage = "Tasks must have a company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
