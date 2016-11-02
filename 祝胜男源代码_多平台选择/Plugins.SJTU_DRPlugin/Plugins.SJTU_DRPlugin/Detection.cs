using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIOCore;
using AIOCore.Layer;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Plugins.INFOcombinePlugin;

namespace Plugins.helloworldPlugin
{
    public partial class Detection : Form
    {
        
        
        public Process process = null;
        public Process processS = null;
        public Process process1 = null;
        public Process process2 = null;
        public Process process3 = null;
        public Process process4 = null;
        

        public string PluginfoldPath, imagepath;

        public string PL_img_path, PL_img_XML_path, Desktop_Path;
        double PL_img_Rez, LatMin, LatMax, LongMin, LongMax;
        int PL_img_Height, PL_img_Width;
        public Detection()
        {
            Desktop_Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            InitializeComponent();

        }
        public static bool CopyIniFileToObject(string inifilePath, string objectPath)
        {
            bool result = false;
            try
            {
                System.IO.DirectoryInfo inifileDirect = new DirectoryInfo(inifilePath);
                System.IO.FileInfo[] files = inifileDirect.GetFiles();
                string sourcePath = inifileDirect.FullName;
                foreach (System.IO.FileInfo item in files)
                {
                    string sourceFileFullName = item.FullName;
                    string destFileFullName = sourceFileFullName.Replace(sourcePath, objectPath);
                    item.CopyTo(destFileFullName, true);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                StringBuilder sb = new StringBuilder(this.textBox1.Text);
                this.textBox1.Text = sb.AppendLine(outLine.Data).ToString();
                this.textBox1.SelectionStart = this.textBox1.Text.Length;
                this.textBox1.ScrollToCaret();
            }
        }
        
             
        #region 选择插件文件夹路径
       
        //选择插件文件夹路径按钮
        private void button1_Click(object sender, EventArgs e)
        {
            int pluginpath_Flag = 1;
            string pluginfoldpath = null;
            StreamReader PluginfoldPath_rl = new StreamReader(@"plugin_path.txt", Encoding.UTF8);
            if ((pluginfoldpath = PluginfoldPath_rl.ReadLine()) != null)
            {
                PluginfoldPath_rl.Close();
                PluginfoldPath = pluginfoldpath;
                DialogResult dr = MessageBox.Show("插件文件夹:" + PluginfoldPath + "\n是否需要修改插件路径?", "选择文件夹提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    PluginfoldPath_rl.Close();
                    pluginpath_Flag = 2;


                }
            }
            else
            {
                PluginfoldPath_rl.Close();
                pluginpath_Flag = 0;
            }

            if (pluginpath_Flag == 0)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                dialog.Description = "请选择插件文件夹路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter plugin_sw = new StreamWriter(@"plugin_path.txt", false);
                    PluginfoldPath = dialog.SelectedPath;
                    MessageBox.Show("已选择文件夹:" + PluginfoldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    plugin_sw.Write(PluginfoldPath);
                    plugin_sw.Close();
                }
            }
            else if (pluginpath_Flag == 2) //原来已有，重新覆盖
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter plugin_sw = new StreamWriter(@"plugin_path.txt", false);//false表示覆盖原有文件
                    PluginfoldPath = dialog.SelectedPath;
                    MessageBox.Show("已选择文件夹:" + PluginfoldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    plugin_sw.Write(PluginfoldPath);//写入新选择的文件夹路径
                    plugin_sw.Close();
                }
            }

        }
        #endregion
        /// <summary>
        /// 获得可见光舰船图像信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void process3_Exited(object sender, EventArgs e)
        {
            //MessageBox.Show("Slice Finished");
        }
       
        #region 获得图像信息
         private void button2_Click(object sender, EventArgs e)
         {
             if (SpatialDataViewer.ActiveViewControl != null || SpatialDataViewer.ActiveViewControl.View != null)
             {
                 //确定当前文件夹路径
                 string current_Path = Environment.CurrentDirectory;
                 string AimFolder = PluginfoldPath;
                 if (!Directory.Exists(AimFolder))
                 {
                     Directory.CreateDirectory(AimFolder);
                 }
                
                 GxView _v = SpatialDataViewer.ActiveViewControl.View;
                 GxImageGraphicsItem image = _v.ImageLayer.EdittedImage;
                 GxImageGraphicsItem image2 = image;
                 //(2)图像名称
                 string img_Name = image2.Name;
                 string ImageFolder = AimFolder + @"\result";
                 if (!Directory.Exists(ImageFolder))
                 { Directory.CreateDirectory(ImageFolder); }
                 StreamWriter sw = new StreamWriter(AimFolder + @".\bin_readVIF\ImageINFO.txt", false, Encoding.Default);
                 //（1）通过GxImageGraphicsItem Class下的Properties获取各种图像信息
                 //(1)图像的绝对路径名
                 this.PL_img_path = image2.FileName;
                 this.imagepath = image2.FileName;

                
                 //将图像路径写出，做缩略图
                 StreamWriter image_path = new StreamWriter(PluginfoldPath + @"\config\image_path.txt", false, Encoding.Default);
                 image_path.Write("<ImagePath>" + "\r\n");
                 image_path.Write(imagepath + "\r\n");
                 image_path.Close();
                 MessageBox.Show("image_path.txt finished");

                 //缩略图
                 Control.CheckForIllegalCrossThreadCalls = false;
                 process3 = new Process();
                 process3.StartInfo.FileName = "cmd.exe";
                 process3.StartInfo.WorkingDirectory = ".";
                 //新加入可以得到exe结束信息的代码
                 process3.EnableRaisingEvents = true;
                 process3.Exited += new EventHandler(process3_Exited);

                 process3.StartInfo.UseShellExecute = false;
                 process3.StartInfo.RedirectStandardInput = true;
                 process3.StartInfo.RedirectStandardOutput = true;
                 process3.StartInfo.CreateNoWindow = true;
                 //Process.Start("cmd.exe");
                 process3.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                 process3.Start();

                 string[] route_str = PluginfoldPath.Split(':');
                 string disk_pos = route_str[0].ToString() + ":";
                 process3.StandardInput.WriteLine(disk_pos);
                 process3.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_thumbnail\\");
                 process3.StandardInput.WriteLine(".\\pyramid.exe");
                 //process.StandardInput.WriteLine("exit");

                 process3.BeginOutputReadLine();
                 process3.CloseMainWindow();
                 process3.StandardInput.WriteLine(">Finish");
                 process3.StandardInput.WriteLine("exit");
                 // process.
                 process3.WaitForExit();
                 //缩略图

                 //显示缩略图
                 this.pictureBox1.Image = Image.FromFile(PluginfoldPath + @"\bin_readVIF\imgthumb.tif");
                 this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;//设置图片显示方式 
                 //

                 this.textBox1.AppendText("ImagePath: " + PL_img_path + "\n");
                 this.textBox1.AppendText("ImageName: " + img_Name + "\n");

                 //(1-3) 获取图像的分辨率
                 PL_img_Rez = image2.ImgPR;
                 double curPixel_size = image2.CurrentImageSource.PadfTransform[1];
                 string Rez_s = PL_img_Rez.ToString();
                 this.textBox1.AppendText("ImageResolution: " + Rez_s + "\n");
                 this.textBox1.AppendText("ImagePixelSize: " + curPixel_size + "\n");
                 this.textBox2.AppendText(Rez_s);
                 //(3) 获取图像尺寸信息
                 PL_img_Height = image2.Height;
                 PL_img_Width = image2.Width;
                 string Height_s = PL_img_Height.ToString();
                 string Width_s = PL_img_Width.ToString();
                 this.textBox1.AppendText("ImageHeight(Rows): " + Height_s + "\n");
                 this.textBox1.AppendText("ImageWidth(Columns): " + Width_s + "\n");

                 //(4)获取平台中的地理经纬度（与图像旁边的一列表中的数据一致)
                 LatMin = image2.MinY;
                 LatMax = image2.MaxY;
                 LongMin = image2.MinX;
                 LongMax = image2.MaxX;
                 if (!string.IsNullOrEmpty(image2.PszProjection))
                 {
                     GeoPoint gpt = GxView.Meter2Degree(new GeoPoint(LongMin, LatMax), image2.PszProjection);
                     GeoPoint gpt2 = GxView.Meter2Degree(new GeoPoint(LongMax, LatMin), image2.PszProjection);
                     LongMin = gpt.X;
                     LatMax = gpt.Y;
                     LongMax = gpt2.X;
                     LatMin = gpt2.Y;
                 }

                 this.textBox1.AppendText("LatMin: " + LatMin + "\n");
                 this.textBox1.AppendText("LatMax: " + LatMax + "\n");
                 this.textBox1.AppendText("LongMin: " + LongMin + "\n");
                 this.textBox1.AppendText("LongMax: " + LongMax + "\n");




                 //MessageBox.Show("获取图像信息结束，请运行检测插件。", "插件运行状态信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 #region 书写ImageINFO.txt和config.txt
                
                 sw.Write("<SARImagePath>" + "\r\n");
                 sw.Write(PL_img_path + "\r\n");
                 sw.Write("<RefImgPath>" + "\r\n");
                 sw.Write(ImageFolder + @"\img_en.tif" + "\r\n");
                 sw.Write("<OutputDir>" + "\r\n");
                 sw.Write(AimFolder + @"\result" + "\r\n");
                 sw.Write("<ImageName>" + "\r\n");
                 sw.Write(img_Name + "\r\n");
                 sw.Write("<ImageHeight(Rows)>" + "\r\n");
                 sw.Write(PL_img_Height + "\r\n");
                 sw.Write("<ImageWidth(Cols)>" + "\r\n");
                 sw.Write(PL_img_Width + "\r\n");
                 sw.Write("<LatMin>" + "\r\n");
                 sw.Write(LatMin + "\r\n");
                 sw.Write("<LatMax>" + "\r\n");
                 sw.Write(LatMax + "\r\n");
                 sw.Write("<LongMin>" + "\r\n");
                 sw.Write(LongMin + "\r\n");
                 sw.Write("<LongMax>" + "\r\n");
                 sw.Write(LongMax + "\r\n");
                 sw.Write("<PixelSize>" + "\r\n");
                 sw.Write(PL_img_Rez + "\r\n");
                 sw.Close();
                 StreamWriter config_W = new StreamWriter(AimFolder + @".\config\config.txt", false, Encoding.Default);
                 config_W.Write("<SARImagePath>" + "\r\n");
                 config_W.Write(PL_img_path + "\r\n");
                 config_W.Write("<MaskPath>" + "\r\n");
                 config_W.Write(AimFolder + @"\result\maskimg.tif" + "\r\n");
                 config_W.Write("<RefImagePath>" + "\r\n");
                 config_W.Write(AimFolder + @"\result\img_en.tif" + "\r\n");
                 config_W.Write("<TemplatePath>" + "\r\n");
                 config_W.Write(AimFolder + @"\Template" + "\r\n");
                 config_W.Write("<ResultImagePath>" + "\r\n");
                 config_W.Write(AimFolder + @"\result" + "\r\n");
                 config_W.Write("<ResultXmlPath>" + "\r\n");
                 config_W.Write(AimFolder + @"\result\" + img_Name + ".xml" + "\r\n");
                 config_W.Write("<MinLongtitude>" + "\r\n");
                 config_W.Write(LongMin + "\r\n");
                 config_W.Write("<MaxLongitude>" + "\r\n");
                 config_W.Write(LongMax + "\r\n");
                 config_W.Write("<MinLatitude>" + "\r\n");
                 config_W.Write(LatMin + "\r\n");
                 config_W.Write("<MaxLatitude>" + "\r\n");
                 config_W.Write(LatMax + "\r\n");
                 config_W.Write("<PixelSize>" + "\r\n");
                 config_W.Write(PL_img_Rez + "\r\n");
                 config_W.Write("<ImageRows>" + "\r\n");
                 config_W.Write(image2.Height + "\r\n");
                 config_W.Write("<ImageCols>" + "\r\n");
                 config_W.Write(image2.Width + "\r\n");
                 config_W.Close();
                 #endregion
                 
                 
             }
         }
        #endregion

        #region 图片缩放和拖拽
         private void Detection_Load(object sender, EventArgs e)
         {
             this.pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
         }

         private void pictureBox1_Click(object sender, EventArgs e)
         {

         }
         private bool canMove = false;
         private Point mousePos;
         //鼠标移动
         private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
         {
             if (this.canMove)
             {
                 pictureBox1.Location = new Point(pictureBox1.Location.X
                                      - mousePos.X + e.X, pictureBox1.Location.Y
                                      - mousePos.Y + e.Y);
             }
             this.pictureBox1.Focus();
         }
         //鼠标按下
         private void pictureBoxAP1_MouseDown(object sender, MouseEventArgs e)
         {
             this.canMove = true;
             this.mousePos = new Point(e.X, e.Y);//获得鼠标按下时刻坐标
         }
         //鼠标释放
         private void pictureBoxAP1_MouseUp(object sender, MouseEventArgs e)
         {
             this.canMove = false;
         }


         private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
         {
             //this.pictureBox1.Width += e.Delta;
             //this.pictureBox1.Height += e.Delta;
             //向前
             float width = this.pictureBox1.Width;
             float height = this.pictureBox1.Height;
             float centerx = this.pictureBox1.Location.X + width / 2;
             float centery = this.pictureBox1.Location.Y + height / 2;
             if (e.Delta > 0)
             {

                 float w = this.pictureBox1.Width * 0.9f; //每次縮小 20%  
                 float h = this.pictureBox1.Height * 0.9f;

                 this.pictureBox1.Size = Size.Ceiling(new SizeF(w, h));
                 this.pictureBox1.Location = new Point(Convert.ToInt32(centerx - (w / 2)), Convert.ToInt32(centery - (h / 2)));//图片居中缩放
             }

             //向后
             else if (e.Delta < 0)
             {

                 float w = this.pictureBox1.Width * 1.1f; //每次放大 20%
                 float h = this.pictureBox1.Height * 1.1f;
                 this.pictureBox1.Size = Size.Ceiling(new SizeF(w, h));
                 this.pictureBox1.Location = new Point(Convert.ToInt32(centerx - (w / 2)), Convert.ToInt32(centery - (h / 2)));//图片居中缩放
                 pictureBox1.Invalidate();

             }

         }
         #endregion       

        #region 修改pixelsize

        private void process2_Exited(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //修改pixelsize按钮
        private void button5_Click(object sender, EventArgs e)
        {
            StreamWriter txtchange = new StreamWriter(PluginfoldPath + @".\config\txtchange.txt", false, Encoding.Default);
            txtchange.Write(textBox2.Text + "\r\n");
            txtchange.Close();

            Control.CheckForIllegalCrossThreadCalls = false;
            process2 = new Process();
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.WorkingDirectory = ".";
            //新加入可以得到exe结束信息的代码
            process2.EnableRaisingEvents = true;
            process2.Exited += new EventHandler(process2_Exited);

            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.RedirectStandardInput = true;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            process2.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process2.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process2.StandardInput.WriteLine(disk_pos);
            process2.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_txt\\");
            process2.StandardInput.WriteLine(".\\txtchange.exe");
            //process.StandardInput.WriteLine("exit");

            process2.BeginOutputReadLine();
            process2.CloseMainWindow();
            process2.StandardInput.WriteLine(">Finish");
            process2.StandardInput.WriteLine("exit");
            // process.
            process2.WaitForExit();

            MessageBox.Show("分辨率修改成功");

        }
        
        #endregion

        #region 舰船检测源程序
        private void button3_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            process1 = new Process();
            process1.StartInfo.FileName = "cmd.exe";
            process1.StartInfo.WorkingDirectory = ".";
            //新加入可以得到exe结束信息的代码
            process1.EnableRaisingEvents = true;
            process1.Exited += new EventHandler(process1_Exited);

            process1.StartInfo.UseShellExecute = false;
            process1.StartInfo.RedirectStandardInput = true;
            process1.StartInfo.RedirectStandardOutput = true;
            process1.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            process1.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process1.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process1.StandardInput.WriteLine(disk_pos);
            process1.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_detection\\");
            process1.StandardInput.WriteLine(".\\SD_Demo_JL1.exe");
            //process.StandardInput.WriteLine("exit");

            process1.BeginOutputReadLine();
            process1.CloseMainWindow();
            process1.StandardInput.WriteLine(">Finish");
            process1.StandardInput.WriteLine("exit");
            // process.
            process1.WaitForExit();
            //process.WaitForExit();

        }
        private void process4_Exited(object sender, EventArgs e)
        {
            //MessageBox.Show("Slice Finished");
        }

        private void process1_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("Detection Finished");
            string originfilePath = PluginfoldPath + @"\result\img_cresult.tiff";
            MessageBox.Show("Recognition result in " + originfilePath);

            //将图像路径写出，做缩略图
            StreamWriter image_path = new StreamWriter(PluginfoldPath + @"\config\image_path.txt", false, Encoding.Default);
            image_path.Write("<ImagePath>" + "\r\n");
            image_path.Write(originfilePath + "\r\n");
            image_path.Close();
            MessageBox.Show("image_path.txt finished");

            //缩略图
            Control.CheckForIllegalCrossThreadCalls = false;
            process4 = new Process();
            process4.StartInfo.FileName = "cmd.exe";
            process4.StartInfo.WorkingDirectory = ".";
            //新加入可以得到exe结束信息的代码
            process4.EnableRaisingEvents = true;
            process4.Exited += new EventHandler(process4_Exited);

            process4.StartInfo.UseShellExecute = false;
            process4.StartInfo.RedirectStandardInput = true;
            process4.StartInfo.RedirectStandardOutput = true;
            process4.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            process4.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process4.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process4.StandardInput.WriteLine(disk_pos);
            process4.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_thumbnail\\");
            process4.StandardInput.WriteLine(".\\pyramid.exe");
            //process.StandardInput.WriteLine("exit");

            process4.BeginOutputReadLine();
            process4.CloseMainWindow();
            process4.StandardInput.WriteLine(">Finish");
            process4.StandardInput.WriteLine("exit");
            // process.
            process4.WaitForExit();
            //缩略图

            //显示缩略图
            this.pictureBox1.Image = Image.FromFile(PluginfoldPath + @"\bin_readVIF\imgthumb.tif");
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;//设置图片显示方式 
            //
           
        }

        #endregion
        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
      
       
     

        

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_2(object sender, EventArgs e)
        {

        }

     
        
        

        

       
       

        

       

       
        

      

       
      

        

      






    }
}

