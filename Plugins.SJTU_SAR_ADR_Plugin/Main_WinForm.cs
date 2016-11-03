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
using Plugins.SJTU_SAR_ADR_Plugin;

namespace Plugins.SJTU_SAR_ADR_Plugin
{
    public partial class Main_WinForm : Form
    {
        public Process process = null;
        public Process processS = null;
        public Process process_Thumbnail = null;

        //需要的相关路径变量，图像信息变量声明
        public string PL_img_path, PL_img_XML_path, Desktop_Path, PluginfoldPath, ImageFolder;
        double PL_img_Rez, LatMin, LatMax, LongMin, LongMax;


        int PL_img_Height, PL_img_Width;

        public Main_WinForm()
        {
            Desktop_Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            InitializeComponent();
        }

        //选择插件文件夹按钮
        private void ChooseFolderButton_Click(object sender, EventArgs e)
        {
            #region 选择插件路径
            int pluginpath_Flag = 1;
            string pluginfoldpath = null;
            StreamReader PluginfoldPath_rl = new StreamReader(@"plugin_path.txt");
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
                    StreamWriter plugin_sw = new StreamWriter(@"plugin_path.txt", false);
                    PluginfoldPath = dialog.SelectedPath;
                    MessageBox.Show("已选择文件夹:" + PluginfoldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    plugin_sw.Write(PluginfoldPath);
                    plugin_sw.Close();
                }
            }
            #endregion

        }
        
        //信息窗口显示信息变化
        private void InfoMonitor_TextChanged(object sender, EventArgs e)
        {

        }
        
        //修改像元尺寸和遥感平台信息按钮
        private void VerifyParamButton_Click(object sender, EventArgs e)
        {
            //修改ImageINFO.txt中的像元尺寸信息
            StreamReader str_change = new StreamReader(PluginfoldPath + @".\bin_readinfo\ImageINFO.txt", Encoding.UTF8);
            StringBuilder str_build = new StringBuilder();
            string line = str_change.ReadLine();
            while(line != null) 
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
            StreamWriter str_write = new StreamWriter(PluginfoldPath + @".\bin_readinfo\ImageINFO.txt", false,Encoding.UTF8);
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
            MessageBox.Show("像元尺寸修改成" + this.pixelsizeTxtBox.Text );
            MessageBox.Show("平台" + this.ChooseRSPlatformComboBox.SelectedItem.ToString() + "设置成功");
        }

        //运行飞机检测程序
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
        //
        private void process_Thumbnail_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("缩略图制作完成");
        }
        //关闭窗口
        private void WinFormCloseButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
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

        //获得图像信息
        private void GetImageInfoButton_Click(object sender, EventArgs e)
        {
            if (SpatialDataViewer.ActiveViewControl != null || SpatialDataViewer.ActiveViewControl.View != null)
            {
                //确定当前文件夹路径
                string current_Path = Environment.CurrentDirectory;
                //确定插件文件夹路径
                this.InfoMonitor.AppendText("PluginfoldPath: " + PluginfoldPath + "\n");
                string AimFolder = PluginfoldPath;
                if (!Directory.Exists(AimFolder))
                { Directory.CreateDirectory(AimFolder); }
                GxView _v = SpatialDataViewer.ActiveViewControl.View;
                GxImageGraphicsItem image = _v.ImageLayer.EdittedImage;
                GxImageGraphicsItem image2 = image;
                //(1)图像名称
                string img_Name = image2.Name;
                //为每一幅图像创建文件夹树
                ImageFolder = AimFolder + @"\SAR_Result\"+img_Name;
                if (!Directory.Exists(ImageFolder))
                { Directory.CreateDirectory(ImageFolder); }
                StreamWriter sw = new StreamWriter(AimFolder + @".\bin_readinfo\ImageINFO.txt", false, Encoding.UTF8);
                //(2)通过GxImageGraphicsItem Class下的Properties获取各种图像信息
                //(2-1)图像的绝对路径名
                this.PL_img_path = image2.FileName;

                # region 将图像路径写出，做缩略图
                //
                StreamWriter image_path = new StreamWriter(PluginfoldPath + @"\config\image_path.txt", false, Encoding.Default);
                image_path.Write("<ImagePath>" + "\r\n");
                image_path.Write(PL_img_path + "\r\n");
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

                this.ImageMonitor.Image = Image.FromFile(PluginfoldPath + @".\\bin_thumbnail\\img_thumbnail.tiff");//动态加载缩略图片  
                this.ImageMonitor.SizeMode = PictureBoxSizeMode.Zoom;//设置图片显示方式 
                this.InfoMonitor.AppendText("ImagePath: " + PL_img_path + "\n");
                sw.Write("<SARImagePath>" + "\r\n");
                sw.Write(PL_img_path + "\r\n");
                sw.Write("<RefImgPath>" + "\r\n");
                sw.Write(ImageFolder + @"\img_en.tif" + "\r\n");
                this.InfoMonitor.AppendText("ImageName: " + img_Name + "\n");
                sw.Write("<OutputDir>" + "\r\n");
                sw.Write(ImageFolder + "\r\n");
                sw.Write("<ImageName>" + "\r\n");
                sw.Write(img_Name + "\r\n");
                //(2-2) 获取图像的分辨率
                PL_img_Rez = image2.ImgPR;
                double curPixel_size = image2.CurrentImageSource.PadfTransform[1];
                string Rez_s = PL_img_Rez.ToString();
                this.InfoMonitor.AppendText("ImageResolution: " + Rez_s + "\n");
                this.InfoMonitor.AppendText("ImagePixelSize: " + curPixel_size + "\n");
                this.pixelsizeTxtBox.AppendText(Rez_s);
                //(2-3) 获取图像尺寸信息
                PL_img_Height = image2.Height;
                PL_img_Width = image2.Width;
                string Height_s = PL_img_Height.ToString();
                string Width_s = PL_img_Width.ToString();
                this.InfoMonitor.AppendText("ImageHeight(Rows): " + Height_s + "\n");
                this.InfoMonitor.AppendText("ImageWidth(Columns): " + Width_s + "\n");
                sw.Write("<ImageHeight(Rows)>" + "\r\n");
                sw.Write(PL_img_Height + "\r\n");
                sw.Write("<ImageWidth(Cols)>" + "\r\n");
                sw.Write(PL_img_Width + "\r\n");
                //(2-4)获取平台中的地理经纬度（与图像旁边的一列表中的数据一致)
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
                this.InfoMonitor.AppendText("LatMin: " + LatMin + "\n");
                this.InfoMonitor.AppendText("LatMax: " + LatMax + "\n");
                this.InfoMonitor.AppendText("LongMin: " + LongMin + "\n");
                this.InfoMonitor.AppendText("LongMax: " + LongMax + "\n");
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
                sw.Write("<CooridinateDelta>" + "\r\n");
                sw.Write(0 + "\r\n");
                sw.Close();
                //用C#写config.txt
                StreamWriter config_W = new StreamWriter(AimFolder + @".\config\config.txt", false, Encoding.UTF8);
                config_W.Write("<SARImagePath>" + "\r\n");
                config_W.Write(PL_img_path + "\r\n");
                config_W.Write("<SARImageName>" + "\r\n");
                config_W.Write(img_Name + "\r\n");
                config_W.Write("<MaskPath>" + "\r\n");
                config_W.Write(ImageFolder + @"\SAR_maskimg.tif" + "\r\n");
                config_W.Write("<RefImgPath>" + "\r\n");
                config_W.Write(ImageFolder + @"\SAR_refimg.tif" + "\r\n");
                config_W.Write("<TemplatePath>" + "\r\n");
                config_W.Write(AimFolder + @"\Template\" + "\r\n");
                config_W.Write("<ResultImagePath>" + "\r\n");
                config_W.Write(ImageFolder + @"\" + "\r\n");
                config_W.Write("<ResultXmlPath>" + "\r\n");
                config_W.Write(ImageFolder + @"\" + img_Name + "_SAR.xml" + "\r\n");
                config_W.Write("<SARFusionXmlPath>" + "\r\n");
                config_W.Write(ImageFolder + @"\" + img_Name + "_SAR_Fusion.xml" + "\r\n");
                config_W.Write("<Opt_ResultTXTPath>" + "\r\n");
                config_W.Write(ImageFolder + @"\Opt_result.txt" + "\r\n");
                config_W.Write("<SAR_ResultTXTPath>" + "\r\n");
                config_W.Write(ImageFolder + @"\SAR_result.txt" + "\r\n");

                config_W.Write("<Platform>" + "\r\n");
                config_W.Write("Unknown" + "\r\n");
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
            }
            MessageBox.Show("图像信息读取操作结束");
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



        private void Main_WinForm_Load(object sender, EventArgs e)
        {
            this.ImageMonitor.MouseWheel += new MouseEventHandler(ImageMonitor_MouseWheel);
        }
    }
}
