namespace KuchaMobile.Logic.Models
{
    public class CaveDistrictModel
    {
        /// <summary>
        /// This model represents a Cave District
        /// </summary>
        public int districtID { get; set; }
        public string name { get; set; }
        public int siteID { get; set; }
        public string description { get; set; }
        public string map { get; set; }
        public string arialMap { get; set; }
        public bool openAccess { get; set; }
    }
}