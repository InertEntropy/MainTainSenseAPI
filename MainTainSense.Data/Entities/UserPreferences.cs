using System.Drawing;

namespace MainTainSense.Data
{
    public class UserPreferences
    {
        public int UserId { get; set; }
        public string PrimaryColor { get; set; } 
        public string SecondaryColor { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public string DefaultFontName { get; set; }
        public float DefaultFontSize { get; set; }
        public FontStyle DefaultFontStyle { get; set; }
        public string HeadingFontName { get; set; }
        public float HeadingFontSize { get; set; }
        public FontStyle HeadingFontStyle { get; set; }
    }

}
