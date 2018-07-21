namespace KuchaMobile.Logic.Models
{
    public class CaveSiteModel
    {
        /// <summary>
        /// This model represents a Cave Site
        /// </summary>
        public int siteID { get; set; }
        public string name { get; set; }
        public string alternativeName { get; set; }
        public string shortName { get; set; }
        public bool openAccess { get; set; }
    }
}