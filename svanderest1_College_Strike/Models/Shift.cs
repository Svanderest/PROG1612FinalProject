using System;
using System.ComponentModel.DataAnnotations;

namespace svanderest1_College_Strike.Models
{
    public class Shift : Auditable
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Shift date is required.")]
        public DateTime Date { get; set; }

        [Display(Name = "Member")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an member.")]
        public int MemberID { get; set; }
        public virtual Member Member { get; set; }

        [Display(Name = "Assignment")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an assignment.")]
        public int AssignmentID { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}