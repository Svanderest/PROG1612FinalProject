namespace svanderest1_College_Strike.Models
{
    public class MemberPosition
    {
        public int MemberID { get; set; }
        public virtual Member Member { get; set; }

        public int PositionID { get; set; }
        public virtual Position Position { get; set; }
    }
}