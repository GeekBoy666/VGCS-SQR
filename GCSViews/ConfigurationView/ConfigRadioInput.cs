using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MissionPlanner.Controls;
using Timer = System.Windows.Forms.Timer;

using System.Diagnostics;

using MissionPlanner.Utilities;
namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class ConfigRadioInput : UserControl, IActivate, IDeactivate
    {
        private readonly float[] rcmax = new float[16];
        private readonly float[] rcmin = new float[16];
        private readonly float[] rctrim = new float[16];
        private readonly Timer timer = new Timer();
        private int chpitch = -1;
        private int chroll = -1;
        private int chthro = -1;
        private int chyaw = -1;
        private bool run;
        private bool startup;
        [Flags]
        public enum SimpleMode
        {
            None = 0,
            Simple1 = 1,
            Simple2 = 2,
            Simple3 = 4,
            Simple4 = 8,
            Simple5 = 16,
            Simple6 = 32
        }
        public ConfigRadioInput()
        {
            InitializeComponent();

            // setup rc calib extents
            for (var a = 0; a < rcmin.Length; a++)
            {
                rcmin[a] = 3000;
                rcmax[a] = 0;
                rctrim[a] = 1500;
            }


            // setup rc update
            timer.Tick += timer_Tick;
        }

        public void Activate()
        {
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Start();

            if (!MainV2.comPort.MAV.param.ContainsKey("RCMAP_ROLL"))
            {
                chroll = 1;
                chpitch = 2;
                chthro = 3;
                chyaw = 4;
            }
            else
            {
                //setup bindings
                chroll = (int) (float) MainV2.comPort.MAV.param["RCMAP_ROLL"];
                chpitch = (int) (float) MainV2.comPort.MAV.param["RCMAP_PITCH"];
                chthro = (int) (float) MainV2.comPort.MAV.param["RCMAP_THROTTLE"];
                chyaw = (int) (float) MainV2.comPort.MAV.param["RCMAP_YAW"];
            }

            BARroll.DataBindings.Clear();
            BARpitch.DataBindings.Clear();
            BARthrottle.DataBindings.Clear();
            BARyaw.DataBindings.Clear();
            BAR5.DataBindings.Clear();
            BAR6.DataBindings.Clear();
            BAR7.DataBindings.Clear();
            BAR8.DataBindings.Clear();
            BAR9.DataBindings.Clear();
            BAR10.DataBindings.Clear();
            BAR11.DataBindings.Clear();
            BAR12.DataBindings.Clear();
            BAR13.DataBindings.Clear();
            BAR14.DataBindings.Clear();

            BARroll.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chroll + "in", true));
            BARpitch.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chpitch + "in", true));
            BARthrottle.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chthro + "in", true));
            BARyaw.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch" + chyaw + "in", true));

            BAR5.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch5in", true));
            BAR6.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch6in", true));
            BAR7.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch7in", true));
            BAR8.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch8in", true));

            BAR9.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch9in", true));
            BAR10.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch10in", true));
            BAR11.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch11in", true));
            BAR12.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch12in", true));
            BAR13.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch13in", true));
            BAR14.DataBindings.Add(new Binding("Value", currentStateBindingSource, "ch14in", true));

            try
            {
                // force this screen to work
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RC_CHANNELS, 2);
            }
            catch
            {
            }

            startup = true;

            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduPlane ||
                MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.Ateryx)
            {
                CHK_mixmode.setup(1, 0, "ELEVON_MIXING", MainV2.comPort.MAV.param);
                CHK_elevonrev.setup(1, 0, "ELEVON_REVERSE", MainV2.comPort.MAV.param);
                CHK_elevonch1rev.setup(1, 0, "ELEVON_CH1_REV", MainV2.comPort.MAV.param);
                CHK_elevonch2rev.setup(1, 0, "ELEVON_CH2_REV", MainV2.comPort.MAV.param);
            }
            else
            {
                groupBoxElevons.Visible = false;
            }

            // this controls the direction of the output, not the input.
            CHK_revch1.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC1_REV", "SERVO1_REVERSED"},
                MainV2.comPort.MAV.param);
            CHK_revch2.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC2_REV", "SERVO2_REVERSED"},
                MainV2.comPort.MAV.param);
            CHK_revch3.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC3_REV", "SERVO3_REVERSED"},
                MainV2.comPort.MAV.param);
            CHK_revch4.setup(new double[] {-1, 1}, new double[] {1, 0}, new string[] {"RC4_REV", "SERVO4_REVERSED"},
                MainV2.comPort.MAV.param);

            // run after to ensure they are disabled on copter
            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2)
            {
                CHK_revch1.Visible = false;
                CHK_revch2.Visible = false;
                CHK_revch3.Visible = false;
                CHK_revch4.Visible = false;
            }

            startup = false;

            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduPlane ||
                MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.Ateryx) // APM
            {
                CB_simple1.Visible = false;
                CB_simple2.Visible = false;
                CB_simple3.Visible = false;
                CB_simple4.Visible = false;
                CB_simple5.Visible = false;
                CB_simple6.Visible = false;

                chk_ss1.Visible = false;
                chk_ss2.Visible = false;
                chk_ss3.Visible = false;
                chk_ss4.Visible = false;
                chk_ss5.Visible = false;
                chk_ss6.Visible = false;

                //linkLabel1_ss.Visible = false;

                try
                {
                    updateDropDown(CMB_fmode1, "FLTMODE1");
                    updateDropDown(CMB_fmode2, "FLTMODE2");
                    updateDropDown(CMB_fmode3, "FLTMODE3");
                    updateDropDown(CMB_fmode4, "FLTMODE4");
                    updateDropDown(CMB_fmode5, "FLTMODE5");
                    updateDropDown(CMB_fmode6, "FLTMODE6");

                    CMB_fmode1.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE1"].ToString());
                    CMB_fmode2.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE2"].ToString());
                    CMB_fmode3.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE3"].ToString());
                    CMB_fmode4.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE4"].ToString());
                    CMB_fmode5.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE5"].ToString());
                    CMB_fmode6.Text = "Manual";
                    CMB_fmode6.Enabled = false;
                }
                catch
                {
                }
            }
            else if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduRover) // APM
            {
                CB_simple1.Visible = false;
                CB_simple2.Visible = false;
                CB_simple3.Visible = false;
                CB_simple4.Visible = false;
                CB_simple5.Visible = false;
                CB_simple6.Visible = false;

                chk_ss1.Visible = false;
                chk_ss2.Visible = false;
                chk_ss3.Visible = false;
                chk_ss4.Visible = false;
                chk_ss5.Visible = false;
                chk_ss6.Visible = false;

                //linkLabel1_ss.Visible = false;

                try
                {
                    updateDropDown(CMB_fmode1, "MODE1");
                    updateDropDown(CMB_fmode2, "MODE2");
                    updateDropDown(CMB_fmode3, "MODE3");
                    updateDropDown(CMB_fmode4, "MODE4");
                    updateDropDown(CMB_fmode5, "MODE5");
                    updateDropDown(CMB_fmode6, "MODE6");

                    CMB_fmode1.SelectedValue = int.Parse(MainV2.comPort.MAV.param["MODE1"].ToString());
                    CMB_fmode2.SelectedValue = int.Parse(MainV2.comPort.MAV.param["MODE2"].ToString());
                    CMB_fmode3.SelectedValue = int.Parse(MainV2.comPort.MAV.param["MODE3"].ToString());
                    CMB_fmode4.SelectedValue = int.Parse(MainV2.comPort.MAV.param["MODE4"].ToString());
                    CMB_fmode5.SelectedValue = int.Parse(MainV2.comPort.MAV.param["MODE5"].ToString());
                    CMB_fmode6.Text = "Manual";
                    CMB_fmode6.Enabled = false;
                }
                catch
                {
                }
            }
            else if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2) // ac2
            {
                try
                {
                    updateDropDown(CMB_fmode1, "FLTMODE1");
                    updateDropDown(CMB_fmode2, "FLTMODE2");
                    updateDropDown(CMB_fmode3, "FLTMODE3");
                    updateDropDown(CMB_fmode4, "FLTMODE4");
                    updateDropDown(CMB_fmode5, "FLTMODE5");
                    updateDropDown(CMB_fmode6, "FLTMODE6");

                    CMB_fmode1.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE1"].ToString());
                    CMB_fmode2.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE2"].ToString());
                    CMB_fmode3.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE3"].ToString());
                    CMB_fmode4.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE4"].ToString());
                    CMB_fmode5.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE5"].ToString());
                    CMB_fmode6.SelectedValue = int.Parse(MainV2.comPort.MAV.param["FLTMODE6"].ToString());
                    CMB_fmode6.Enabled = true;

                    if (MainV2.comPort.MAV.param.ContainsKey("SIMPLE"))
                    {
                        var simple = int.Parse(MainV2.comPort.MAV.param["SIMPLE"].ToString());

                        CB_simple1.Checked = ((simple >> 0 & 1) == 1);
                        CB_simple2.Checked = ((simple >> 1 & 1) == 1);
                        CB_simple3.Checked = ((simple >> 2 & 1) == 1);
                        CB_simple4.Checked = ((simple >> 3 & 1) == 1);
                        CB_simple5.Checked = ((simple >> 4 & 1) == 1);
                        CB_simple6.Checked = ((simple >> 5 & 1) == 1);
                    }

                    if (MainV2.comPort.MAV.param.ContainsKey("SUPER_SIMPLE"))
                    {
                        var simple = int.Parse(MainV2.comPort.MAV.param["SUPER_SIMPLE"].ToString());

                        chk_ss1.Checked = ((simple >> 0 & 1) == 1);
                        chk_ss2.Checked = ((simple >> 1 & 1) == 1);
                        chk_ss3.Checked = ((simple >> 2 & 1) == 1);
                        chk_ss4.Checked = ((simple >> 3 & 1) == 1);
                        chk_ss5.Checked = ((simple >> 4 & 1) == 1);
                        chk_ss6.Checked = ((simple >> 5 & 1) == 1);
                    }
                }
                catch
                {
                }
            }
            else if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.PX4) // APM
            {
                CB_simple1.Visible = false;
                CB_simple2.Visible = false;
                CB_simple3.Visible = false;
                CB_simple4.Visible = false;
                CB_simple5.Visible = false;
                CB_simple6.Visible = false;

                chk_ss1.Visible = false;
                chk_ss2.Visible = false;
                chk_ss3.Visible = false;
                chk_ss4.Visible = false;
                chk_ss5.Visible = false;
                chk_ss6.Visible = false;

                //linkLabel1_ss.Visible = false;

                try
                {
                    updateDropDown(CMB_fmode1, "COM_FLTMODE1");
                    CMB_fmode1.DataSource = ParameterMetaDataRepository.GetParameterOptionsInt("COM_FLTMODE1", "PX4");
                    updateDropDown(CMB_fmode2, "COM_FLTMODE2");
                    CMB_fmode2.DataSource = ParameterMetaDataRepository.GetParameterOptionsInt("COM_FLTMODE2", "PX4");
                    updateDropDown(CMB_fmode3, "COM_FLTMODE3");
                    CMB_fmode3.DataSource = ParameterMetaDataRepository.GetParameterOptionsInt("COM_FLTMODE3", "PX4");
                    updateDropDown(CMB_fmode4, "COM_FLTMODE4");
                    CMB_fmode4.DataSource = ParameterMetaDataRepository.GetParameterOptionsInt("COM_FLTMODE4", "PX4");
                    updateDropDown(CMB_fmode5, "COM_FLTMODE5");
                    CMB_fmode5.DataSource = ParameterMetaDataRepository.GetParameterOptionsInt("COM_FLTMODE5", "PX4");
                    updateDropDown(CMB_fmode6, "COM_FLTMODE6");
                    CMB_fmode6.DataSource = ParameterMetaDataRepository.GetParameterOptionsInt("COM_FLTMODE6", "PX4");

                    CMB_fmode1.SelectedValue = int.Parse(MainV2.comPort.MAV.param["COM_FLTMODE1"].ToString());
                    CMB_fmode2.SelectedValue = int.Parse(MainV2.comPort.MAV.param["COM_FLTMODE2"].ToString());
                    CMB_fmode3.SelectedValue = int.Parse(MainV2.comPort.MAV.param["COM_FLTMODE3"].ToString());
                    CMB_fmode4.SelectedValue = int.Parse(MainV2.comPort.MAV.param["COM_FLTMODE4"].ToString());
                    CMB_fmode5.SelectedValue = int.Parse(MainV2.comPort.MAV.param["COM_FLTMODE5"].ToString());
                    CMB_fmode6.SelectedValue = int.Parse(MainV2.comPort.MAV.param["COM_FLTMODE6"].ToString());
                }
                catch
                {
                }
            }
                        
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Start();
        }

        public void Deactivate()
        {
            timer.Stop();
        }


        private void BUT_Calibrateradio_Click(object sender, EventArgs e)
        {
            if (run)
            {
                BUT_Calibrateradio.Text = Strings.Completed;
                run = false;
                return;
            }

            CustomMessageBox.Show(
                "Ensure your transmitter is on and receiver is powered and connected\nEnsure your motor does not have power/no props!!!");

            var oldrc = MainV2.comPort.MAV.cs.raterc;
            var oldatt = MainV2.comPort.MAV.cs.rateattitude;
            var oldpos = MainV2.comPort.MAV.cs.rateposition;
            var oldstatus = MainV2.comPort.MAV.cs.ratestatus;

            MainV2.comPort.MAV.cs.raterc = 10;
            MainV2.comPort.MAV.cs.rateattitude = 0;
            MainV2.comPort.MAV.cs.rateposition = 0;
            MainV2.comPort.MAV.cs.ratestatus = 0;

            try
            {
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RC_CHANNELS, 10);
            }
            catch
            {
            }

            BUT_Calibrateradio.Text = Strings.Click_when_Done;

            CustomMessageBox.Show(
                "Click OK and move all RC sticks and switches to their\nextreme positions so the red bars hit the limits.");

            run = true;


            while (run)
            {
                Application.DoEvents();

                Thread.Sleep(5);

                MainV2.comPort.MAV.cs.UpdateCurrentSettings(currentStateBindingSource, true, MainV2.comPort);

                // check for non 0 values
                if (MainV2.comPort.MAV.cs.ch1in > 800 && MainV2.comPort.MAV.cs.ch1in < 2200)
                {
                    rcmin[0] = Math.Min(rcmin[0], MainV2.comPort.MAV.cs.ch1in);
                    rcmax[0] = Math.Max(rcmax[0], MainV2.comPort.MAV.cs.ch1in);

                    rcmin[1] = Math.Min(rcmin[1], MainV2.comPort.MAV.cs.ch2in);
                    rcmax[1] = Math.Max(rcmax[1], MainV2.comPort.MAV.cs.ch2in);

                    rcmin[2] = Math.Min(rcmin[2], MainV2.comPort.MAV.cs.ch3in);
                    rcmax[2] = Math.Max(rcmax[2], MainV2.comPort.MAV.cs.ch3in);

                    rcmin[3] = Math.Min(rcmin[3], MainV2.comPort.MAV.cs.ch4in);
                    rcmax[3] = Math.Max(rcmax[3], MainV2.comPort.MAV.cs.ch4in);

                    rcmin[4] = Math.Min(rcmin[4], MainV2.comPort.MAV.cs.ch5in);
                    rcmax[4] = Math.Max(rcmax[4], MainV2.comPort.MAV.cs.ch5in);

                    rcmin[5] = Math.Min(rcmin[5], MainV2.comPort.MAV.cs.ch6in);
                    rcmax[5] = Math.Max(rcmax[5], MainV2.comPort.MAV.cs.ch6in);

                    rcmin[6] = Math.Min(rcmin[6], MainV2.comPort.MAV.cs.ch7in);
                    rcmax[6] = Math.Max(rcmax[6], MainV2.comPort.MAV.cs.ch7in);

                    rcmin[7] = Math.Min(rcmin[7], MainV2.comPort.MAV.cs.ch8in);
                    rcmax[7] = Math.Max(rcmax[7], MainV2.comPort.MAV.cs.ch8in);

                    rcmin[8] = Math.Min(rcmin[8], MainV2.comPort.MAV.cs.ch9in);
                    rcmax[8] = Math.Max(rcmax[8], MainV2.comPort.MAV.cs.ch9in);

                    rcmin[9] = Math.Min(rcmin[9], MainV2.comPort.MAV.cs.ch10in);
                    rcmax[9] = Math.Max(rcmax[9], MainV2.comPort.MAV.cs.ch10in);

                    rcmin[10] = Math.Min(rcmin[10], MainV2.comPort.MAV.cs.ch11in);
                    rcmax[10] = Math.Max(rcmax[10], MainV2.comPort.MAV.cs.ch11in);

                    rcmin[11] = Math.Min(rcmin[11], MainV2.comPort.MAV.cs.ch12in);
                    rcmax[11] = Math.Max(rcmax[11], MainV2.comPort.MAV.cs.ch12in);

                    rcmin[12] = Math.Min(rcmin[12], MainV2.comPort.MAV.cs.ch13in);
                    rcmax[12] = Math.Max(rcmax[12], MainV2.comPort.MAV.cs.ch13in);

                    rcmin[13] = Math.Min(rcmin[13], MainV2.comPort.MAV.cs.ch14in);
                    rcmax[13] = Math.Max(rcmax[13], MainV2.comPort.MAV.cs.ch14in);

                    rcmin[14] = Math.Min(rcmin[14], MainV2.comPort.MAV.cs.ch15in);
                    rcmax[14] = Math.Max(rcmax[14], MainV2.comPort.MAV.cs.ch15in);

                    rcmin[15] = Math.Min(rcmin[15], MainV2.comPort.MAV.cs.ch16in);
                    rcmax[15] = Math.Max(rcmax[15], MainV2.comPort.MAV.cs.ch16in);

                    BARroll.minline = (int) rcmin[chroll - 1];
                    BARroll.maxline = (int) rcmax[chroll - 1];

                    BARpitch.minline = (int) rcmin[chpitch - 1];
                    BARpitch.maxline = (int) rcmax[chpitch - 1];

                    BARthrottle.minline = (int) rcmin[chthro - 1];
                    BARthrottle.maxline = (int) rcmax[chthro - 1];

                    BARyaw.minline = (int) rcmin[chyaw - 1];
                    BARyaw.maxline = (int) rcmax[chyaw - 1];

                    setBARStatus(BAR5, rcmin[4], rcmax[4]);
                    setBARStatus(BAR6, rcmin[5], rcmax[5]);
                    setBARStatus(BAR7, rcmin[6], rcmax[6]);
                    setBARStatus(BAR8, rcmin[7], rcmax[7]);

                    setBARStatus(BAR9, rcmin[8], rcmax[8]);
                    setBARStatus(BAR10, rcmin[9], rcmax[9]);
                    setBARStatus(BAR11, rcmin[10], rcmax[10]);
                    setBARStatus(BAR12, rcmin[11], rcmax[11]);
                    setBARStatus(BAR13, rcmin[12], rcmax[12]);
                    setBARStatus(BAR14, rcmin[13], rcmax[13]);
                }
            }

            if (rcmin[0] > 800 && rcmin[0] < 2200)
            {
            }
            else
            {
                CustomMessageBox.Show("Bad channel 1 input, canceling");
                return;
            }

            CustomMessageBox.Show("Ensure all your sticks are centered and throttle is down, and click ok to continue");

            MainV2.comPort.MAV.cs.UpdateCurrentSettings(currentStateBindingSource, true, MainV2.comPort);

            rctrim[0] = MainV2.comPort.MAV.cs.ch1in;
            rctrim[1] = MainV2.comPort.MAV.cs.ch2in;
            rctrim[2] = MainV2.comPort.MAV.cs.ch3in;
            rctrim[3] = MainV2.comPort.MAV.cs.ch4in;
            rctrim[4] = MainV2.comPort.MAV.cs.ch5in;
            rctrim[5] = MainV2.comPort.MAV.cs.ch6in;
            rctrim[6] = MainV2.comPort.MAV.cs.ch7in;
            rctrim[7] = MainV2.comPort.MAV.cs.ch8in;

            rctrim[8] = MainV2.comPort.MAV.cs.ch9in;
            rctrim[9] = MainV2.comPort.MAV.cs.ch10in;
            rctrim[10] = MainV2.comPort.MAV.cs.ch11in;
            rctrim[11] = MainV2.comPort.MAV.cs.ch12in;
            rctrim[12] = MainV2.comPort.MAV.cs.ch13in;
            rctrim[13] = MainV2.comPort.MAV.cs.ch14in;
            rctrim[14] = MainV2.comPort.MAV.cs.ch15in;
            rctrim[15] = MainV2.comPort.MAV.cs.ch16in;

            var data = "---------------\n";

            for (var a = 0; a < rctrim.Length; a++)
            {
                // we want these to save no matter what
                BUT_Calibrateradio.Text = Strings.Saving;
                try
                {
                    if (rcmin[a] != rcmax[a])
                    {
                        MainV2.comPort.setParam("RC" + (a + 1).ToString("0") + "_MIN", rcmin[a]);
                        MainV2.comPort.setParam("RC" + (a + 1).ToString("0") + "_MAX", rcmax[a]);
                    }
                    if (rctrim[a] < 1195 || rctrim[a] > 1205)
                        MainV2.comPort.setParam("RC" + (a + 1).ToString("0") + "_TRIM", rctrim[a]);
                }
                catch
                {
                    if (MainV2.comPort.MAV.param.ContainsKey("RC" + (a + 1).ToString("0") + "_MIN"))
                        CustomMessageBox.Show("Failed to set Channel " + (a + 1));
                }

                data = data + "CH" + (a + 1) + " " + rcmin[a] + " | " + rcmax[a] + "\n";
            }

            MainV2.comPort.MAV.cs.raterc = oldrc;
            MainV2.comPort.MAV.cs.rateattitude = oldatt;
            MainV2.comPort.MAV.cs.rateposition = oldpos;
            MainV2.comPort.MAV.cs.ratestatus = oldstatus;

            try
            {
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RC_CHANNELS, oldrc);
            }
            catch
            {
            }

            CustomMessageBox.Show(
                "Here are the detected radio options\nNOTE Channels not connected are displayed as 1500 +-2\nNormal values are around 1100 | 1900\nChannel:Min | Max \n" +
                data, "Radio");

            BUT_Calibrateradio.Text = Strings.Completed;
        }

        private void setBARStatus(HorizontalProgressBar2 bar, float min, float max)
        {
            bar.minline = (int) min;
            bar.maxline = (int) max;
        }

        private void CHK_revch1_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARroll);
        }

        private void CHK_revch2_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARpitch);
        }

        private void CHK_revch3_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARthrottle);
        }

        private void CHK_revch4_CheckedChanged(object sender, EventArgs e)
        {
            reverseChannel(((CheckBox) sender).Checked, BARyaw);
        }

        private void reverseChannel(bool normalreverse, Control progressbar)
        {
            if (normalreverse)
            {
                ((HorizontalProgressBar2) progressbar).reverse = true;
                ((HorizontalProgressBar2) progressbar).BackgroundColor = Color.FromArgb(148, 193, 31);
                ((HorizontalProgressBar2) progressbar).ValueColor = Color.FromArgb(0x43, 0x44, 0x45);
            }
            else
            {
                ((HorizontalProgressBar2) progressbar).reverse = false;
                ((HorizontalProgressBar2) progressbar).BackgroundColor = Color.FromArgb(0x43, 0x44, 0x45);
                ((HorizontalProgressBar2) progressbar).ValueColor = Color.FromArgb(148, 193, 31);
            }

            if (startup)
                return;
            if (MainV2.comPort.MAV.param["SWITCH_ENABLE"] != null &&
                (float) MainV2.comPort.MAV.param["SWITCH_ENABLE"] == 1)
            {
                try
                {
                    MainV2.comPort.setParam("SWITCH_ENABLE", 0);
                    CustomMessageBox.Show("Disabled Dip Switchs");
                }
                catch
                {
                    CustomMessageBox.Show("Error Disableing Dip Switch");
                }
            }
        }

        private void BUT_Bindradiodsm2_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommand(MAVLink.MAV_CMD.START_RX_PAIR, 0, 0, 0, 0, 0, 0, 0);
                CustomMessageBox.Show(Strings.Put_the_transmitter_in_bind_mode__Receiver_is_waiting);
            }
            catch
            {
                CustomMessageBox.Show(Strings.Error_binding);
            }
        }

        private void BUT_BindradiodsmX_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommand(MAVLink.MAV_CMD.START_RX_PAIR, 0, 1, 0, 0, 0, 0, 0);
                CustomMessageBox.Show(Strings.Put_the_transmitter_in_bind_mode__Receiver_is_waiting);
            }
            catch
            {
                CustomMessageBox.Show(Strings.Error_binding);
            }
        }

        private void BUT_Bindradiodsm8_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommand(MAVLink.MAV_CMD.START_RX_PAIR, 0, 2, 0, 0, 0, 0, 0);
                CustomMessageBox.Show(Strings.Put_the_transmitter_in_bind_mode__Receiver_is_waiting);
            }
            catch
            {
                CustomMessageBox.Show(Strings.Error_binding);
            }
        }

    


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                BUT_SaveModes_Click(null, null);
                return true;
            }

            return false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.MAV.cs.UpdateCurrentSettings(currentStateBindingSource);
            }
            catch
            {
            }

            float pwm = 0;

            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduPlane ||
                MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduRover ||
                MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.Ateryx) // APM 
            {
                if (MainV2.comPort.MAV.param.ContainsKey("FLTMODE_CH") ||
                    MainV2.comPort.MAV.param.ContainsKey("MODE_CH"))
                {
                    var sw = 0;
                    if (MainV2.comPort.MAV.param.ContainsKey("FLTMODE_CH"))
                    {
                        sw = (int)MainV2.comPort.MAV.param["FLTMODE_CH"].Value;
                    }
                    else
                    {
                        sw = (int)MainV2.comPort.MAV.param["MODE_CH"].Value;
                    }

                    switch (sw)
                    {
                        case 5:
                            pwm = MainV2.comPort.MAV.cs.ch5in;
                            break;
                        case 6:
                            pwm = MainV2.comPort.MAV.cs.ch6in;
                            break;
                        case 7:
                            pwm = MainV2.comPort.MAV.cs.ch7in;
                            break;
                        case 8:
                            pwm = MainV2.comPort.MAV.cs.ch8in;
                            break;
                        default:

                            break;
                    }

                    if (MainV2.comPort.MAV.param.ContainsKey("FLTMODE_CH"))
                    {
                        LBL_flightmodepwm.Text = MainV2.comPort.MAV.param["FLTMODE_CH"] + ": " + pwm;
                    }
                    else
                    {
                        LBL_flightmodepwm.Text = MainV2.comPort.MAV.param["MODE_CH"] + ": " + pwm;
                    }
                }
            }

            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2) // ac2
            {
                pwm = MainV2.comPort.MAV.cs.ch5in;
                LBL_flightmodepwm.Text = "5: " + MainV2.comPort.MAV.cs.ch5in;
            }

            Control[] fmodelist = { CMB_fmode1, CMB_fmode2, CMB_fmode3, CMB_fmode4, CMB_fmode5, CMB_fmode6 };

            foreach (var ctl in fmodelist)
            {
                ThemeManager.ApplyThemeTo(ctl);
            }

            var no = readSwitch(pwm);

            fmodelist[no].BackColor = ThemeManager.CurrentPPMBackground;
        }

        // from arducopter code
        private byte readSwitch(float inpwm)
        {
            var pulsewidth = (int)inpwm; // default for Arducopter

            if (pulsewidth > 1230 && pulsewidth <= 1360) return 1;
            if (pulsewidth > 1360 && pulsewidth <= 1490) return 2;
            if (pulsewidth > 1490 && pulsewidth <= 1620) return 3;
            if (pulsewidth > 1620 && pulsewidth <= 1749) return 4; // Software Manual
            if (pulsewidth >= 1750) return 5; // Hardware Manual
            return 0;
        }

        private void BUT_SaveModes_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainV2.comPort.MAV.param.ContainsKey("FLTMODE1"))
                {
                    MainV2.comPort.setParam("FLTMODE1", int.Parse(CMB_fmode1.SelectedValue.ToString()));
                    MainV2.comPort.setParam("FLTMODE2", int.Parse(CMB_fmode2.SelectedValue.ToString()));
                    MainV2.comPort.setParam("FLTMODE3", int.Parse(CMB_fmode3.SelectedValue.ToString()));
                    MainV2.comPort.setParam("FLTMODE4", int.Parse(CMB_fmode4.SelectedValue.ToString()));
                    MainV2.comPort.setParam("FLTMODE5", int.Parse(CMB_fmode5.SelectedValue.ToString()));
                    MainV2.comPort.setParam("FLTMODE6", int.Parse(CMB_fmode6.SelectedValue.ToString()));
                }
                else if (MainV2.comPort.MAV.param.ContainsKey("MODE1"))
                {
                    MainV2.comPort.setParam("MODE1", int.Parse(CMB_fmode1.SelectedValue.ToString()));
                    MainV2.comPort.setParam("MODE2", int.Parse(CMB_fmode2.SelectedValue.ToString()));
                    MainV2.comPort.setParam("MODE3", int.Parse(CMB_fmode3.SelectedValue.ToString()));
                    MainV2.comPort.setParam("MODE4", int.Parse(CMB_fmode4.SelectedValue.ToString()));
                    MainV2.comPort.setParam("MODE5", int.Parse(CMB_fmode5.SelectedValue.ToString()));
                    MainV2.comPort.setParam("MODE6", int.Parse(CMB_fmode6.SelectedValue.ToString()));
                }
                else if (MainV2.comPort.MAV.param.ContainsKey("COM_FLTMODE1"))
                {
                    MainV2.comPort.setParam("COM_FLTMODE1", int.Parse(CMB_fmode1.SelectedValue.ToString()));
                    MainV2.comPort.setParam("COM_FLTMODE2", int.Parse(CMB_fmode2.SelectedValue.ToString()));
                    MainV2.comPort.setParam("COM_FLTMODE3", int.Parse(CMB_fmode3.SelectedValue.ToString()));
                    MainV2.comPort.setParam("COM_FLTMODE4", int.Parse(CMB_fmode4.SelectedValue.ToString()));
                    MainV2.comPort.setParam("COM_FLTMODE5", int.Parse(CMB_fmode5.SelectedValue.ToString()));
                    MainV2.comPort.setParam("COM_FLTMODE6", int.Parse(CMB_fmode6.SelectedValue.ToString()));
                }

                if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2) // ac2
                {
                    // simple
                    var value = (float)(CB_simple1.Checked ? (int)SimpleMode.Simple1 : 0) +
                                (CB_simple2.Checked ? (int)SimpleMode.Simple2 : 0) +
                                (CB_simple3.Checked ? (int)SimpleMode.Simple3 : 0)
                                + (CB_simple4.Checked ? (int)SimpleMode.Simple4 : 0) +
                                (CB_simple5.Checked ? (int)SimpleMode.Simple5 : 0) +
                                (CB_simple6.Checked ? (int)SimpleMode.Simple6 : 0);
                    if (MainV2.comPort.MAV.param.ContainsKey("SIMPLE"))
                        MainV2.comPort.setParam("SIMPLE", value);

                    // supersimple
                    value = (float)(chk_ss1.Checked ? (int)SimpleMode.Simple1 : 0) +
                            (chk_ss2.Checked ? (int)SimpleMode.Simple2 : 0) +
                            (chk_ss3.Checked ? (int)SimpleMode.Simple3 : 0)
                            + (chk_ss4.Checked ? (int)SimpleMode.Simple4 : 0) +
                            (chk_ss5.Checked ? (int)SimpleMode.Simple5 : 0) +
                            (chk_ss6.Checked ? (int)SimpleMode.Simple6 : 0);
                    if (MainV2.comPort.MAV.param.ContainsKey("SUPER_SIMPLE"))
                        MainV2.comPort.setParam("SUPER_SIMPLE", value);
                }
            }
            catch
            {
                CustomMessageBox.Show(Strings.ErrorSettingParameter, Strings.ERROR);
            }
            BUT_SaveModes.Text = "Complete";
        }

        private void updateDropDown(ComboBox ctl, string param)
        {
            ctl.DataSource = Common.getModesList(MainV2.comPort.MAV.cs);
            ctl.DisplayMember = "Value";
            ctl.ValueMember = "Key";
        }

        //private void linkLabel1_ss_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("http://ardupilot.org/copter/docs/simpleandsuper-simple-modes.html");
        //    }
        //    catch
        //    {
        //        CustomMessageBox.Show(Strings.ERROR +
        //                              " http://ardupilot.org/copter/docs/simpleandsuper-simple-modes.html");
        //    }
        //}

        private void flightmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainV2.comPort.MAV.cs.firmware == MainV2.Firmwares.ArduCopter2)
            {
                var sender2 = (Control)sender;
                var currentmode = sender2.Text.ToLower();

                if (currentmode.Contains("althold") || currentmode.Contains("auto") ||
                    currentmode.Contains("autotune") || currentmode.Contains("land") ||
                    currentmode.Contains("loiter") || currentmode.Contains("ofloiter") ||
                    currentmode.Contains("poshold") || currentmode.Contains("rtl") ||
                    currentmode.Contains("sport") || currentmode.Contains("stabilize"))
                {
                    //CMB_fmode1
                    //CB_simple1
                    //chk_ss1

                    var number = sender2.Name.Substring(sender2.Name.Length - 1);

                    findandenableordisable("CB_simple" + number, true);
                    findandenableordisable("chk_ss" + number, true);
                }
                else
                {
                    var number = sender2.Name.Substring(sender2.Name.Length - 1);

                    findandenableordisable("CB_simple" + number, false);
                    findandenableordisable("chk_ss" + number, false);
                }
            }
        }

        private void findandenableordisable(string ctl, bool enable)
        {
            var items = Controls.Find(ctl, true);

            if (items.Length > 0)
            {
                items[0].Enabled = enable;
            }
        }

    }
}



