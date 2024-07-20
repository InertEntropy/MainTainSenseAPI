using System.Drawing;
using System.Windows.Forms;
namespace MainTainSense
{
    public static class FormStyling
    {
        // Colors
        public static Color PrimaryColor { get; set; } = Color.Green;
        public static Color SecondaryColor { get; set; } = Color.LightBlue;
        public static Color BackgroundColor { get; set; } = Color.Gainsboro;
        public static Color TextColor { get; set; } = Color.White;

        // Fonts
        public static Font DefaultFont { get; set; } = new Font("Arial Rounded MT Bold", 12);
        public static Font HeadingFont { get; set; } = new Font("Arial Rounded MT Bold", 14, FontStyle.Bold);


        // Button Styles
        public static void StyleButton(Button button)
        {
            button.BackColor = PrimaryColor;
            button.ForeColor = TextColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = DefaultFont;
            button.AutoSize = true;
        }

        // TextBox Styles 
        public static void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = PrimaryColor;
            textBox.ForeColor = TextColor;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = DefaultFont;
        }

        // Label Styles
        public static void StyleLabel(Label label, bool isHeading = false)
        {
            label.ForeColor = TextColor;
            label.Font = isHeading ? HeadingFont : DefaultFont;
            label.AutoSize = true;
        }

        // DataGridView Styles
        public static void StyleDataGridView(DataGridView dataGridView)
        {
            dataGridView.BackgroundColor = PrimaryColor;
            dataGridView.BorderStyle = BorderStyle.Fixed3D;
            dataGridView.AutoSize = true;
            // Alternating Row Colors
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = SecondaryColor;

            // Header Style
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(DefaultFont, FontStyle.Bold);
        }

        // Panel Styles
        public static void StylePanel(Panel panel)
        {
            panel.BackColor = SecondaryColor;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Font = DefaultFont;
            panel.AutoSize = true;
            
        }
    }
}
