namespace KuchaMobile.Logic.Models
{
    public class IconographyModel
    {
        /// <summary>
        /// This model represents an iconography
        /// </summary>
        public int iconographyID { get; set; }
        public int parentID { get; set; }
        public string text { get; set; }
        public bool openAccess { get; set; }
    }
}