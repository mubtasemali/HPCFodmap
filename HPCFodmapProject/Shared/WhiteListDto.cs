namespace HPCFodmapProject.Server.Models
{
    public class WhiteList
    {
        public int UserID { get; set; }
        public int IngredientsID { get; set; }
        //says binary in ERD not sure if this should be bool or not
        public bool userIsAffected { get; set; }
    }
}
