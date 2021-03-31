using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAppBrowser
{

    /// <summary> 
    /// 重写的TabControl控件 带关闭按钮
    /// </summary>
    public class MyTabControl : TabControl
    {
        private int iconWidth = 16;
        private int iconHeight = 16;
        private Image icon = null;
        private Brush biaocolor = Brushes.LightGray; //选项卡的背景色
        public bool Selected = true;
        Color noSelectedColor = Color.LightGray;
        //private Form_paint father;//父窗口，即绘图界面，为的是当选项卡全关后调用父窗口的dispose事件关闭父窗口
        //private AxMicrosoft.Office.Interop.VisOcx.AxDrawingControl axDrawingControl1;
        public MyTabControl()
            : base()
        {
            //this.axDrawingControl1 = axDrawingControl;
            this.SizeMode= TabSizeMode.Fixed;
            this.ItemSize = new Size(150, 25); //设置选项卡标签的大小,可改变高不可改变宽  
            //this.Appearance = TabAppearance.Buttons; //选项卡的显示模式 
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            
            icon = Properties.Resources.close.ToBitmap();
            iconWidth = icon.Width; iconHeight = icon.Height;
        }

        /// <summary>
        /// 解决TabControl 页里面多余边距问题
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                return new Rectangle(rect.Left - 2, rect.Top + 0, rect.Width + 2, rect.Height + 1);
            }
        }

        /// <summary>
        /// 重写的绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)//重写绘制事件。
        {
            Graphics g = e.Graphics;
            Rectangle r = GetTabRect(e.Index);
            if (e.Index == this.SelectedIndex)    //当前选中的Tab页，设置不同的样式以示选中
            {
                Brush selected_color = Brushes.White; //选中的项的背景色
                g.FillRectangle(selected_color, r); //改变选项卡标签的背景色 
                string title = this.TabPages[e.Index].Text + "　　　　";
                Rectangle rectFont = new Rectangle(r.X, r.Y + 4, r.Width - 10, r.Height - 10);
                Rectangle rectFontLinearBrush = new Rectangle(rectFont.X + rectFont.Width - 5, r.Y + 6, 15, r.Height - 10);
                drawString(g, rectFont, rectFontLinearBrush, title, this.Font);
                //g.DrawString(title, this.Font, new SolidBrush(Color.Black), new PointF(r.X + 3, r.Y + 6));//PointF选项卡标题的位置 
                r.Offset(r.Width - iconWidth - 3, 2);
                g.DrawImage(icon, new Point(r.X, r.Y));//选项卡上的图标的位置 fntTab = new System.Drawing.Font(e.Font, FontStyle.Bold);
            }
            else//非选中的
            {
                g.FillRectangle(biaocolor, r); //改变选项卡标签的背景色 
                string title = this.TabPages[e.Index].Text + "　　　　";
                Rectangle rectFont = new Rectangle(r.X, r.Y + 4, r.Width - 10, r.Height - 10);
                Rectangle rectFontLinearBrush = new Rectangle(rectFont.X + rectFont.Width - 5, r.Y + 6, 15, r.Height - 10);
                this.Selected = false;
                drawString(g, rectFont, rectFontLinearBrush, title, this.Font);
                //g.DrawString(title, this.Font, new SolidBrush(Color.Black), new PointF(r.X + 3, r.Y + 6));//PointF选项卡标题的位置 
                r.Offset(r.Width - iconWidth - 3, 2);
                //g.DrawImage(icon, new Point(r.X, r.Y));//选项卡上的图标的位置 
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            #region 左键判断是否在关闭区域
            if (e.Button == MouseButtons.Left)
            {
                Point p = e.Location;
                Rectangle r = GetTabRect(this.SelectedIndex);
                r.Offset(r.Width - iconWidth - 3, 2);
                r.Width = iconWidth;
                r.Height = iconHeight;
                if (r.Contains(p)) //点击特定区域时才发生 
                {
                   
                    if (this.TabCount == 1)//是最后一个选项卡，直接关闭父界面，即画图界面
                    {
                        MessageBox.Show("最后一个选项卡");
                    }
                    else//不是最后一个
                    {
                        this.TabPages.Remove(this.SelectedTab);
                    }
                }
            }
            #endregion
            #region 右键 选中
            else if (e.Button == MouseButtons.Right)    //  右键选中
            {
                for (int i = 0; i < this.TabPages.Count; i++)
                {
                    TabPage tp = this.TabPages[i];
                    if (this.GetTabRect(i).Contains(new Point(e.X, e.Y)))
                    {
                        this.SelectedTab = tp;
                        break;
                    }
                }
            }
            #endregion
            #region 中键 选中 关闭
            else if (e.Button == MouseButtons.Middle)//鼠标中键关闭
            {
                for (int i = 0; i < this.TabPages.Count; i++)
                {
                    TabPage tp = this.TabPages[i];
                    if (this.GetTabRect(i).Contains(new Point(e.X, e.Y)))//找到后，关闭
                    {
                        this.SelectedTab = tp;
                        
                        if (this.TabCount == 1)//是最后一个选项卡，直接关闭父界面，即画图界面
                        {
                            MessageBox.Show("最后一个选项卡");
                        }
                        else//不是最后一个
                        {
                            this.TabPages.Remove(this.SelectedTab);
                        }
                        break;
                    }
                }
            }
            #endregion
        }

        public void drawString(Graphics g, Rectangle rect, Rectangle rectFontLinearBrush, string title, Font font)
        {
            g.DrawString(title, font, new SolidBrush(Color.Black), rect);

            using (LinearGradientBrush brush = new LinearGradientBrush(rectFontLinearBrush, Color.Transparent, Selected ? Color.White : noSelectedColor, 0, false))
            {
                g.FillRectangle(brush, rectFontLinearBrush);
            }
        }

    }

}
