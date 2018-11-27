using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcFinalProject.Models
{
    public class MemberPosition
    {
        public int MemberID { get; set; }
        public virtual Member Member { get; set; }

        public int PositionID { get; set; }
        public virtual Position Position { get; set; }
    }
}
