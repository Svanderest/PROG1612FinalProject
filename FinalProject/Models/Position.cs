using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Position : Auditable
    {
        public int ID { get; set; }

        [Required (ErrorMessage = "You cannot leave the title blank.")]
        [StringLength (50, ErrorMessage = "Ttile cannot be more than 50 characters long.")]
        public string Title { get; set; }

        public virtual ICollection<MemberPosition> Members { get; set; }
    }
}
