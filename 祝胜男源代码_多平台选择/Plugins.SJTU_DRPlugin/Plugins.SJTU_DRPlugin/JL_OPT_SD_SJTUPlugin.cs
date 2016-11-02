
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOCore;
using PluginCore;

namespace Plugins.helloworldPlugin
{
    [GxPlugin("Hello world")]//
    public class helloworldsPlugin : Plugin
    {
        public static GxPluginInfo GetRegisterPluginInfo()
        {
            return new GxPluginInfo("Hello world", null);
        }

        public override void InitGui()
        {
            GxActionGroup topGroup = new GxActionGroup("军事目标识别技术");
            topGroup.Name = "军事目标识别技术";
            topGroup.Priority = 4;

            GxActionGroup group = new GxActionGroup("吉林一号插件组");
            group.Priority = 2;
            group.Name = "吉林一号插件组";

            GxAction action = new GxAction("舰船检测(上海交大)");
            action.Name = "舰船检测(上海交大)";
            action.Priority = 19;
            action.OnExecuted += new EventHandler(Helloworld_Click);

            group.AddAction(action);
            topGroup.AddAction(group);
            GisAPP.MennActionGroup.AddAction(topGroup);

        }

        private void Helloworld_Click(object sender, EventArgs e)
        {
            ///(0)从平台抓取信息
            //窗体命名为Detection
            Detection form = new Detection();
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
