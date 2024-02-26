namespace SupervisorsDashboard
{
    partial class frmManageUsers
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
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.userIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isAdminColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isDefaultAdminColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isEnabledColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rolesIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToOrderColumns = true;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userIDColumn,
            this.userNameColumn,
            this.isAdminColumn,
            this.isDefaultAdminColumn,
            this.isEnabledColumn,
            this.rolesIDColumn});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.Size = new System.Drawing.Size(758, 555);
            this.dgvUsers.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(27, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add New User";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(27, 107);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 34);
            this.button2.TabIndex = 2;
            this.button2.Text = "Save Changes";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(27, 167);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(137, 34);
            this.button3.TabIndex = 3;
            this.button3.Text = "Close";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvUsers);
            this.panel1.Location = new System.Drawing.Point(305, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(758, 555);
            this.panel1.TabIndex = 4;
            // 
            // userIDColumn
            // 
            this.userIDColumn.HeaderText = "User ID";
            this.userIDColumn.Name = "userIDColumn";
            this.userIDColumn.ReadOnly = true;
            this.userIDColumn.Visible = false;
            // 
            // userNameColumn
            // 
            this.userNameColumn.HeaderText = "UserName";
            this.userNameColumn.Name = "userNameColumn";
            // 
            // isAdminColumn
            // 
            this.isAdminColumn.HeaderText = "Is Admin";
            this.isAdminColumn.Name = "isAdminColumn";
            // 
            // isDefaultAdminColumn
            // 
            this.isDefaultAdminColumn.HeaderText = "Default Admin";
            this.isDefaultAdminColumn.Name = "isDefaultAdminColumn";
            // 
            // isEnabledColumn
            // 
            this.isEnabledColumn.HeaderText = "Enabled";
            this.isEnabledColumn.Name = "isEnabledColumn";
            // 
            // rolesIDColumn
            // 
            this.rolesIDColumn.HeaderText = "Role";
            this.rolesIDColumn.Name = "rolesIDColumn";
            // 
            // frmManageUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1087, 615);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmManageUsers";
            this.Text = "Manage Users";
            this.Load += new System.EventHandler(this.frmManageUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAdminColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDefaultAdminColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isEnabledColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rolesIDColumn;
        private System.Windows.Forms.Panel panel1;
    }
}