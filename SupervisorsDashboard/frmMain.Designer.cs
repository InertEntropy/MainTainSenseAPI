namespace SupervisorsDashboard
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSettings = new System.Windows.Forms.Button();
            this.lblPolarisCopylocation = new System.Windows.Forms.Label();
            this.lblPolarisLocation = new System.Windows.Forms.Label();
            this.lblMainCopyPolarisLocation = new System.Windows.Forms.Label();
            this.lblMainPolarisLocation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(14, 568);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(111, 42);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // lblPolarisCopylocation
            // 
            this.lblPolarisCopylocation.AutoSize = true;
            this.lblPolarisCopylocation.Location = new System.Drawing.Point(14, 521);
            this.lblPolarisCopylocation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPolarisCopylocation.Name = "lblPolarisCopylocation";
            this.lblPolarisCopylocation.Size = new System.Drawing.Size(182, 18);
            this.lblPolarisCopylocation.TabIndex = 3;
            this.lblPolarisCopylocation.Text = "Copy Polaris Location";
            // 
            // lblPolarisLocation
            // 
            this.lblPolarisLocation.AutoSize = true;
            this.lblPolarisLocation.Location = new System.Drawing.Point(14, 487);
            this.lblPolarisLocation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPolarisLocation.Name = "lblPolarisLocation";
            this.lblPolarisLocation.Size = new System.Drawing.Size(137, 18);
            this.lblPolarisLocation.TabIndex = 2;
            this.lblPolarisLocation.Text = "Polaris Location";
            // 
            // lblMainCopyPolarisLocation
            // 
            this.lblMainCopyPolarisLocation.AutoSize = true;
            this.lblMainCopyPolarisLocation.Location = new System.Drawing.Point(207, 521);
            this.lblMainCopyPolarisLocation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblMainCopyPolarisLocation.Name = "lblMainCopyPolarisLocation";
            this.lblMainCopyPolarisLocation.Size = new System.Drawing.Size(182, 18);
            this.lblMainCopyPolarisLocation.TabIndex = 5;
            this.lblMainCopyPolarisLocation.Text = "Copy Polaris Location";
            // 
            // lblMainPolarisLocation
            // 
            this.lblMainPolarisLocation.AutoSize = true;
            this.lblMainPolarisLocation.Location = new System.Drawing.Point(207, 487);
            this.lblMainPolarisLocation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblMainPolarisLocation.Name = "lblMainPolarisLocation";
            this.lblMainPolarisLocation.Size = new System.Drawing.Size(137, 18);
            this.lblMainPolarisLocation.TabIndex = 4;
            this.lblMainPolarisLocation.Text = "Polaris Location";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(892, 623);
            this.Controls.Add(this.lblMainCopyPolarisLocation);
            this.Controls.Add(this.lblMainPolarisLocation);
            this.Controls.Add(this.lblPolarisCopylocation);
            this.Controls.Add(this.lblPolarisLocation);
            this.Controls.Add(this.btnSettings);
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmMain";
            this.Text = "Supervisors Dashboard";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label lblPolarisCopylocation;
        private System.Windows.Forms.Label lblPolarisLocation;
        private System.Windows.Forms.Label lblMainCopyPolarisLocation;
        private System.Windows.Forms.Label lblMainPolarisLocation;
    }
}

