using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        [Required(ErrorMessage = "Companies must have a name")]
        public string Name { get; set; }

    }
}
