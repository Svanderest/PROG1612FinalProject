using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcFinalProject.ViewModels
{
    public class AssignedPositions
    {
        public int PositionID { get; set; }
        public string PositionTitle { get; set; }
        public bool Assigned { get; set; }
    }
}
