using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SendFileCommon
{
    /// <summary>
    /// 自定义进度条
    /// </summary>
    [ToolboxBitmap(typeof(ProgressBar))]
    public class CustomProgressBar : ProgressBar
    {
        public CustomProgressBar() {
            //使用自定义绘制
            this.SetStyle(ControlStyles.UserPaint, true);
            //绘制时先在内存中绘制
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        //进度条显示文本
        private string _text;

        [Browsable(true)]
        [Description("显示进度条中间的文本"), Category("Appearance")]
        public override string Text
        {
            set
            {
                this._text = value;
            }
            get
            {
                return this._text;
            }
        }

        //进度条显示文本
        private Font _textFont = new Font("宋体", 9);

        [Browsable(true)]
        [Description("进度条中间文本的字体"), Category("Appearance")]
        public Font TextFont
        {
            set
            {
                this._textFont = value;
            }
            get
            {
                return this._textFont;
            }
        }
        //进度条显示文本
        private Color _textColor = Color.Black;

        [Browsable(true)]
        [Description("进度条中间文本的颜色"), Category("Appearance")]
        public Color TextColor
        {
            set
            {
                this._textColor = value;
            }
            get
            {
                return this._textColor;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush brush = null;
            Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);

            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);

            Pen pen = new Pen(this.ForeColor, 1);
            //绘制边框
            e.Graphics.DrawRectangle(pen, rec);
            //绘制背景
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 2, 2, rec.Width - 4, rec.Height - 4);

            rec.Height -= 4;
            rec.Width = (int)(rec.Width * ((double)this.Value / this.Maximum)) - 4;
            brush = new SolidBrush(this.ForeColor);
            //绘制当前进度
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);

            if (_text != null && this._text.Length > 0)
            {
                SizeF vSizeF = e.Graphics.MeasureString(_text, _textFont);
                int dStrLen = Convert.ToInt32(Math.Ceiling(vSizeF.Width));


                brush = new SolidBrush(_textColor);
                Point point = new Point(Width / 2 - dStrLen / 2, Height / 2 - _textFont.Height / 2);
                e.Graphics.DrawString(_text, _textFont, brush, point);
            }
        }
    }
}
