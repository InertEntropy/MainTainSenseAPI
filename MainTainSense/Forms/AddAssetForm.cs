using System;
using System.Windows.Forms;
using MainTainSense.Data;
using NLog;

namespace MainTainSense
{
    
    public partial class AddAssetForm : Form
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AddAssetForm()
        {
            InitializeComponent();
        }

        private void AddAssetForm_Load(object sender, EventArgs e)
        {

        }

        private void NewAssetSaveButton_Click(object sender, EventArgs e)
        {
            var asset = new Assets()
            {
                AssetName = assetNameTextBox.Text,
                // ... Gather data from other controls ... 
            };

            // Placeholder for validation (we'll add this soon!)

            var dataRepository = new DataRepository();

            try
            {
                dataRepository.AddAsset(asset);
                MessageBox.Show("Asset Added Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Saving Asset. See logs for details");
                logger.Error(ex); // Assuming you have logging set up
            }
        }
    }
}
