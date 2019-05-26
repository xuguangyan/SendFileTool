using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SendFileCommon
{
    /// <summary>
    /// 控件委托调用类
    /// </summary>
    public class InvokeCtrl
    {

        //委托类：获取控件文本
        delegate string GetCtrlTextDelegate(Control ctrl);
        /// <summary>
        /// 获取控件文本
        /// </summary>
        /// <param name="value"></param>
        public static string GetCtrlText(Control ctrl)
        {
            string text = "";
            if (ctrl.InvokeRequired)
            {
                GetCtrlTextDelegate d = new GetCtrlTextDelegate(GetCtrlText);
                text = (string)ctrl.Invoke(d, ctrl);
            }
            else
            {
                text = ctrl.Text;
            }
            return text;
        }

        //委托类：更新控件文本
        delegate void UpdateCtrlTextDelegate(Control ctrl, string value);
        /// <summary>
        /// 更新控件文本
        /// </summary>
        /// <param name="value"></param>
        public static void UpdateCtrlText(Control ctrl, string value)
        {
            if (ctrl.InvokeRequired)
            {
                UpdateCtrlTextDelegate d = new UpdateCtrlTextDelegate(UpdateCtrlText);
                ctrl.Invoke(d, ctrl, value);
            }
            else
            {
                ctrl.Text = value;
            }
        }

        //委托类：更新控件可用状态
        delegate void UpdateCtrlEnabledDelegate(Control ctrl, bool value);
        /// <summary>
        /// 更新控件可用状态
        /// </summary>
        /// <param name="value"></param>
        public static void UpdateCtrlEnabled(Control ctrl, bool value)
        {
            if (ctrl.InvokeRequired)
            {
                UpdateCtrlEnabledDelegate d = new UpdateCtrlEnabledDelegate(UpdateCtrlEnabled);
                ctrl.Invoke(d, ctrl, value);
            }
            else
            {
                ctrl.Enabled = value;
            }
        }

        //委托类：更新滚动条
        delegate void UpdatePrgBarValueDelegate(CustomProgressBar ctrl, int value, Color foreColor, string text);
        /// <summary>
        /// 更新滚动条
        /// </summary>
        /// <param name="value"></param>
        public static void UpdatePrgBarValue(CustomProgressBar ctrl, int value, Color foreColor, string text)
        {
            if (ctrl.InvokeRequired)
            {
                UpdatePrgBarValueDelegate d = new UpdatePrgBarValueDelegate(UpdatePrgBarValue);
                ctrl.Invoke(d, ctrl, value, foreColor, text);
            }
            else
            {
                ctrl.Value = value;
                ctrl.ForeColor = foreColor;
                ctrl.Text = text;
            }
        }
    }
}
