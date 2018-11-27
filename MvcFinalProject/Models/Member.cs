using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcFinalProject.Models
{
    public class Member : Auditable
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string FirstName { get; set; }        

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(50, ErrorMessage = "Last name cannot be more than 50 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 Phone { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string eMail { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="You must select an assignment")]
        [Display(Name = "Assignment")]
        public int AssignmentID { get; set; }
        public virtual Assignment Assignment { get; set; }

        public virtual ICollection<Shift> Shifts { get; set; }

        [Display(Name = "Positions")]
        public virtual ICollection<MemberPosition> Positions { get; set; }

        [Display(Name = "Member")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }        
    }
}
