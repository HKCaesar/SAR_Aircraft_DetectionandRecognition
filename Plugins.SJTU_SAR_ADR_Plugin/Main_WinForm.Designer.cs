namespace Plugins.SJTU_SAR_ADR_Plugin
{
    partial class Main_WinForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_WinForm));
            this.ChooseFolderButton = new System.Windows.Forms.Button();
            this.GetImageInfoButton = new System.Windows.Forms.Button();
            this.InfoMonitor = new System.Windows.Forms.TextBox();
            this.ImageMonitor = new System.Windows.Forms.PictureBox();
            this.pixelsizeLabel = new System.Windows.Forms.Label();
            this.pixelsizeTxtBox = new System.Windows.Forms.TextBox();
            this.meterlabel = new System.Windows.Forms.Label();
            this.RSplatformLabel = new System.Windows.Forms.Label();
            this.ChooseRSPlatformComboBox = new System.Windows.Forms.ComboBox();
            this.RunDetectionButton = new System.Windows.Forms.Button();
            this.WinFormCloseButton = new System.Windows.Forms.Button();
            this.VerifyParamButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ImageMonitor)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChooseFolderButton
            // 
            this.ChooseFolderButton.Location = new System.Drawing.Point(48, 423);
            this.ChooseFolderButton.Name = "ChooseFolderButton";
            this.ChooseFolderButton.Size = new System.Drawing.Size(124, 23);
            this.ChooseFolderButton.TabIndex = 0;
            this.ChooseFolderButton.Text = "选择插件文件夹路径";
            this.ChooseFolderButton.UseVisualStyleBackColor = true;
            this.ChooseFolderButton.Click += new System.EventHandler(this.ChooseFolderButton_Click);
            // 
            // GetImageInfoButton
            // 
            this.GetImageInfoButton.Location = new System.Drawing.Point(340, 423);
            this.GetImageInfoButton.Name = "GetImageInfoButton";
            this.GetImageInfoButton.Size = new System.Drawing.Size(129, 23);
            this.GetImageInfoButton.TabIndex = 1;
            this.GetImageInfoButton.Text = "获得当前图像信息";
            this.GetImageInfoButton.UseVisualStyleBackColor = true;
            this.GetImageInfoButton.Click += new System.EventHandler(this.GetImageInfoButton_Click);
            // 
            // InfoMonitor
            // 
            this.InfoMonitor.Location = new System.Drawing.Point(609, 142);
            this.InfoMonitor.Multiline = true;
            this.InfoMonitor.Name = "InfoMonitor";
            this.InfoMonitor.Size = new System.Drawing.Size(332, 274);
            this.InfoMonitor.TabIndex = 2;
            this.InfoMonitor.TextChanged += new System.EventHandler(this.InfoMonitor_TextChanged);
            // 
            // ImageMonitor
            // 
            this.ImageMonitor.Location = new System.Drawing.Point(-1, -1);
            this.ImageMonitor.Name = "ImageMonitor";
            this.ImageMonitor.Size = new System.Drawing.Size(569, 405);
            this.ImageMonitor.TabIndex = 3;
            this.ImageMonitor.TabStop = false;
            this.ImageMonitor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageMonitor_MouseDown);
            this.ImageMonitor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageMonitor_MouseMove);
            this.ImageMonitor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageMonitor_MouseUp);
            // 
            // pixelsizeLabel
            // 
            this.pixelsizeLabel.AutoSize = true;
            this.pixelsizeLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pixelsizeLabel.Location = new System.Drawing.Point(631, 22);
            this.pixelsizeLabel.Name = "pixelsizeLabel";
            this.pixelsizeLabel.Size = new System.Drawing.Size(80, 16);
            this.pixelsizeLabel.TabIndex = 4;
            this.pixelsizeLabel.Text = " 像元尺寸";
            // 
            // pixelsizeTxtBox
            // 
            this.pixelsizeTxtBox.Location = new System.Drawing.Point(714, 19);
            this.pixelsizeTxtBox.Name = "pixelsizeTxtBox";
            this.pixelsizeTxtBox.Size = new System.Drawing.Size(110, 21);
            this.pixelsizeTxtBox.TabIndex = 5;
            // 
            // meterlabel
            // 
            this.meterlabel.AutoSize = true;
            this.meterlabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.meterlabel.Location = new System.Drawing.Point(830, 23);
            this.meterlabel.Name = "meterlabel";
            this.meterlabel.Size = new System.Drawing.Size(16, 16);
            this.meterlabel.TabIndex = 6;
            this.meterlabel.Text = "m";
            // 
            // RSplatformLabel
            // 
            this.RSplatformLabel.AutoSize = true;
            this.RSplatformLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RSplatformLabel.Location = new System.Drawing.Point(639, 60);
            this.RSplatformLabel.Name = "RSplatformLabel";
            this.RSplatformLabel.Size = new System.Drawing.Size(72, 16);
            this.RSplatformLabel.TabIndex = 7;
            this.RSplatformLabel.Text = "遥感平台";
            // 
            // ChooseRSPlatformComboBox
            // 
            this.ChooseRSPlatformComboBox.FormattingEnabled = true;
            this.ChooseRSPlatformComboBox.Items.AddRange(new object[] {
            "TerraSAR-X",
            "J5/J7",
            "MiniSAR",
            "Unknown"});
            this.ChooseRSPlatformComboBox.Location = new System.Drawing.Point(714, 60);
            this.ChooseRSPlatformComboBox.Name = "ChooseRSPlatformComboBox";
            this.ChooseRSPlatformComboBox.Size = new System.Drawing.Size(110, 20);
            this.ChooseRSPlatformComboBox.TabIndex = 8;
            // 
            // RunDetectionButton
            // 
            this.RunDetectionButton.Location = new System.Drawing.Point(609, 423);
            this.RunDetectionButton.Name = "RunDetectionButton";
            this.RunDetectionButton.Size = new System.Drawing.Size(167, 23);
            this.RunDetectionButton.TabIndex = 9;
            this.RunDetectionButton.Text = "运行检测程序";
            this.RunDetectionButton.UseVisualStyleBackColor = true;
            this.RunDetectionButton.Click += new System.EventHandler(this.RunDetectionButton_Click);
            // 
            // WinFormCloseButton
            // 
            this.WinFormCloseButton.Location = new System.Drawing.Point(832, 423);
            this.WinFormCloseButton.Name = "WinFormCloseButton";
            this.WinFormCloseButton.Size = new System.Drawing.Size(109, 23);
            this.WinFormCloseButton.TabIndex = 10;
            this.WinFormCloseButton.Text = "关闭窗口";
            this.WinFormCloseButton.UseVisualStyleBackColor = true;
            this.WinFormCloseButton.Click += new System.EventHandler(this.WinFormCloseButton_Click);
            // 
            // VerifyParamButton
            // 
            this.VerifyParamButton.Location = new System.Drawing.Point(861, 102);
            this.VerifyParamButton.Name = "VerifyParamButton";
            this.VerifyParamButton.Size = new System.Drawing.Size(80, 23);
            this.VerifyParamButton.TabIndex = 11;
            this.VerifyParamButton.Text = "确认参数";
            this.VerifyParamButton.UseVisualStyleBackColor = true;
            this.VerifyParamButton.Click += new System.EventHandler(this.VerifyParamButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ImageMonitor);
            this.panel1.Location = new System.Drawing.Point(24, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 405);
            this.panel1.TabIndex = 12;
            // 
            // Main_WinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 458);
            this.Controls.Add(this.VerifyParamButton);
            this.Controls.Add(this.WinFormCloseButton);
            this.Controls.Add(this.RunDetectionButton);
            this.Controls.Add(this.ChooseRSPlatformComboBox);
            this.Controls.Add(this.RSplatformLabel);
            this.Controls.Add(this.meterlabel);
            this.Controls.Add(this.pixelsizeTxtBox);
            this.Controls.Add(this.pixelsizeLabel);
            this.Controls.Add(this.InfoMonitor);
            this.Controls.Add(this.GetImageInfoButton);
            this.Controls.Add(this.ChooseFolderButton);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main_WinForm";
            this.Text = "SAR航空器目标检测插件（上海交大提供）";
            this.Load += new System.EventHandler(this.Main_WinForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImageMonitor)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChooseFolderButton;
        private System.Windows.Forms.Button GetImageInfoButton;
        private System.Windows.Forms.TextBox InfoMonitor;
        private System.Windows.Forms.PictureBox ImageMonitor;
        private System.Windows.Forms.Label pixelsizeLabel;
        private System.Windows.Forms.TextBox pixelsizeTxtBox;
        private System.Windows.Forms.Label meterlabel;
        private System.Windows.Forms.Label RSplatformLabel;
        private System.Windows.Forms.ComboBox ChooseRSPlatformComboBox;
        private System.Windows.Forms.Button RunDetectionButton;
        private System.Windows.Forms.Button WinFormCloseButton;
        private System.Windows.Forms.Button VerifyParamButton;
        private System.Windows.Forms.Panel panel1;
    }
}