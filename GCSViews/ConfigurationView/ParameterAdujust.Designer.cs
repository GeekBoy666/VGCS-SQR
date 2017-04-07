namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ParameterAdujust
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.configSimplePids1 = new MissionPlanner.GCSViews.ConfigurationView.ConfigSimplePids();
            this.configFriendlyParamsAdv1 = new MissionPlanner.GCSViews.ConfigurationView.ConfigFriendlyParamsAdv();
            this.configRawParams1 = new MissionPlanner.GCSViews.ConfigurationView.ConfigRawParams();
            this.configPlanner1 = new MissionPlanner.GCSViews.ConfigurationView.ConfigPlanner();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1149, 690);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.configSimplePids1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1116, 682);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本调参";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.configFriendlyParamsAdv1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1116, 682);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "扩展调参";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.configRawParams1);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1116, 682);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "标准参数";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.configPlanner1);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1116, 682);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Planner";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // configSimplePids1
            // 
            this.configSimplePids1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configSimplePids1.Font = new System.Drawing.Font("华文楷体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.configSimplePids1.Location = new System.Drawing.Point(3, 3);
            this.configSimplePids1.Name = "configSimplePids1";
            this.configSimplePids1.Size = new System.Drawing.Size(1110, 676);
            this.configSimplePids1.TabIndex = 0;
            // 
            // configFriendlyParamsAdv1
            // 
            this.configFriendlyParamsAdv1.AutoSize = true;
            this.configFriendlyParamsAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configFriendlyParamsAdv1.Font = new System.Drawing.Font("华文楷体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.configFriendlyParamsAdv1.Location = new System.Drawing.Point(3, 3);
            this.configFriendlyParamsAdv1.Name = "configFriendlyParamsAdv1";
            this.configFriendlyParamsAdv1.ParameterMode = "Advanced";
            this.configFriendlyParamsAdv1.Size = new System.Drawing.Size(1110, 676);
            this.configFriendlyParamsAdv1.TabIndex = 0;
            // 
            // configRawParams1
            // 
            this.configRawParams1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configRawParams1.Font = new System.Drawing.Font("华文楷体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.configRawParams1.Location = new System.Drawing.Point(0, 0);
            this.configRawParams1.Name = "configRawParams1";
            this.configRawParams1.Size = new System.Drawing.Size(1116, 682);
            this.configRawParams1.TabIndex = 0;
            // 
            // configPlanner1
            // 
            this.configPlanner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configPlanner1.Font = new System.Drawing.Font("华文楷体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.configPlanner1.Location = new System.Drawing.Point(0, 0);
            this.configPlanner1.Name = "configPlanner1";
            this.configPlanner1.Size = new System.Drawing.Size(1116, 682);
            this.configPlanner1.TabIndex = 0;
            // 
            // ParameterAdujust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ParameterAdujust";
            this.Size = new System.Drawing.Size(1149, 690);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ConfigSimplePids configSimplePids1;
        private ConfigFriendlyParamsAdv configFriendlyParamsAdv1;
        private System.Windows.Forms.TabPage tabPage3;
        private ConfigRawParams configRawParams1;
        private System.Windows.Forms.TabPage tabPage4;
        private ConfigPlanner configPlanner1;
    }
}
