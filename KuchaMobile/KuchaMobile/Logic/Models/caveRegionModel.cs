namespace KuchaMobile.Logic.Models
{
    public class CaveRegionModel
    {
        /// <summary>
        /// This model represents a CaveRegion
        /// </summary>
        public int regionID { get; set; }
        public string phoneticName { get; set; }
        public string originalName { get; set; }
        public string englishName { get; set; }
        public int siteID { get; set; }
        public bool openAccess { get; set; }
    }
}