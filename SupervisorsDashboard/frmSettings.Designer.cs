namespace SupervisorsDashboard
{
    partial class frmSettings
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
            this.lblPolarisLocation = new System.Windows.Forms.Label();
            this.lblPolarisCopylocation = new System.Windows.Forms.Label();
            this.txtPolarisPath = new System.Windows.Forms.TextBox();
            this.txtPolarisCopyPath = new System.Windows.Forms.TextBox();
            this.btnBrowsePolaris = new System.Windows.Forms.Button();
            this.btnBrowsePolarisCopy = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlAdminSettings = new System.Windows.Forms.Panel();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.pnlAdminSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPolarisLocation
            // 
            this.lblPolarisLocation.AutoSize = true;
            this.lblPolarisLocation.Location = new System.Drawing.Point(20, 50);
            this.lblPolarisLocation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPolarisLocation.Name = "lblPolarisLocation";
            this.lblPolarisLocation.Size = new System.Drawing.Size(137, 18);
            this.lblPolarisLocation.TabIndex = 0;
            this.lblPolarisLocation.Text = "Polaris Location";
            // 
            // lblPolarisCopylocation
            // 
            this.lblPolarisCopylocation.AutoSize = true;
            this.lblPolarisCopylocation.Location = new System.Drawing.Point(20, 84);
            this.lblPolarisCopylocation.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPolarisCopylocation.Name = "lblPolarisCopylocation";
            this.lblPolarisCopylocation.Size = new System.Drawing.Size(182, 18);
            this.lblPolarisCopylocation.TabIndex = 1;
            this.lblPolarisCopylocation.Text = "Copy Polaris Location";
            // 
            // txtPolarisPath
            // 
            this.txtPolarisPath.Location = new System.Drawing.Point(208, 42);
            this.txtPolarisPath.Name = "txtPolarisPath";
            this.txtPolarisPath.Size = new System.Drawing.Size(706, 26);
            this.txtPolarisPath.TabIndex = 2;
            // 
            // txtPolarisCopyPath
            // 
            this.txtPolarisCopyPath.Location = new System.Drawing.Point(208, 76);
            this.txtPolarisCopyPath.Name = "txtPolarisCopyPath";
            this.txtPolarisCopyPath.Size = new System.Drawing.Size(706, 26);
            this.txtPolarisCopyPath.TabIndex = 3;
            // 
            // btnBrowsePolaris
            // 
            this.btnBrowsePolaris.Location = new System.Drawing.Point(920, 35);
            this.btnBrowsePolaris.Name = "btnBrowsePolaris";
            this.btnBrowsePolaris.Size = new System.Drawing.Size(92, 33);
            this.btnBrowsePolaris.TabIndex = 5;
            this.btnBrowsePolaris.Text = "Browse";
            this.btnBrowsePolaris.UseVisualStyleBackColor = true;
            this.btnBrowsePolaris.Click += new System.EventHandler(this.btnBrowsePolaris_Click);
            // 
            // btnBrowsePolarisCopy
            // 
            this.btnBrowsePolarisCopy.Location = new System.Drawing.Point(920, 69);
            this.btnBrowsePolarisCopy.Name = "btnBrowsePolarisCopy";
            this.btnBrowsePolarisCopy.Size = new System.Drawing.Size(92, 33);
            this.btnBrowsePolarisCopy.TabIndex = 6;
            this.btnBrowsePolarisCopy.Text = "Browse";
            this.btnBrowsePolarisCopy.UseVisualStyleBackColor = true;
            this.btnBrowsePolarisCopy.Click += new System.EventHandler(this.btnBrowsePolarisCopy_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1089, 569);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 33);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlAdminSettings
            // 
            this.pnlAdminSettings.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlAdminSettings.Controls.Add(this.btnManageUsers);
            this.pnlAdminSettings.Controls.Add(this.txtPolarisCopyPath);
            this.pnlAdminSettings.Controls.Add(this.lblPolarisLocation);
            this.pnlAdminSettings.Controls.Add(this.btnBrowsePolarisCopy);
            this.pnlAdminSettings.Controls.Add(this.lblPolarisCopylocation);
            this.pnlAdminSettings.Controls.Add(this.btnBrowsePolaris);
            this.pnlAdminSettings.Controls.Add(this.txtPolarisPath);
            this.pnlAdminSettings.Location = new System.Drawing.Point(12, 12);
            this.pnlAdminSettings.Name = "pnlAdminSettings";
            this.pnlAdminSettings.Size = new System.Drawing.Size(1296, 164);
            this.pnlAdminSettings.TabIndex = 8;
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Location = new System.Drawing.Point(1136, 35);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(138, 33);
            this.btnManageUsers.TabIndex = 9;
            this.btnManageUsers.Text = "Manage Users";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1333, 623);
            this.Controls.Add(this.pnlAdminSettings);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.pnlAdminSettings.ResumeLayout(false);
            this.pnlAdminSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPolarisLocation;
        private System.Windows.Forms.Label lblPolarisCopylocation;
        private System.Windows.Forms.TextBox txtPolarisPath;
        private System.Windows.Forms.TextBox txtPolarisCopyPath;
        private System.Windows.Forms.Button btnBrowsePolaris;
        private System.Windows.Forms.Button btnBrowsePolarisCopy;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlAdminSettings;
        private System.Windows.Forms.Button btnManageUsers;
    }
}