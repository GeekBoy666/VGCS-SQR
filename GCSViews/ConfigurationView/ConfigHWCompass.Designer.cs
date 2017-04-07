﻿namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ConfigHWCompass
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigHWCompass));
            this.linkLabelmagdec = new System.Windows.Forms.LinkLabel();
            this.TXT_declination_deg = new System.Windows.Forms.TextBox();
            this.CHK_autodec = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TXT_declination_min = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxGeneralSettings = new System.Windows.Forms.GroupBox();
            this.CMB_primary_compass = new MissionPlanner.Controls.MavlinkComboBox();
            this.LBL_primary_compass = new System.Windows.Forms.Label();
            this.CHK_compass_learn = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_enablecompass = new MissionPlanner.Controls.MavlinkCheckBox();
            this.groupBoxCompass1 = new System.Windows.Forms.GroupBox();
            this.LBL_compass1_mot = new System.Windows.Forms.Label();
            this.LBL_compass1_offset = new System.Windows.Forms.Label();
            this.CHK_compass1_external = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_compass1_use = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CMB_compass1_orient = new MissionPlanner.Controls.MavlinkComboBox();
            this.groupBoxCompass2 = new System.Windows.Forms.GroupBox();
            this.LBL_compass2_mot = new System.Windows.Forms.Label();
            this.LBL_compass2_offset = new System.Windows.Forms.Label();
            this.CHK_compass2_external = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_compass2_use = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CMB_compass2_orient = new MissionPlanner.Controls.MavlinkComboBox();
            this.groupBoxCompass3 = new System.Windows.Forms.GroupBox();
            this.LBL_compass3_mot = new System.Windows.Forms.Label();
            this.LBL_compass3_offset = new System.Windows.Forms.Label();
            this.CHK_compass3_external = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CHK_compass3_use = new MissionPlanner.Controls.MavlinkCheckBox();
            this.CMB_compass3_orient = new MissionPlanner.Controls.MavlinkComboBox();
            this.BUT_MagCalibrationLive = new MissionPlanner.Controls.MyButton();
            this.groupBoxGeneralSettings.SuspendLayout();
            this.groupBoxCompass1.SuspendLayout();
            this.groupBoxCompass2.SuspendLayout();
            this.groupBoxCompass3.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabelmagdec
            // 
            resources.ApplyResources(this.linkLabelmagdec, "linkLabelmagdec");
            this.linkLabelmagdec.Name = "linkLabelmagdec";
            this.linkLabelmagdec.TabStop = true;
            this.linkLabelmagdec.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // TXT_declination_deg
            // 
            resources.ApplyResources(this.TXT_declination_deg, "TXT_declination_deg");
            this.TXT_declination_deg.Name = "TXT_declination_deg";
            this.TXT_declination_deg.Validated += new System.EventHandler(this.TXT_declination_Validated);
            // 
            // CHK_autodec
            // 
            resources.ApplyResources(this.CHK_autodec, "CHK_autodec");
            this.CHK_autodec.Name = "CHK_autodec";
            this.CHK_autodec.UseVisualStyleBackColor = true;
            this.CHK_autodec.CheckedChanged += new System.EventHandler(this.CHK_autodec_CheckedChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // TXT_declination_min
            // 
            resources.ApplyResources(this.TXT_declination_min, "TXT_declination_min");
            this.TXT_declination_min.Name = "TXT_declination_min";
            this.TXT_declination_min.Validated += new System.EventHandler(this.TXT_declination_Validated);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBoxGeneralSettings
            // 
            this.groupBoxGeneralSettings.Controls.Add(this.CMB_primary_compass);
            this.groupBoxGeneralSettings.Controls.Add(this.LBL_primary_compass);
            this.groupBoxGeneralSettings.Controls.Add(this.CHK_compass_learn);
            this.groupBoxGeneralSettings.Controls.Add(this.CHK_enablecompass);
            this.groupBoxGeneralSettings.Controls.Add(this.CHK_autodec);
            this.groupBoxGeneralSettings.Controls.Add(this.label2);
            this.groupBoxGeneralSettings.Controls.Add(this.label3);
            this.groupBoxGeneralSettings.Controls.Add(this.TXT_declination_min);
            this.groupBoxGeneralSettings.Controls.Add(this.TXT_declination_deg);
            this.groupBoxGeneralSettings.Controls.Add(this.linkLabelmagdec);
            resources.ApplyResources(this.groupBoxGeneralSettings, "groupBoxGeneralSettings");
            this.groupBoxGeneralSettings.Name = "groupBoxGeneralSettings";
            this.groupBoxGeneralSettings.TabStop = false;
            // 
            // CMB_primary_compass
            // 
            this.CMB_primary_compass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.CMB_primary_compass, "CMB_primary_compass");
            this.CMB_primary_compass.FormattingEnabled = true;
            this.CMB_primary_compass.Name = "CMB_primary_compass";
            this.CMB_primary_compass.ParamName = null;
            this.CMB_primary_compass.SubControl = null;
            // 
            // LBL_primary_compass
            // 
            resources.ApplyResources(this.LBL_primary_compass, "LBL_primary_compass");
            this.LBL_primary_compass.Name = "LBL_primary_compass";
            // 
            // CHK_compass_learn
            // 
            resources.ApplyResources(this.CHK_compass_learn, "CHK_compass_learn");
            this.CHK_compass_learn.Name = "CHK_compass_learn";
            this.CHK_compass_learn.OffValue = 0D;
            this.CHK_compass_learn.OnValue = 1D;
            this.CHK_compass_learn.ParamName = null;
            this.CHK_compass_learn.UseVisualStyleBackColor = true;
            this.CHK_compass_learn.CheckedChanged += new System.EventHandler(this.CHK_compasslearn_CheckedChanged);
            // 
            // CHK_enablecompass
            // 
            resources.ApplyResources(this.CHK_enablecompass, "CHK_enablecompass");
            this.CHK_enablecompass.Name = "CHK_enablecompass";
            this.CHK_enablecompass.OffValue = 0D;
            this.CHK_enablecompass.OnValue = 1D;
            this.CHK_enablecompass.ParamName = null;
            this.CHK_enablecompass.UseVisualStyleBackColor = true;
            this.CHK_enablecompass.CheckedChanged += new System.EventHandler(this.CHK_enablecompass_CheckedChanged);
            // 
            // groupBoxCompass1
            // 
            this.groupBoxCompass1.Controls.Add(this.LBL_compass1_mot);
            this.groupBoxCompass1.Controls.Add(this.LBL_compass1_offset);
            this.groupBoxCompass1.Controls.Add(this.CHK_compass1_external);
            this.groupBoxCompass1.Controls.Add(this.CHK_compass1_use);
            this.groupBoxCompass1.Controls.Add(this.CMB_compass1_orient);
            resources.ApplyResources(this.groupBoxCompass1, "groupBoxCompass1");
            this.groupBoxCompass1.Name = "groupBoxCompass1";
            this.groupBoxCompass1.TabStop = false;
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
            // CHK_compass1_external
            // 
            resources.ApplyResources(this.CHK_compass1_external, "CHK_compass1_external");
            this.CHK_compass1_external.Name = "CHK_compass1_external";
            this.CHK_compass1_external.OffValue = 0D;
            this.CHK_compass1_external.OnValue = 1D;
            this.CHK_compass1_external.ParamName = null;
            this.CHK_compass1_external.UseVisualStyleBackColor = true;
            this.CHK_compass1_external.CheckedChanged += new System.EventHandler(this.CHK_compass);
            // 
            // CHK_compass1_use
            // 
            resources.ApplyResources(this.CHK_compass1_use, "CHK_compass1_use");
            this.CHK_compass1_use.Name = "CHK_compass1_use";
            this.CHK_compass1_use.OffValue = 0D;
            this.CHK_compass1_use.OnValue = 1D;
            this.CHK_compass1_use.ParamName = null;
            this.CHK_compass1_use.UseVisualStyleBackColor = true;
            this.CHK_compass1_use.CheckedChanged += new System.EventHandler(this.CHK_compass);
            // 
            // CMB_compass1_orient
            // 
            this.CMB_compass1_orient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.CMB_compass1_orient, "CMB_compass1_orient");
            this.CMB_compass1_orient.FormattingEnabled = true;
            this.CMB_compass1_orient.Name = "CMB_compass1_orient";
            this.CMB_compass1_orient.ParamName = null;
            this.CMB_compass1_orient.SubControl = null;
            // 
            // groupBoxCompass2
            // 
            this.groupBoxCompass2.Controls.Add(this.LBL_compass2_mot);
            this.groupBoxCompass2.Controls.Add(this.LBL_compass2_offset);
            this.groupBoxCompass2.Controls.Add(this.CHK_compass2_external);
            this.groupBoxCompass2.Controls.Add(this.CHK_compass2_use);
            this.groupBoxCompass2.Controls.Add(this.CMB_compass2_orient);
            resources.ApplyResources(this.groupBoxCompass2, "groupBoxCompass2");
            this.groupBoxCompass2.Name = "groupBoxCompass2";
            this.groupBoxCompass2.TabStop = false;
            // 
            // LBL_compass2_mot
            // 
            resources.ApplyResources(this.LBL_compass2_mot, "LBL_compass2_mot");
            this.LBL_compass2_mot.Name = "LBL_compass2_mot";
            // 
            // LBL_compass2_offset
            // 
            resources.ApplyResources(this.LBL_compass2_offset, "LBL_compass2_offset");
            this.LBL_compass2_offset.Name = "LBL_compass2_offset";
            // 
            // CHK_compass2_external
            // 
            resources.ApplyResources(this.CHK_compass2_external, "CHK_compass2_external");
            this.CHK_compass2_external.Name = "CHK_compass2_external";
            this.CHK_compass2_external.OffValue = 0D;
            this.CHK_compass2_external.OnValue = 1D;
            this.CHK_compass2_external.ParamName = null;
            this.CHK_compass2_external.UseVisualStyleBackColor = true;
            this.CHK_compass2_external.CheckedChanged += new System.EventHandler(this.CHK_compass);
            // 
            // CHK_compass2_use
            // 
            resources.ApplyResources(this.CHK_compass2_use, "CHK_compass2_use");
            this.CHK_compass2_use.Name = "CHK_compass2_use";
            this.CHK_compass2_use.OffValue = 0D;
            this.CHK_compass2_use.OnValue = 1D;
            this.CHK_compass2_use.ParamName = null;
            this.CHK_compass2_use.UseVisualStyleBackColor = true;
            this.CHK_compass2_use.CheckedChanged += new System.EventHandler(this.CHK_compass);
            // 
            // CMB_compass2_orient
            // 
            this.CMB_compass2_orient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.CMB_compass2_orient, "CMB_compass2_orient");
            this.CMB_compass2_orient.FormattingEnabled = true;
            this.CMB_compass2_orient.Name = "CMB_compass2_orient";
            this.CMB_compass2_orient.ParamName = null;
            this.CMB_compass2_orient.SubControl = null;
            // 
            // groupBoxCompass3
            // 
            this.groupBoxCompass3.Controls.Add(this.LBL_compass3_mot);
            this.groupBoxCompass3.Controls.Add(this.LBL_compass3_offset);
            this.groupBoxCompass3.Controls.Add(this.CHK_compass3_external);
            this.groupBoxCompass3.Controls.Add(this.CHK_compass3_use);
            this.groupBoxCompass3.Controls.Add(this.CMB_compass3_orient);
            resources.ApplyResources(this.groupBoxCompass3, "groupBoxCompass3");
            this.groupBoxCompass3.Name = "groupBoxCompass3";
            this.groupBoxCompass3.TabStop = false;
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
            // CHK_compass3_external
            // 
            resources.ApplyResources(this.CHK_compass3_external, "CHK_compass3_external");
            this.CHK_compass3_external.Name = "CHK_compass3_external";
            this.CHK_compass3_external.OffValue = 0D;
            this.CHK_compass3_external.OnValue = 1D;
            this.CHK_compass3_external.ParamName = null;
            this.CHK_compass3_external.UseVisualStyleBackColor = true;
            this.CHK_compass3_external.CheckedChanged += new System.EventHandler(this.CHK_compass);
            // 
            // CHK_compass3_use
            // 
            resources.ApplyResources(this.CHK_compass3_use, "CHK_compass3_use");
            this.CHK_compass3_use.Name = "CHK_compass3_use";
            this.CHK_compass3_use.OffValue = 0D;
            this.CHK_compass3_use.OnValue = 1D;
            this.CHK_compass3_use.ParamName = null;
            this.CHK_compass3_use.UseVisualStyleBackColor = true;
            this.CHK_compass3_use.CheckedChanged += new System.EventHandler(this.CHK_compass);
            // 
            // CMB_compass3_orient
            // 
            this.CMB_compass3_orient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.CMB_compass3_orient, "CMB_compass3_orient");
            this.CMB_compass3_orient.FormattingEnabled = true;
            this.CMB_compass3_orient.Name = "CMB_compass3_orient";
            this.CMB_compass3_orient.ParamName = null;
            this.CMB_compass3_orient.SubControl = null;
            // 
            // BUT_MagCalibrationLive
            // 
            this.BUT_MagCalibrationLive.BGGradBot = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(168)))), ((int)(((byte)(173)))));
            this.BUT_MagCalibrationLive.BGGradTop = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(168)))), ((int)(((byte)(173)))));
            this.BUT_MagCalibrationLive.ColorMouseDown = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.BUT_MagCalibrationLive, "BUT_MagCalibrationLive");
            this.BUT_MagCalibrationLive.Name = "BUT_MagCalibrationLive";
            this.BUT_MagCalibrationLive.UseVisualStyleBackColor = true;
            this.BUT_MagCalibrationLive.Click += new System.EventHandler(this.BUT_MagCalibration_Click);
            // 
            // ConfigHWCompass
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.BUT_MagCalibrationLive);
            this.Controls.Add(this.groupBoxCompass3);
            this.Controls.Add(this.groupBoxCompass2);
            this.Controls.Add(this.groupBoxCompass1);
            this.Controls.Add(this.groupBoxGeneralSettings);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Name = "ConfigHWCompass";
            resources.ApplyResources(this, "$this");
            this.groupBoxGeneralSettings.ResumeLayout(false);
            this.groupBoxGeneralSettings.PerformLayout();
            this.groupBoxCompass1.ResumeLayout(false);
            this.groupBoxCompass1.PerformLayout();
            this.groupBoxCompass2.ResumeLayout(false);
            this.groupBoxCompass2.PerformLayout();
            this.groupBoxCompass3.ResumeLayout(false);
            this.groupBoxCompass3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.LinkLabel linkLabelmagdec;
        private System.Windows.Forms.TextBox TXT_declination_deg;
        private Controls.MavlinkCheckBox CHK_enablecompass;
        private System.Windows.Forms.CheckBox CHK_autodec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private Controls.MavlinkComboBox CMB_compass1_orient;
        private System.Windows.Forms.TextBox TXT_declination_min;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBoxGeneralSettings;
        private System.Windows.Forms.GroupBox groupBoxCompass1;
        private Controls.MavlinkCheckBox CHK_compass_learn;
        private Controls.MavlinkCheckBox CHK_compass1_external;
        private Controls.MavlinkCheckBox CHK_compass1_use;
        private Controls.MavlinkComboBox CMB_primary_compass;
        private System.Windows.Forms.Label LBL_primary_compass;
        private System.Windows.Forms.GroupBox groupBoxCompass2;
        private Controls.MavlinkCheckBox CHK_compass2_external;
        private Controls.MavlinkCheckBox CHK_compass2_use;
        private Controls.MavlinkComboBox CMB_compass2_orient;
        private System.Windows.Forms.GroupBox groupBoxCompass3;
        private Controls.MavlinkCheckBox CHK_compass3_external;
        private Controls.MavlinkCheckBox CHK_compass3_use;
        private Controls.MavlinkComboBox CMB_compass3_orient;
        private System.Windows.Forms.Label LBL_compass1_mot;
        private System.Windows.Forms.Label LBL_compass1_offset;
        private System.Windows.Forms.Label LBL_compass2_mot;
        private System.Windows.Forms.Label LBL_compass2_offset;
        private System.Windows.Forms.Label LBL_compass3_mot;
        private System.Windows.Forms.Label LBL_compass3_offset;
        private Controls.MyButton BUT_MagCalibrationLive;
    }
}
