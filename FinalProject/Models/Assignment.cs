using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Assignment : Auditable
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Assignment name is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 200 characters.")]
        public string Name { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public virtual ICollection<Shift> Shifts { get; set; }
    }
}