namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class Calibration
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Calibration));
            this.label1 = new System.Windows.Forms.Label();
            this.BUT_level = new MissionPlanner.Controls.MyButton();
            this.lbl_Accel_user = new System.Windows.Forms.Label();
            this.BUT_calib_accell = new MissionPlanner.Controls.MyButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BUT_MagCalibrationLive = new MissionPlanner.Controls.MyButton();
            this.LBL_primary_compass = new System.Windows.Forms.Label();
            this.LBL_compass1_mot = new System.Windows.Forms.Label();
            this.LBL_compass1_offset = new System.Windows.Forms.Label();
            this.LBL_compass2_mot = new System.Windows.Forms.Label();
            this.LBL_compass3_mot = new System.Windows.Forms.Label();
            this.LBL_compass3_offset = new System.Windows.Forms.Label();
            this.LBL_compass2_offset = new System.Windows.Forms.Label();
            this.groupBoxCompass3 = new System.Windows.Forms.GroupBox();
            this.CHK_compass3_external = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_compass3_use = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CMB_compass3_orient = new MissionPlanner.Controls.MavlinkComboBox();
            this.CHK_autodec = new System.Windows.Forms.CheckBox();
            this.TXT_declination_deg = new System.Windows.Forms.TextBox();
            this.linkLabelmagdec = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TXT_declination_min = new System.Windows.Forms.TextBox();
            this.groupBoxCompass2 = new System.Windows.Forms.GroupBox();
            this.CHK_compass2_external = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_compass2_use = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CMB_compass2_orient = new MissionPlanner.Controls.MavlinkComboBox();
            this.groupBoxCompass1 = new System.Windows.Forms.GroupBox();
            this.CHK_compass1_external = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_compass1_use = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CMB_compass1_orient = new MissionPlanner.Controls.MavlinkComboBox();
            this.groupBoxGeneralSettings = new System.Windows.Forms.GroupBox();
            this.CMB_primary_compass = new MissionPlanner.Controls.MavlinkComboBox();
            this.CHK_compass_learn = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_enablecompass = new MissionPlanner.Controls.MavlinkCheckBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBoxCompass3.SuspendLayout();
            this.groupBoxCompass2.SuspendLayout();
            this.groupBoxCompass1.SuspendLayout();
            this.groupBoxGeneralSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // BUT_level
            // 
            resources.ApplyResources(this.BUT_level, "BUT_level");
            this.BUT_level.BGGradBot = System.Drawing.Color.LightGray;
            this.BUT_level.BGGradTop = System.Drawing.Color.DarkGray;
            this.BUT_level.ColorMouseDown = System.Drawing.Color.Gray;
            this.BUT_level.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BUT_level.Name = "BUT_level";
            this.BUT_level.Outline = System.Drawing.Color.Black;
            this.BUT_level.TextColor = System.Drawing.Color.White;
            this.BUT_level.UseVisualStyleBackColor = true;
            this.BUT_level.Click += new System.EventHandler(this.BUT_level_Click);
            // 
            // lbl_Accel_user
            // 
            resources.ApplyResources(this.lbl_Accel_user, "lbl_Accel_user");
            this.lbl_Accel_user.Name = "lbl_Accel_user";
            // 
            // BUT_calib_accell
            // 
            resources.ApplyResources(this.BUT_calib_accell, "BUT_calib_accell");
            this.BUT_calib_accell.BGGradBot = System.Drawing.Color.LightGray;
            this.BUT_calib_accell.BGGradTop = System.Drawing.Color.DarkGray;
            this.BUT_calib_accell.ColorMouseDown = System.Drawing.Color.Gray;
            this.BUT_calib_accell.Name = "BUT_calib_accell";
            this.BUT_calib_accell.Outline = System.Drawing.Color.Black;
            this.BUT_calib_accell.TextColor = System.Drawing.Color.White;
            this.BUT_calib_accell.UseVisualStyleBackColor = true;
            this.BUT_calib_accell.Click += new System.EventHandler(this.BUT_calib_accell_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::MissionPlanner.Properties.Resources.rotor;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // BUT_MagCalibrationLive
            // 
            resources.ApplyResources(this.BUT_MagCalibrationLive, "BUT_MagCalibrationLive");
            this.BUT_MagCalibrationLive.BGGradBot = System.Drawing.Color.LightGray;
            this.BUT_MagCalibrationLive.BGGradTop = System.Drawing.Color.DarkGray;
            this.BUT_MagCalibrationLive.ColorMouseDown = System.Drawing.Color.Gray;
            this.BUT_MagCalibrationLive.Name = "BUT_MagCalibrationLive";
            this.BUT_MagCalibrationLive.Outline = System.Drawing.Color.Black;
            this.BUT_MagCalibrationLive.TextColor = System.Drawing.Color.White;
            this.BUT_MagCalibrationLive.UseVisualStyleBackColor = true;
            // 
            // LBL_primary_compass
            // 
            resources.ApplyResources(this.LBL_primary_compass, "LBL_primary_compass");
            this.LBL_primary_compass.Name = "LBL_primary_compass";
            // 
            // LBL_compass1_mot
            // 
            resources.ApplyResources(this.LBL_compass1_mot, "LBL_compass1_mot");
            this.LBL_compass1_mot.Name = "LBL_compass1_mot";
            // 
            // LBL_compass1_offset
            // 
            resources.ApplyResources(this.LBL_compass1_offset, "LBL_compass1_offset");
            this.LBL_compass1_offset.Name = "LBL_compass1_offset";
            // 
            // LBL_compass2_mot
            // 
            resources.ApplyResources(this.LBL_compass2_mot, "LBL_compass2_mot");
            this.LBL_compass2_mot.Name = "LBL_compass2_mot";
            // 
            // LBL_compass3_mot
            // 
            resources.ApplyResources(this.LBL_compass3_mot, "LBL_compass3_mot");
            this.LBL_compass3_mot.Name = "LBL_compass3_mot";
            // 
            // LBL_compass3_offset
            // 
            resources.ApplyResources(this.LBL_compass3_offset, "LBL_compass3_offset");
            this.LBL_compass3_offset.Name = "LBL_compass3_offset";
            // 
            // LBL_compass2_offset
            // 
            resources.ApplyResources(this.LBL_compass2_offset, "LBL_compass2_offset");
            this.LBL_compass2_offset.Name = "LBL_compass2_offset";
            // 
            // groupBoxCompass3
            // 
            resources.ApplyResources(this.groupBoxCompass3, "groupBoxCompass3");
            this.groupBoxCompass3.Controls.Add(this.LBL_compass3_mot);
            this.groupBoxCompass3.Controls.Add(this.LBL_compass3_offset);
            this.groupBoxCompass3.Controls.Add(this.CHK_compass3_external);
            this.groupBoxCompass3.Controls.Add(this.CHK_compass3_use);
            this.groupBoxCompass3.Controls.Add(this.CMB_compass3_orient);
            this.groupBoxCompass3.Name = "groupBoxCompass3";
            this.groupBoxCompass3.TabStop = false;
            // 
            // CHK_compass3_external
            // 
            resources.ApplyResources(this.CHK_compass3_external, "CHK_compass3_external");
            this.CHK_compass3_external.Name = "CHK_compass3_external";
            this.CHK_compass3_external.OffValue = 0D;
            this.CHK_compass3_external.OnValue = 1D;
            this.CHK_compass3_external.ParamName = null;
            this.CHK_compass3_external.UseVisualStyleBackColor = true;
            // 
            // CHK_compass3_use
            // 
            resources.ApplyResources(this.CHK_compass3_use, "CHK_compass3_use");
            this.CHK_compass3_use.Name = "CHK_compass3_use";
            this.CHK_compass3_use.OffValue = 0D;
            this.CHK_compass3_use.OnValue = 1D;
            this.CHK_compass3_use.ParamName = null;
            this.CHK_compass3_use.UseVisualStyleBackColor = true;
            // 
            // CMB_compass3_orient
            // 
            resources.ApplyResources(this.CMB_compass3_orient, "CMB_compass3_orient");
            this.CMB_compass3_orient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_compass3_orient.FormattingEnabled = true;
            this.CMB_compass3_orient.Name = "CMB_compass3_orient";
            this.CMB_compass3_orient.ParamName = null;
            this.CMB_compass3_orient.SubControl = null;
            // 
            // CHK_autodec
            // 
            resources.ApplyResources(this.CHK_autodec, "CHK_autodec");
            this.CHK_autodec.Name = "CHK_autodec";
            this.CHK_autodec.UseVisualStyleBackColor = true;
            // 
            // TXT_declination_deg
            // 
            resources.ApplyResources(this.TXT_declination_deg, "TXT_declination_deg");
            this.TXT_declination_deg.Name = "TXT_declination_deg";
            // 
            // linkLabelmagdec
            // 
            resources.ApplyResources(this.linkLabelmagdec, "linkLabelmagdec");
            this.linkLabelmagdec.Name = "linkLabelmagdec";
            this.linkLabelmagdec.TabStop = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // TXT_declination_min
            // 
            resources.ApplyResources(this.TXT_declination_min, "TXT_declination_min");
            this.TXT_declination_min.Name = "TXT_declination_min";
            // 
            // groupBoxCompass2
            // 
            resources.ApplyResources(this.groupBoxCompass2, "groupBoxCompass2");
            this.groupBoxCompass2.Controls.Add(this.LBL_compass2_mot);
            this.groupBoxCompass2.Controls.Add(this.LBL_compass2_offset);
            this.groupBoxCompass2.Controls.Add(this.CHK_compass2_external);
            this.groupBoxCompass2.Controls.Add(this.CHK_compass2_use);
            this.groupBoxCompass2.Controls.Add(this.CMB_compass2_orient);
            this.groupBoxCompass2.Name = "groupBoxCompass2";
            this.groupBoxCompass2.TabStop = false;
            // 
            // CHK_compass2_external
            // 
            resources.ApplyResources(this.CHK_compass2_external, "CHK_compass2_external");
            this.CHK_compass2_external.Name = "CHK_compass2_external";
            this.CHK_compass2_external.OffValue = 0D;
            this.CHK_compass2_external.OnValue = 1D;
            this.CHK_compass2_external.ParamName = null;
            this.CHK_compass2_external.UseVisualStyleBackColor = true;
            // 
            // CHK_compass2_use
            // 
            resources.ApplyResources(this.CHK_compass2_use, "CHK_compass2_use");
            this.CHK_compass2_use.Name = "CHK_compass2_use";
            this.CHK_compass2_use.OffValue = 0D;
            this.CHK_compass2_use.OnValue = 1D;
            this.CHK_compass2_use.ParamName = null;
            this.CHK_compass2_use.UseVisualStyleBackColor = true;
            // 
            // CMB_compass2_orient
            // 
            resources.ApplyResources(this.CMB_compass2_orient, "CMB_compass2_orient");
            this.CMB_compass2_orient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_compass2_orient.FormattingEnabled = true;
            this.CMB_compass2_orient.Name = "CMB_compass2_orient";
            this.CMB_compass2_orient.ParamName = null;
            this.CMB_compass2_orient.SubControl = null;
            // 
            // groupBoxCompass1
            // 
            resources.ApplyResources(this.groupBoxCompass1, "groupBoxCompass1");
            this.groupBoxCompass1.Controls.Add(this.LBL_compass1_mot);
            this.groupBoxCompass1.Controls.Add(this.LBL_compass1_offset);
            this.groupBoxCompass1.Controls.Add(this.CHK_compass1_external);
            this.groupBoxCompass1.Controls.Add(this.CHK_compass1_use);
            this.groupBoxCompass1.Controls.Add(this.CMB_compass1_orient);
            this.groupBoxCompass1.Name = "groupBoxCompass1";
            this.groupBoxCompass1.TabStop = false;
            // 
            // CHK_compass1_external
            // 
            resources.ApplyResources(this.CHK_compass1_external, "CHK_compass1_external");
            this.CHK_compass1_external.Name = "CHK_compass1_external";
            this.CHK_compass1_external.OffValue = 0D;
            this.CHK_compass1_external.OnValue = 1D;
            this.CHK_compass1_external.ParamName = null;
            this.CHK_compass1_external.UseVisualStyleBackColor = true;
            // 
            // CHK_compass1_use
            // 
            resources.ApplyResources(this.CHK_compass1_use, "CHK_compass1_use");
            this.CHK_compass1_use.Name = "CHK_compass1_use";
            this.CHK_compass1_use.OffValue = 0D;
            this.CHK_compass1_use.OnValue = 1D;
            this.CHK_compass1_use.ParamName = null;
            this.CHK_compass1_use.UseVisualStyleBackColor = true;
            // 
            // CMB_compass1_orient
            // 
            resources.ApplyResources(this.CMB_compass1_orient, "CMB_compass1_orient");
            this.CMB_compass1_orient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_compass1_orient.FormattingEnabled = true;
            this.CMB_compass1_orient.Name = "CMB_compass1_orient";
            this.CMB_compass1_orient.ParamName = null;
            this.CMB_compass1_orient.SubControl = null;
            // 
            // groupBoxGeneralSettings
            // 
            resources.ApplyResources(this.groupBoxGeneralSettings, "groupBoxGeneralSettings");
            this.groupBoxGeneralSettings.Controls.Add(this.CMB_primary_compass);
            this.groupBoxGeneralSettings.Controls.Add(this.LBL_primary_compass);
            this.groupBoxGeneralSettings.Controls.Add(this.CHK_compass_learn);
            this.groupBoxGeneralSettings.Controls.Add(this.CHK_enablecompass);
            this.groupBoxGeneralSettings.Controls.Add(this.CHK_autodec);
            this.groupBoxGeneralSettings.Controls.Add(this.label6);
            this.groupBoxGeneralSettings.Controls.Add(this.label7);
            this.groupBoxGeneralSettings.Controls.Add(this.TXT_declination_min);
            this.groupBoxGeneralSettings.Controls.Add(this.TXT_declination_deg);
            this.groupBoxGeneralSettings.Controls.Add(this.linkLabelmagdec);
            this.groupBoxGeneralSettings.Name = "groupBoxGeneralSettings";
            this.groupBoxGeneralSettings.TabStop = false;
            // 
            // CMB_primary_compass
            // 
            resources.ApplyResources(this.CMB_primary_compass, "CMB_primary_compass");
            this.CMB_primary_compass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_primary_compass.FormattingEnabled = true;
            this.CMB_primary_compass.Name = "CMB_primary_compass";
            this.CMB_primary_compass.ParamName = null;
            this.CMB_primary_compass.SubControl = null;
            // 
            // CHK_compass_learn
            // 
            resources.ApplyResources(this.CHK_compass_learn, "CHK_compass_learn");
            this.CHK_compass_learn.Name = "CHK_compass_learn";
            this.CHK_compass_learn.OffValue = 0D;
            this.CHK_compass_learn.OnValue = 1D;
            this.CHK_compass_learn.ParamName = null;
            this.CHK_compass_learn.UseVisualStyleBackColor = true;
            // 
            // CHK_enablecompass
            // 
            resources.ApplyResources(this.CHK_enablecompass, "CHK_enablecompass");
            this.CHK_enablecompass.Name = "CHK_enablecompass";
            this.CHK_enablecompass.OffValue = 0D;
            this.CHK_enablecompass.OnValue = 1D;
            this.CHK_enablecompass.ParamName = null;
            this.CHK_enablecompass.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Image = global::MissionPlanner.Properties.Resources._00010;
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            resources.ApplyResources(this.pictureBox4, "pictureBox4");
            this.pictureBox4.Image = global::MissionPlanner.Properties.Resources.compass61;
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // Calibration
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BUT_MagCalibrationLive);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.groupBoxCompass3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxCompass2);
            this.Controls.Add(this.groupBoxCompass1);
            this.Controls.Add(this.groupBoxGeneralSettings);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.BUT_level);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_Accel_user);
            this.Controls.Add(this.BUT_calib_accell);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Name = "Calibration";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBoxCompass3.ResumeLayout(false);
            this.groupBoxCompass3.PerformLayout();
            this.groupBoxCompass2.ResumeLayout(false);
            this.groupBoxCompass2.PerformLayout();
            this.groupBoxCompass1.ResumeLayout(false);
            this.groupBoxCompass1.PerformLayout();
            this.groupBoxGeneralSettings.ResumeLayout(false);
            this.groupBoxGeneralSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Controls.MyButton BUT_level;
        private System.Windows.Forms.Label lbl_Accel_user;
        private Controls.MyButton BUT_calib_accell;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer timer1;
        private Controls.MyButton BUT_MagCalibrationLive;
        private Controls.MavlinkComboBox CMB_primary_compass;
        private System.Windows.Forms.Label LBL_primary_compass;
        private Controls.MavlinkCheckBox CHK_compass_learn;
        private Controls.MavlinkCheckBox CHK_enablecompass;
        private System.Windows.Forms.Label LBL_compass1_mot;
        private System.Windows.Forms.Label LBL_compass1_offset;
        private Controls.MavlinkCheckBox CHK_compass1_external;
        private Controls.MavlinkCheckBox CHK_compass1_use;
        private Controls.MavlinkComboBox CMB_compass1_orient;
        private System.Windows.Forms.Label LBL_compass2_mot;
        private Controls.MavlinkCheckBox CHK_compass2_external;
        private Controls.MavlinkCheckBox CHK_compass2_use;
        private Controls.MavlinkComboBox CMB_compass2_orient;
        private System.Windows.Forms.Label LBL_compass3_mot;
        private System.Windows.Forms.Label LBL_compass3_offset;
        private Controls.MavlinkCheckBox CHK_compass3_external;
        private Controls.MavlinkCheckBox CHK_compass3_use;
        private Controls.MavlinkComboBox CMB_compass3_orient;
        private System.Windows.Forms.Label LBL_compass2_offset;
        private System.Windows.Forms.GroupBox groupBoxCompass3;
        private System.Windows.Forms.CheckBox CHK_autodec;
        private System.Windows.Forms.TextBox TXT_declination_deg;
        private System.Windows.Forms.LinkLabel linkLabelmagdec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TXT_declination_min;
        private System.Windows.Forms.GroupBox groupBoxCompass2;
        private System.Windows.Forms.GroupBox groupBoxCompass1;
        private System.Windows.Forms.GroupBox groupBoxGeneralSettings;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
