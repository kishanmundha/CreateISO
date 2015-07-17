namespace CreateISO
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.DriveList = new System.Windows.Forms.ComboBox();
            this.SlactDrvLbl = new System.Windows.Forms.Label();
            this.SelectDrivePanel = new System.Windows.Forms.Panel();
            this.Refresh_Drive = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.SavePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.errByteLabel = new System.Windows.Forms.Label();
            this.statusMsg = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.errPanel = new System.Windows.Forms.Panel();
            this.errMsg = new System.Windows.Forms.Label();
            this.Start = new System.Windows.Forms.Button();
            this.Abort = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SelectDrivePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.errPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DriveList
            // 
            this.DriveList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DriveList.FormattingEnabled = true;
            this.DriveList.Location = new System.Drawing.Point(81, 2);
            this.DriveList.Name = "DriveList";
            this.DriveList.Size = new System.Drawing.Size(115, 23);
            this.DriveList.TabIndex = 0;
            // 
            // SlactDrvLbl
            // 
            this.SlactDrvLbl.AutoSize = true;
            this.SlactDrvLbl.Location = new System.Drawing.Point(3, 6);
            this.SlactDrvLbl.Name = "SlactDrvLbl";
            this.SlactDrvLbl.Size = new System.Drawing.Size(68, 15);
            this.SlactDrvLbl.TabIndex = 1;
            this.SlactDrvLbl.Text = "Select Drive";
            // 
            // SelectDrivePanel
            // 
            this.SelectDrivePanel.Controls.Add(this.Refresh_Drive);
            this.SelectDrivePanel.Controls.Add(this.DriveList);
            this.SelectDrivePanel.Controls.Add(this.SlactDrvLbl);
            this.SelectDrivePanel.Location = new System.Drawing.Point(12, 12);
            this.SelectDrivePanel.Name = "SelectDrivePanel";
            this.SelectDrivePanel.Size = new System.Drawing.Size(307, 28);
            this.SelectDrivePanel.TabIndex = 2;
            // 
            // Refresh_Drive
            // 
            this.Refresh_Drive.Location = new System.Drawing.Point(202, 1);
            this.Refresh_Drive.Name = "Refresh_Drive";
            this.Refresh_Drive.Size = new System.Drawing.Size(100, 24);
            this.Refresh_Drive.TabIndex = 2;
            this.Refresh_Drive.Text = "Refresh";
            this.Refresh_Drive.UseVisualStyleBackColor = true;
            this.Refresh_Drive.Click += new System.EventHandler(this.Refresh_Drive_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "test.iso";
            this.saveFileDialog1.Filter = "ISO Image (*.iso)|*.iso";
            this.saveFileDialog1.InitialDirectory = "C:\\";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.SavePath);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 57);
            this.panel1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(236, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SavePath
            // 
            this.SavePath.Location = new System.Drawing.Point(6, 24);
            this.SavePath.Name = "SavePath";
            this.SavePath.Size = new System.Drawing.Size(223, 23);
            this.SavePath.TabIndex = 1;
            this.SavePath.Text = "D:\\test.iso";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Iso File Path";
            // 
            // statusPanel
            // 
            this.statusPanel.Controls.Add(this.errByteLabel);
            this.statusPanel.Controls.Add(this.statusMsg);
            this.statusPanel.Controls.Add(this.progressBar);
            this.statusPanel.Location = new System.Drawing.Point(12, 144);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(305, 92);
            this.statusPanel.TabIndex = 4;
            this.statusPanel.Visible = false;
            // 
            // errByteLabel
            // 
            this.errByteLabel.AutoSize = true;
            this.errByteLabel.ForeColor = System.Drawing.Color.Red;
            this.errByteLabel.Location = new System.Drawing.Point(16, 60);
            this.errByteLabel.Name = "errByteLabel";
            this.errByteLabel.Size = new System.Drawing.Size(30, 15);
            this.errByteLabel.TabIndex = 2;
            this.errByteLabel.Text = "0 KB";
            this.errByteLabel.Visible = false;
            // 
            // statusMsg
            // 
            this.statusMsg.AutoSize = true;
            this.statusMsg.Location = new System.Drawing.Point(15, 37);
            this.statusMsg.Name = "statusMsg";
            this.statusMsg.Size = new System.Drawing.Size(30, 15);
            this.statusMsg.TabIndex = 1;
            this.statusMsg.Text = "0 KB";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(3, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(298, 25);
            this.progressBar.TabIndex = 0;
            // 
            // errPanel
            // 
            this.errPanel.Controls.Add(this.errMsg);
            this.errPanel.Location = new System.Drawing.Point(12, 242);
            this.errPanel.Name = "errPanel";
            this.errPanel.Size = new System.Drawing.Size(304, 48);
            this.errPanel.TabIndex = 5;
            this.errPanel.Visible = false;
            // 
            // errMsg
            // 
            this.errMsg.AutoSize = true;
            this.errMsg.ForeColor = System.Drawing.Color.Red;
            this.errMsg.Location = new System.Drawing.Point(4, 4);
            this.errMsg.Name = "errMsg";
            this.errMsg.Size = new System.Drawing.Size(38, 15);
            this.errMsg.TabIndex = 0;
            this.errMsg.Text = "Error: ";
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(42, 112);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(120, 26);
            this.Start.TabIndex = 6;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Abort
            // 
            this.Abort.Location = new System.Drawing.Point(168, 112);
            this.Abort.Name = "Abort";
            this.Abort.Size = new System.Drawing.Size(120, 26);
            this.Abort.TabIndex = 7;
            this.Abort.Text = "Abort";
            this.Abort.UseVisualStyleBackColor = true;
            this.Abort.Click += new System.EventHandler(this.Abort_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 302);
            this.Controls.Add(this.Abort);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.errPanel);
            this.Controls.Add(this.statusPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SelectDrivePanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "Form1";
            this.Text = "Create ISO";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SelectDrivePanel.ResumeLayout(false);
            this.SelectDrivePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            this.errPanel.ResumeLayout(false);
            this.errPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox DriveList;
        private System.Windows.Forms.Label SlactDrvLbl;
        private System.Windows.Forms.Panel SelectDrivePanel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SavePath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Refresh_Drive;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label statusMsg;
        private System.Windows.Forms.Panel errPanel;
        private System.Windows.Forms.Label errMsg;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Abort;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label errByteLabel;

    }
}

