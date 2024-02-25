namespace SupervisorsDashboard
{
    partial class frmDefaultAdminLogin
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnOKDefaultLogin = new System.Windows.Forms.Button();
            this.btnCancelDefaultLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(155, 34);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(138, 26);
            this.txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPassword.Location = new System.Drawing.Point(155, 76);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(138, 26);
            this.txtPassword.TabIndex = 3;
            // 
            // btnOKDefaultLogin
            // 
            this.btnOKDefaultLogin.Location = new System.Drawing.Point(43, 128);
            this.btnOKDefaultLogin.Name = "btnOKDefaultLogin";
            this.btnOKDefaultLogin.Size = new System.Drawing.Size(87, 38);
            this.btnOKDefaultLogin.TabIndex = 4;
            this.btnOKDefaultLogin.Text = "OK";
            this.btnOKDefaultLogin.UseVisualStyleBackColor = true;
            // 
            // btnCancelDefaultLogin
            // 
            this.btnCancelDefaultLogin.Location = new System.Drawing.Point(206, 128);
            this.btnCancelDefaultLogin.Name = "btnCancelDefaultLogin";
            this.btnCancelDefaultLogin.Size = new System.Drawing.Size(87, 38);
            this.btnCancelDefaultLogin.TabIndex = 5;
            this.btnCancelDefaultLogin.Text = "Cancel";
            this.btnCancelDefaultLogin.UseVisualStyleBackColor = true;
            // 
            // frmDefaultAdminLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(332, 205);
            this.Controls.Add(this.btnCancelDefaultLogin);
            this.Controls.Add(this.btnOKDefaultLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmDefaultAdminLogin";
            this.Text = "Default Admin Login";
            this.Load += new System.EventHandler(this.frmDefaultAdminLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnOKDefaultLogin;
        private System.Windows.Forms.Button btnCancelDefaultLogin;
    }
}