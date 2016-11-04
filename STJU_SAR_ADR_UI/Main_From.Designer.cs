namespace STJU_SAR_ADR_UI
{
    partial class Main_From
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_From));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ImageMonitor = new System.Windows.Forms.PictureBox();
            this.RSplatformLabel = new System.Windows.Forms.Label();
            this.meterlabel = new System.Windows.Forms.Label();
            this.pixelsizeLabel = new System.Windows.Forms.Label();
            this.pixelsizeTxtBox = new System.Windows.Forms.TextBox();
            this.ChooseRSPlatformComboBox = new System.Windows.Forms.ComboBox();
            this.VerifyParamButton = new System.Windows.Forms.Button();
            this.WinFormCloseButton = new System.Windows.Forms.Button();
            this.RunDetectionButton = new System.Windows.Forms.Button();
            this.InfoMonitor = new System.Windows.Forms.TextBox();
            this.GetImageInfoButton = new System.Windows.Forms.Button();
            this.LoadImageBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageMonitor)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ImageMonitor);
            this.panel1.Location = new System.Drawing.Point(24, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 405);
            this.panel1.TabIndex = 0;
            // 
            // ImageMonitor
            // 
            this.ImageMonitor.Location = new System.Drawing.Point(0, 0);
            this.ImageMonitor.Name = "ImageMonitor";
            this.ImageMonitor.Size = new System.Drawing.Size(569, 406);
            this.ImageMonitor.TabIndex = 0;
            this.ImageMonitor.TabStop = false;
            // 
            // RSplatformLabel
            // 
            this.RSplatformLabel.AutoSize = true;
            this.RSplatformLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RSplatformLabel.Location = new System.Drawing.Point(639, 60);
            this.RSplatformLabel.Name = "RSplatformLabel";
            this.RSplatformLabel.Size = new System.Drawing.Size(72, 16);
            this.RSplatformLabel.TabIndex = 10;
            this.RSplatformLabel.Text = "遥感平台";
            // 
            // meterlabel
            // 
            this.meterlabel.AutoSize = true;
            this.meterlabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.meterlabel.Location = new System.Drawing.Point(830, 23);
            this.meterlabel.Name = "meterlabel";
            this.meterlabel.Size = new System.Drawing.Size(16, 16);
            this.meterlabel.TabIndex = 9;
            this.meterlabel.Text = "m";
            // 
            // pixelsizeLabel
            // 
            this.pixelsizeLabel.AutoSize = true;
            this.pixelsizeLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pixelsizeLabel.Location = new System.Drawing.Point(631, 22);
            this.pixelsizeLabel.Name = "pixelsizeLabel";
            this.pixelsizeLabel.Size = new System.Drawing.Size(80, 16);
            this.pixelsizeLabel.TabIndex = 8;
            this.pixelsizeLabel.Text = " 像元尺寸";
            // 
            // pixelsizeTxtBox
            // 
            this.pixelsizeTxtBox.Location = new System.Drawing.Point(714, 22);
            this.pixelsizeTxtBox.Name = "pixelsizeTxtBox";
            this.pixelsizeTxtBox.Size = new System.Drawing.Size(110, 21);
            this.pixelsizeTxtBox.TabIndex = 11;
            // 
            // ChooseRSPlatformComboBox
            // 
            this.ChooseRSPlatformComboBox.FormattingEnabled = true;
            this.ChooseRSPlatformComboBox.Items.AddRange(new object[] {
            "TerraSAR-X",
            "J5/J7",
            "MiniSAR",
            "Unknown"});
            this.ChooseRSPlatformComboBox.Location = new System.Drawing.Point(714, 61);
            this.ChooseRSPlatformComboBox.Name = "ChooseRSPlatformComboBox";
            this.ChooseRSPlatformComboBox.Size = new System.Drawing.Size(110, 20);
            this.ChooseRSPlatformComboBox.TabIndex = 12;
            // 
            // VerifyParamButton
            // 
            this.VerifyParamButton.Location = new System.Drawing.Point(862, 102);
            this.VerifyParamButton.Name = "VerifyParamButton";
            this.VerifyParamButton.Size = new System.Drawing.Size(80, 23);
            this.VerifyParamButton.TabIndex = 18;
            this.VerifyParamButton.Text = "确认参数";
            this.VerifyParamButton.UseVisualStyleBackColor = true;
            this.VerifyParamButton.Click += new System.EventHandler(this.VerifyParamButton_Click);
            // 
            // WinFormCloseButton
            // 
            this.WinFormCloseButton.Location = new System.Drawing.Point(833, 423);
            this.WinFormCloseButton.Name = "WinFormCloseButton";
            this.WinFormCloseButton.Size = new System.Drawing.Size(109, 23);
            this.WinFormCloseButton.TabIndex = 17;
            this.WinFormCloseButton.Text = "关闭窗口";
            this.WinFormCloseButton.UseVisualStyleBackColor = true;
            this.WinFormCloseButton.Click += new System.EventHandler(this.WinFormCloseButton_Click);
            // 
            // RunDetectionButton
            // 
            this.RunDetectionButton.Location = new System.Drawing.Point(610, 423);
            this.RunDetectionButton.Name = "RunDetectionButton";
            this.RunDetectionButton.Size = new System.Drawing.Size(167, 23);
            this.RunDetectionButton.TabIndex = 16;
            this.RunDetectionButton.Text = "运行检测程序";
            this.RunDetectionButton.UseVisualStyleBackColor = true;
            this.RunDetectionButton.Click += new System.EventHandler(this.RunDetectionButton_Click);
            // 
            // InfoMonitor
            // 
            this.InfoMonitor.Location = new System.Drawing.Point(610, 142);
            this.InfoMonitor.Multiline = true;
            this.InfoMonitor.Name = "InfoMonitor";
            this.InfoMonitor.Size = new System.Drawing.Size(332, 274);
            this.InfoMonitor.TabIndex = 15;
            // 
            // GetImageInfoButton
            // 
            this.GetImageInfoButton.Location = new System.Drawing.Point(341, 423);
            this.GetImageInfoButton.Name = "GetImageInfoButton";
            this.GetImageInfoButton.Size = new System.Drawing.Size(129, 23);
            this.GetImageInfoButton.TabIndex = 14;
            this.GetImageInfoButton.Text = "获得当前图像信息";
            this.GetImageInfoButton.UseVisualStyleBackColor = true;
            this.GetImageInfoButton.Click += new System.EventHandler(this.GetImageInfoButton_Click);
            // 
            // LoadImageBtn
            // 
            this.LoadImageBtn.Location = new System.Drawing.Point(49, 423);
            this.LoadImageBtn.Name = "LoadImageBtn";
            this.LoadImageBtn.Size = new System.Drawing.Size(124, 23);
            this.LoadImageBtn.TabIndex = 13;
            this.LoadImageBtn.Text = "载入图像";
            this.LoadImageBtn.UseVisualStyleBackColor = true;
            this.LoadImageBtn.Click += new System.EventHandler(this.LoadImageBtn_Click);
            // 
            // Main_From
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 458);
            this.Controls.Add(this.VerifyParamButton);
            this.Controls.Add(this.WinFormCloseButton);
            this.Controls.Add(this.RunDetectionButton);
            this.Controls.Add(this.InfoMonitor);
            this.Controls.Add(this.GetImageInfoButton);
            this.Controls.Add(this.LoadImageBtn);
            this.Controls.Add(this.ChooseRSPlatformComboBox);
            this.Controls.Add(this.pixelsizeTxtBox);
            this.Controls.Add(this.RSplatformLabel);
            this.Controls.Add(this.meterlabel);
            this.Controls.Add(this.pixelsizeLabel);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main_From";
            this.Text = "SAR航空器目标检测插件（上海交大提供）";
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageMonitor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label RSplatformLabel;
        private System.Windows.Forms.Label meterlabel;
        private System.Windows.Forms.Label pixelsizeLabel;
        private System.Windows.Forms.TextBox pixelsizeTxtBox;
        private System.Windows.Forms.ComboBox ChooseRSPlatformComboBox;
        private System.Windows.Forms.Button VerifyParamButton;
        private System.Windows.Forms.Button WinFormCloseButton;
        private System.Windows.Forms.Button RunDetectionButton;
        private System.Windows.Forms.TextBox InfoMonitor;
        private System.Windows.Forms.Button GetImageInfoButton;
        private System.Windows.Forms.Button LoadImageBtn;
        private System.Windows.Forms.PictureBox ImageMonitor;
    }
}

