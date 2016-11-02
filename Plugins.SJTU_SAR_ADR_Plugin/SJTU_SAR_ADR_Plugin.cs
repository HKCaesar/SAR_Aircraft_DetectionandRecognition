using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOCore;
using PluginCore;



namespace Plugins.SJTU_SAR_ADR_Plugin
{
    [GxPlugin("SAR_ADR")]
    public class SAR_ADR_Plugin : Plugin
    {
        public static GxPluginInfo GetRegisterPluginInfo()
        {
            return new GxPluginInfo("SAR_ADR", null);
        }

        public override void InitGui()
        {
            if (GisAPP == null)
            {
                return;
            }
            GxActionGroup topGroup = new GxActionGroup("军事目标识别技术");
            topGroup.Name = "军事目标识别技术";
            topGroup.Priority = 4;

            GxActionGroup group = new GxActionGroup("SAR航空器_SJTU");
            group.Priority = 5;
            group.Name = "SAR航空器_SJTU";

            
            
            GxAction action = new GxAction("飞机检测识别");
            string icon_path = System.Windows.Forms.Application.StartupPath+"\\icons\\";
            action.Icon = System.Drawing.Bitmap.FromFile(icon_path + "飞机icon.png");
            action.Name = "飞机检测识别";
            action.Priority = 8;
            action.OnExecuted += new EventHandler(SAR_ADR_Click);

            group.AddAction(action);
            topGroup.AddAction(group);
            GisAPP.MennActionGroup.AddAction(topGroup);

        }

        private void SAR_ADR_Click(object sender, EventArgs e)
        {
            ///(0)从平台抓取信息
            //窗体命名为Detection
            Main_WinForm form = new Main_WinForm();
            form.Show();

            ///(1)调用DLL的方法
            //ClassificationUI form = new ClassificationUI();
            //form.ShowDialog();
            //System.Windows.Forms.MessageBox.Show("hello wrold！");

            ///(2)调用CMD的方法
            //Form1 form = new Form1();
            //form.Show();

        }

    }
}
