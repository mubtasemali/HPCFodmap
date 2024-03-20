namespace HPCFodmapProject.Server.Models
{
    public class Ingredients
    {
        public int IngredientsID { get; set; }
        public string IngredientsName { get; set;}
        public int severity { get; set; }
        //says bit in db not sure if this should be bool
        public bool inFodMap { get; set; }
    }
}
