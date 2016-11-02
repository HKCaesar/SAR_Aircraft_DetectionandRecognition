using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace configuration
{
    public partial class Form1 : Form
    {
        public Process process = null;
        public Process processS = null;
        public Process process1 = null;
        public Process process2 = null;

        public string PluginfoldPath, imagepath;
       
        public Form1()
        {
            InitializeComponent();
        }
        #region 载入图像
        private void button1_Click(object sender, EventArgs e)
        {
            
            //选择文件
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            //文件格式
            openFileDialog.Filter = "所有文件|*.*";
            //还原当前目录
            openFileDialog.RestoreDirectory = true;
            //默认的文件格式
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagepath = openFileDialog.FileName;
            this.pictureBox1.Image = Image.FromFile(imagepath);//动态加载图片  
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;//设置图片显示方式 
            }

            
        }
        #endregion

        #region 图片缩放和拖拽
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
        #region 选择插件文件夹路径
        private void process_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("config.txt Finished");
            MessageBox.Show("imageINFO.txt Finished");
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
      
       private void button2_Click(object sender, EventArgs e)
        {
           #region 选择插件路径
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
           #endregion

            #region 书写path.txt
            StreamWriter path = new StreamWriter(PluginfoldPath + @".\config\path.txt", false, Encoding.Default);
            path.Write("<ImagePath>" + "\r\n");
            path.Write(imagepath + "\r\n");
            path.Write("<PluginfoldPath>" + "\r\n");
            path.Write(PluginfoldPath + "\r\n");
            path.Close();
            
            MessageBox.Show("path.txt finished");
            #endregion


            #region 书写config.txt
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
            process.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_config\\");
            process.StandardInput.WriteLine(".\\myproject_gdal_vehicle.exe");
            //process.StandardInput.WriteLine("exit");

            process.BeginOutputReadLine();
            process.CloseMainWindow();
            process.StandardInput.WriteLine(">Finish");
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
            #endregion
            #region 分辨率修改
            //界面分辨率的输入
            StreamReader config = new StreamReader(PluginfoldPath + @".\config\config.txt", Encoding.UTF8);
            string str;
            while ((str = config.ReadLine()) != null)
            {
                if (str == "<PixelSize>")
                {
                    str = config.ReadLine();
                    //label3.Text = str;
                    textBox2.Text = null;
                    textBox2.AppendText(str);
                }
            }

            config.Close();

            #endregion
            //StreamReader config = new StreamReader(PluginfoldPath + @".\config\config.txt", false, Encoding.Default);
          

        }

        #endregion
       
        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string a = this.comboBox1.SelectedItem.ToString();
            StreamWriter platchoose = new StreamWriter(PluginfoldPath + @".\config\platchoose.txt", false, Encoding.Default);
            platchoose.Write(a + "\r\n");
            platchoose.Close();
            MessageBox.Show("平台" + a + "设置成功");
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
        private void button6_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            processS = new Process();
            processS.StartInfo.FileName = "cmd.exe";
            processS.StartInfo.WorkingDirectory = ".";
            processS.StartInfo.UseShellExecute = false;
            processS.StartInfo.RedirectStandardInput = true;
            processS.StartInfo.RedirectStandardOutput = true;
            processS.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            processS.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            processS.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process.StandardInput.WriteLine(disk_pos);
            processS.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_recognition\\");
            processS.StandardInput.WriteLine(".\\VehicleRec.exe");
            //process.StandardInput.WriteLine("exit");

            processS.BeginOutputReadLine();
            processS.CloseMainWindow();
            MessageBox.Show("Vehicle Recognition Finished");
            //因为没有丁拥科的源代码，无法改变路径，采取将原路径下的所有文件进行拷贝
            string originfilePath = PluginfoldPath + @"\bin_readVIF\test\";
            string destinationPath = PluginfoldPath + @"\result\";
            CopyIniFileToObject(originfilePath, destinationPath);
            MessageBox.Show("Recognition result in " + originfilePath);
        }


        private void process1_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("Detection Finished");
            string originfilePath = PluginfoldPath + @"\bin_detection\08res_tar_对象化结果.tiff";
            MessageBox.Show("Recognition result in " + originfilePath);
            this.pictureBox1.Image = Image.FromFile(originfilePath);//动态加载图片  
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;//设置图片显示方式  
        }
        private void button4_Click(object sender, EventArgs e)
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
            process1.StandardInput.WriteLine(".\\TD_demo_car.exe");
            //process.StandardInput.WriteLine("exit");

            process1.BeginOutputReadLine();
            process1.CloseMainWindow();
            process1.StandardInput.WriteLine(">Finish");
            process1.StandardInput.WriteLine("exit");
            // process.
            process1.WaitForExit();
            //process.WaitForExit();
           
        }
        private void processS_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("Slice Finished");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            processS = new Process();
            processS.StartInfo.FileName = "cmd.exe";
            processS.StartInfo.WorkingDirectory = ".";
            processS.StartInfo.UseShellExecute = false;
            processS.StartInfo.RedirectStandardInput = true;
            processS.StartInfo.RedirectStandardOutput = true;
            processS.StartInfo.CreateNoWindow = true;
            //新加入可以得到exe结束信息的代码
            processS.EnableRaisingEvents = true;
            processS.Exited += new EventHandler(processS_Exited);
            //Process.Start("cmd.exe");
            processS.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            processS.Start();

            string[] route_str = PluginfoldPath.Split(':');
            string disk_pos = route_str[0].ToString() + ":";
            process.StandardInput.WriteLine(disk_pos);
            processS.StandardInput.WriteLine("cd " + PluginfoldPath + @".\\bin_readVIF\\");
            processS.StandardInput.WriteLine(".\\detection_slice.exe");
            //process.StandardInput.WriteLine("exit");

            processS.BeginOutputReadLine();
            processS.CloseMainWindow();
            process.StandardInput.WriteLine(">Finish");
            processS.StandardInput.WriteLine("exit");
            // process.
            processS.WaitForExit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void process2_Exited(object sender, EventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
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
            
            //StreamReader config = new StreamReader(PluginfoldPath + @".\config\config.txt", Encoding.UTF8);
            //string str;
            //int i = 0;
            //while ((str = config.ReadLine()) != null)
            //{
            //    i++;
            //    if (str == "<PixelSize>")
            //    {

            //        break;

            //    }

            //}
            //config.Close();
            
           
            //string[] ary = File.ReadAllLines(PluginfoldPath + @".\config\config.txt", Encoding.Default);
            //ary[i] = textBox2.Text;
            //string stri = string.Join("\r\n", ary);
            //File.WriteAllText(PluginfoldPath + @".\config\config.txt", stri);
            //StreamReader config = new StreamReader(PluginfoldPath + @".\config\config.txt", Encoding.UTF8);

            //StreamWriter config_W = new StreamWriter(PluginfoldPath + @".\config\config.txt", false, Encoding.Default);

            MessageBox.Show("分辨率修改成功");
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
       }

    }

