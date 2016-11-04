using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;


namespace STJU_SAR_ADR_UI
{
    public partial class Main_From : Form
    {
        //定义需要调用的进程
        public Process process = null;
        public Process processS = null;
        public Process process_Thumbnail = null;
        public Process process_ReadImageInfo = null;

        //需要的相关路径变量，图像信息变量声明
        public string PL_img_path, PL_img_XML_path, Desktop_Path, PluginfoldPath, ImageFolder,ImagePath,img_Name;

        //信息窗口显示信息变化
        private void InfoMonitor_TextChanged(object sender, EventArgs e)
        {

        }

        //将EXE中显示的信息加入信息显示窗口，自动滚动至最后一行
        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                StringBuilder detectionEXEInfo = new StringBuilder(this.InfoMonitor.Text);
                this.InfoMonitor.Text = detectionEXEInfo.AppendLine(outLine.Data).ToString();
                this.InfoMonitor.SelectionStart = this.InfoMonitor.Text.Length;
                this.InfoMonitor.ScrollToCaret();
            }
        }

        #region 图片的缩放和拖拽
        private bool canMove = false;
        private Point mousePos;
        //鼠标移动
        private void ImageMonitor_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.canMove)
            {
                ImageMonitor.Location = new Point(ImageMonitor.Location.X
                                     - mousePos.X + e.X, ImageMonitor.Location.Y
                                     - mousePos.Y + e.Y);
            }
            this.ImageMonitor.Focus();
        }
        //鼠标释放
        private void ImageMonitor_MouseUp(object sender, MouseEventArgs e)
        {
            this.canMove = false;
        }
        //鼠标按下
        private void ImageMonitor_MouseDown(object sender, MouseEventArgs e)
        {
            this.canMove = true;
            this.mousePos = new Point(e.X, e.Y);//获得鼠标按下时刻坐标
        }
        //鼠标中轴滚轮
        private void ImageMonitor_MouseWheel(object sender, MouseEventArgs e)
        {
            //向前滚动
            float width = this.ImageMonitor.Width;
            float height = this.ImageMonitor.Height;
            float centerx = this.ImageMonitor.Location.X + width / 2;
            float centery = this.ImageMonitor.Location.Y + height / 2;
            if (e.Delta > 0)
            {

                float w = this.ImageMonitor.Width * 0.95f; //每次缩小为原来的0.9025
                float h = this.ImageMonitor.Height * 0.95f;

                this.ImageMonitor.Size = Size.Ceiling(new SizeF(w, h));
                this.ImageMonitor.Location = new Point(Convert.ToInt32(centerx - (w / 2)), Convert.ToInt32(centery - (h / 2)));//图片居中缩放
            }
            //向后滚动
            else if (e.Delta < 0)
            {

                float w = this.ImageMonitor.Width * 1.05f; //每次放大为原来的1.1025
                float h = this.ImageMonitor.Height * 1.05f;
                this.ImageMonitor.Size = Size.Ceiling(new SizeF(w, h));
                this.ImageMonitor.Location = new Point(Convert.ToInt32(centerx - (w / 2)), Convert.ToInt32(centery - (h / 2)));//图片居中缩放
                ImageMonitor.Invalidate();

            }
        }

        #endregion

        //GetThumbImageFromPyramid.exe制作缩略图程序的结束信息
        private void process_Thumbnail_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("成功载入图像");
        }

        //检测进程结束后显示结果图片
        private void process_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("Detection Finished");
            string originfilePath = ImageFolder + @"\Final_result.tiff";
            MessageBox.Show("Result image is " + originfilePath);
            this.ImageMonitor.Image = Image.FromFile(originfilePath);
            //动态加载结果图片，结果图片为彩色图片，默认不需要缩略图
            this.ImageMonitor.SizeMode = PictureBoxSizeMode.Zoom;//设置图片显示方式 
        }

        //GDAL_ReadImageInfo.exe运行结束的提示信息
        private void process_ReadImageInfo_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("成功利用GDAL读取图像信息");
        }
        
        //载入图像按钮
        private void LoadImageBtn_Click(object sender, EventArgs e)
        {
            #region 打开文件夹面板，选取图像
            //选择文件
            OpenFileDialog openFileDialog_SARImage = new OpenFileDialog();
            openFileDialog_SARImage.Multiselect = true;
            //文件格式
            openFileDialog_SARImage.Filter = "SAR图像文件|*.tif;*.tiff;*.TIF;*.TIFF";
            //还原当前目录
            openFileDialog_SARImage.RestoreDirectory = true;
            openFileDialog_SARImage.FilterIndex = 1;
            if (openFileDialog_SARImage.ShowDialog() == DialogResult.OK)
            {
                ImagePath = openFileDialog_SARImage.FileName;
                img_Name = System.IO.Path.GetFileNameWithoutExtension(ImagePath);
            }
            #endregion

            #region 将图像路径写出,做缩略图
            //获取UI.exe所在路径
            PluginfoldPath = System.Windows.Forms.Application.StartupPath;
            //创建文件夹树
            ImageFolder = PluginfoldPath + @"\SAR_Result\" + img_Name;
            if (!Directory.Exists(ImageFolder))
            { Directory.CreateDirectory(ImageFolder); }

            StreamWriter image_path = new StreamWriter(PluginfoldPath+@"\config\image_path.txt", false, Encoding.Default);
            image_path.Write("<ImagePath>" + "\r\n");
            image_path.Write(ImagePath + "\r\n");
            image_path.Write("<PluginFolderPath>" + "\r\n");
            image_path.Write(PluginfoldPath + "\r\n");
            image_path.Close();
            //缩略图
            Control.CheckForIllegalCrossThreadCalls = false;
            process_Thumbnail = new Process();
            process_Thumbnail.StartInfo.FileName = "cmd.exe";
            process_Thumbnail.StartInfo.WorkingDirectory = ".";
            //新加入可以得到exe结束信息的代码
            process_Thumbnail.EnableRaisingEvents = true;
            process_Thumbnail.Exited += new EventHandler(process_Thumbnail_Exited);

            process_Thumbnail.StartInfo.UseShellExecute = false;
            process_Thumbnail.StartInfo.RedirectStandardInput = true;
            process_Thumbnail.StartInfo.RedirectStandardOutput = true;
            process_Thumbnail.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            process_Thumbnail.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process_Thumbnail.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process_Thumbnail.StandardInput.WriteLine(disk_pos);
            process_Thumbnail.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_thumbnail\\");
            process_Thumbnail.StandardInput.WriteLine(".\\GetThumbImageFromPyramid.exe");
            //process.StandardInput.WriteLine("exit");

            process_Thumbnail.BeginOutputReadLine();
            process_Thumbnail.CloseMainWindow();
            process_Thumbnail.StandardInput.WriteLine(">Finish");
            process_Thumbnail.StandardInput.WriteLine("exit");
            // process.
            process_Thumbnail.WaitForExit();

            #endregion
            this.ImageMonitor.Image = Image.FromFile(PluginfoldPath + @"\bin_thumbnail\img_thumbnail.tiff");//动态加载缩略图片  
            this.ImageMonitor.SizeMode = PictureBoxSizeMode.Zoom;//设置图片显示方式 
        }

        //修改像元尺寸和遥感平台信息
        private void VerifyParamButton_Click(object sender, EventArgs e)
        {
            //修改ImageINFO.txt中的像元尺寸信息
            StreamReader str_change = new StreamReader(PluginfoldPath + @".\bin_readinfo\ImageINFO.txt", Encoding.UTF8);
            StringBuilder str_build = new StringBuilder();
            string line = str_change.ReadLine();
            while (line != null)
            {
                if (line == "<PixelSize>")
                {
                    str_build.Append(line + "\r\n");
                    line = str_change.ReadLine();
                    str_build.Append(pixelsizeTxtBox.Text + "\r\n");
                    line = str_change.ReadLine();
                }
                else
                {
                    str_build.Append(line + "\r\n");
                    line = str_change.ReadLine();
                }
            }
            str_change.Close();
            StreamWriter str_write = new StreamWriter(PluginfoldPath + @".\bin_readinfo\ImageINFO.txt", false, Encoding.UTF8);
            str_write.Write(str_build.ToString());
            str_write.Close();
            //修改config.txt中的像元尺寸信息
            StreamReader str_change_config = new StreamReader(PluginfoldPath + @".\config\config.txt", Encoding.UTF8);
            StringBuilder str_build_config = new StringBuilder();
            string con_line = str_change_config.ReadLine();
            while (con_line != null)
            {
                if (con_line == "<PixelSize>")
                {
                    str_build_config.Append(con_line + "\r\n");
                    con_line = str_change_config.ReadLine();
                    str_build_config.Append(pixelsizeTxtBox.Text + "\r\n");
                    con_line = str_change_config.ReadLine();
                }
                else if (con_line == "<Platform>")
                {
                    str_build_config.Append(con_line + "\r\n");
                    con_line = str_change_config.ReadLine();
                    str_build_config.Append(this.ChooseRSPlatformComboBox.SelectedItem.ToString() + "\r\n");
                    con_line = str_change_config.ReadLine();
                }
                else
                {
                    str_build_config.Append(con_line + "\r\n");
                    con_line = str_change_config.ReadLine();
                }
            }
            str_change_config.Close();
            StreamWriter str_write_config = new StreamWriter(PluginfoldPath + @".\config\config.txt", false, Encoding.UTF8);
            str_write_config.Write(str_build_config.ToString());
            str_write_config.Close();
            //显示完成情况
            MessageBox.Show("像元尺寸修改成" + this.pixelsizeTxtBox.Text);
            MessageBox.Show("平台" + this.ChooseRSPlatformComboBox.SelectedItem.ToString() + "设置成功");
        }

        //关闭整个窗口
        private void WinFormCloseButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //运行飞机检测程序EXE
        //一共有四条支路：TerraSAR，JB，MiniSAR，Unknown
        private void RunDetectionButton_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = ".";
            //新加入可以得到exe结束信息的代码
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(process_Exited);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process.StandardInput.WriteLine(disk_pos);
            process.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_detection\\");
            if (this.ChooseRSPlatformComboBox.SelectedItem.ToString() == "TerraSAR-X")
            {
                process.StandardInput.WriteLine(".\\SAR_ADR_TerraSAR_Algorithm.exe");
            }
            else if (this.ChooseRSPlatformComboBox.SelectedItem.ToString() == "J5/J7")
            {
                process.StandardInput.WriteLine(".\\SAR_ADR_JB_Algorithm.exe");
            }
            else if (this.ChooseRSPlatformComboBox.SelectedItem.ToString() == "MiniSAR")
            {
                process.StandardInput.WriteLine(".\\SAR_ADR_MiniSAR_Algorithm.exe");
            }
            else if (this.ChooseRSPlatformComboBox.SelectedItem.ToString() == "Unknown")
            {
                process.StandardInput.WriteLine(".\\SAR_ADR_Unknown_Algorithm.exe");
            }
            else
            {
                DialogResult PluginErrorMsg;
                PluginErrorMsg = MessageBox.Show("未找到适合的平台", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (PluginErrorMsg == DialogResult.OK)
                {
                    this.Dispose();
                }
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();
            }
            process.BeginOutputReadLine();
            process.CloseMainWindow();
            process.StandardInput.WriteLine(">Finish");
            process.StandardInput.WriteLine("exit");

            process.WaitForExit();

        }

        //获得图像信息按钮
        private void GetImageInfoButton_Click(object sender, EventArgs e)
        {
            #region 读取并记录图像信息,写入config.txt,ImageINFO.txt
            Control.CheckForIllegalCrossThreadCalls = false;
            process_ReadImageInfo = new Process();
            process_ReadImageInfo.StartInfo.FileName = "cmd.exe";
            process_ReadImageInfo.StartInfo.WorkingDirectory = ".";
            //新加入可以得到exe结束信息的代码
            process_ReadImageInfo.EnableRaisingEvents = true;
            process_ReadImageInfo.Exited += new EventHandler(process_ReadImageInfo_Exited);

            process_ReadImageInfo.StartInfo.UseShellExecute = false;
            process_ReadImageInfo.StartInfo.RedirectStandardInput = true;
            process_ReadImageInfo.StartInfo.RedirectStandardOutput = true;
            process_ReadImageInfo.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            process_ReadImageInfo.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process_ReadImageInfo.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process_ReadImageInfo.StandardInput.WriteLine(disk_pos);
            process_ReadImageInfo.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_readinfo\\");
            process_ReadImageInfo.StandardInput.WriteLine(".\\GDAL_ReadImageInfo.exe");




            process_ReadImageInfo.BeginOutputReadLine();
            process_ReadImageInfo.CloseMainWindow();
            process_ReadImageInfo.StandardInput.WriteLine(">Finish");
            process_ReadImageInfo.StandardInput.WriteLine("exit");

            process_ReadImageInfo.WaitForExit();
            #endregion

            #region 从.\\bin_readinfo\\ImageINFO中读取图像信息，写入pixelsizeTxtBox
            StreamReader sr = new StreamReader(PluginfoldPath + @".\bin_readinfo\ImageINFO.txt", Encoding.UTF8);
            string Rez_s = "";
            string sr_line = sr.ReadLine();
            while (sr_line != null)
            {
                if (sr_line == "<PixelSize>")
                {
                    sr_line = sr.ReadLine();
                    Rez_s = sr_line.ToString();
                }
                    sr_line = sr.ReadLine();
            }
            sr.Close();
            this.pixelsizeTxtBox.AppendText(Rez_s);
            #endregion
        }
        double PL_img_Rez, LatMin, LatMax, LongMin, LongMax;

        int PL_img_Height, PL_img_Width;

        public Main_From()
        {
            Desktop_Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            InitializeComponent();
        }

        private void Main_WinForm_Load(object sender, EventArgs e)
        {
            this.ImageMonitor.MouseWheel += new MouseEventHandler(ImageMonitor_MouseWheel);
        }
    }
}
