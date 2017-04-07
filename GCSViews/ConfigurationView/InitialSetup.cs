using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class InitialSetup : UserControl
    {
        public InitialSetup()
        {
            InitializeComponent();
            tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += new DrawItemEventHandler(this.TabControl_DrawItem);
        }
        private void TabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Font fntTab;
            Brush bshBack;
            Brush bshFore;
            if (e.Index == this.tabControl1.SelectedIndex)
            {
                fntTab = new Font(new FontFamily("华文楷体"), e.Font.Size-2, FontStyle.Regular);
                //bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, SystemColors.Control, SystemColors.Control, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                bshBack = new SolidBrush(Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(151)))), ((int)(((byte)(248))))));
                bshFore = Brushes.White;
            }
            else
            {
                fntTab = new Font(new FontFamily("华文楷体"), e.Font.Size-2, FontStyle.Regular);
                bshBack = new SolidBrush(Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200))))));
                bshFore = new SolidBrush(Color.Black);
            }
            string tabName = tabControl1.TabPages[e.Index].Text;
            StringFormat sftTab = new StringFormat();
            e.Graphics.FillRectangle(bshBack, e.Bounds);
            Rectangle recTab = e.Bounds;
            recTab = new Rectangle(recTab.X, recTab.Y + 4, recTab.Width, recTab.Height - 4);
            e.Graphics.DrawString(tabName, fntTab, bshFore, recTab, sftTab);
        }
    }
}
