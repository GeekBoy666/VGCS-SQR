using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using System.Threading;

using System.Drawing.Drawing2D;
using log4net;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
//using OpenTK.Graphics;


// Control written by Michael Oborne 2011
// dual opengl and GDI+

namespace MissionPlanner.Controls
{
    public class HUD : GLControl
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private object paintlock = new object();
        private object streamlock = new object();
        /// <QYJ>
        /// 创建图片资源
        /// </QYJ>
        Bitmap bmp = new Bitmap(MissionPlanner.Controls.Properties.Resources.hud);
        Bitmap bmpLock = new Bitmap(MissionPlanner.Controls.Properties.Resources.Lock);
        Bitmap bmpUnLock = new Bitmap(MissionPlanner.Controls.Properties.Resources.Unlock);
        /// <QYJ>
        /// 创建图片资源
        /// </QYJ>
        private MemoryStream _streamjpg = new MemoryStream();
        //[System.ComponentModel.Browsable(false)]
        public MemoryStream streamjpg
        {
            get
            {
                lock (streamlock)
                {
                    return _streamjpg;
                }
            }
            set
            {
                lock (streamlock)
                {
                    _streamjpg = value;
                }
            }
        }

        private DateTime textureResetDateTime = DateTime.Now;

        /// <summary>
        /// this is to reduce cpu usage
        /// </summary>
        public bool streamjpgenable = false;

        public bool HoldInvalidation = false;

        public bool Russian { get; set; }

        private class character
        {
            public Bitmap bitmap;
            public int gltextureid;
            public int width;
            public int size;
        }

        private Dictionary<int, character> charDict = new Dictionary<int, character>();

        public int huddrawtime = 0;

        [DefaultValue(true)]
        public bool opengl { get; set; }

        [Browsable(false)]
        public bool npotSupported { get; private set; }

        public bool SixteenXNine = false;

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayheading { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayspeed { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayalt { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayconninfo { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayxtrack { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayrollpitch { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displaygps { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool bgon { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool hudon { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool batteryon { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayekf { get; set; }

        [System.ComponentModel.Browsable(true), DefaultValue(true)]
        public bool displayvibe { get; set; }

        private static ImageCodecInfo ici = GetImageCodec("image/jpeg");
        private static EncoderParameters eps = new EncoderParameters(1);

        private bool started = false;

        public HUD()
        {
            opengl = false;
            displayvibe =
                displayekf =
                    displayheading =
                        displayspeed =
                            displayalt =
                                displayconninfo =
                                    displayxtrack = displayrollpitch = displaygps = bgon = hudon = batteryon = true;

            this.Name = "Hud";

            eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
            // or whatever other quality value you want

            objBitmap.MakeTransparent();

            graphicsObject = this;
            graphicsObjectGDIP = Graphics.FromImage(objBitmap);
        }

        private float _roll = 0;
        private float _navroll = 0;
        private float _pitch = 0;
        private float _navpitch = 0;
        private float _heading = 0;
        private float _targetheading = 0;
        private float _alt = 0;
        private float _targetalt = 0;
        private float _groundspeed = 0;
        private float _airspeed = 0;
        private bool _lowgroundspeed = false;
        private bool _lowairspeed = false;
        private float _targetspeed = 0;
        private float _batterylevel = 0;
        private float _current = 0;
        private float _batteryremaining = 0;
        private float _gpsfix = 0;
        private float _gpshdop = 0;
        private float _gpsfix2 = 0;
        private float _gpshdop2 = 0;
        private float _disttowp = 0;
        private float _groundcourse = 0;
        private float _xtrack_error = 0;
        private float _turnrate = 0;
        private float _verticalspeed = 0;
        private float _linkqualitygcs = 0;
        private DateTime _datetime;
        private string _mode = "Manual";
        private int _wpno = 0;

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float roll
        {
            get { return _roll; }
            set
            {
                if (_roll != value)
                {
                    _roll = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float navroll
        {
            get { return _navroll; }
            set
            {
                if (_navroll != value)
                {
                    _navroll = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float pitch
        {
            get { return _pitch; }
            set
            {
                if (_pitch != value)
                {
                    _pitch = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float navpitch
        {
            get { return _navpitch; }
            set
            {
                if (_navpitch != value)
                {
                    _navpitch = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float heading
        {
            get { return _heading; }
            set
            {
                if (_heading != value)
                {
                    _heading = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float targetheading
        {
            get { return _targetheading; }
            set
            {
                if (_targetheading != value)
                {
                    _targetheading = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float alt
        {
            get { return _alt; }
            set
            {
                if (_alt != value)
                {
                    _alt = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float targetalt
        {
            get { return _targetalt; }
            set
            {
                if (_targetalt != value)
                {
                    _targetalt = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float groundspeed
        {
            get { return _groundspeed; }
            set
            {
                if (_groundspeed != value)
                {
                    _groundspeed = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float airspeed
        {
            get { return _airspeed; }
            set
            {
                if (_airspeed != value)
                {
                    _airspeed = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public bool lowgroundspeed
        {
            get { return _lowgroundspeed; }
            set
            {
                if (_lowgroundspeed != value)
                {
                    _lowgroundspeed = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public bool lowairspeed
        {
            get { return _lowairspeed; }
            set
            {
                if (_lowairspeed != value)
                {
                    _lowairspeed = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float targetspeed
        {
            get { return _targetspeed; }
            set
            {
                if (_targetspeed != value)
                {
                    _targetspeed = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float batterylevel
        {
            get { return _batterylevel; }
            set
            {
                if (_batterylevel != value)
                {
                    _batterylevel = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float batteryremaining
        {
            get { return _batteryremaining; }
            set
            {
                if (_batteryremaining != value)
                {
                    _batteryremaining = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float current
        {
            get { return _current; }
            set
            {
                if (_current != value)
                {
                    _current = value;
                    this.Invalidate();
                    if (_current > 0) batteryon = true;
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float gpsfix
        {
            get { return _gpsfix; }
            set
            {
                if (_gpsfix != value)
                {
                    _gpsfix = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float gpshdop
        {
            get { return _gpshdop; }
            set
            {
                if (_gpshdop != value)
                {
                    _gpshdop = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float gpsfix2
        {
            get { return _gpsfix2; }
            set
            {
                if (_gpsfix2 != value)
                {
                    _gpsfix2 = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float gpshdop2
        {
            get { return _gpshdop2; }
            set
            {
                if (_gpshdop2 != value)
                {
                    _gpshdop2 = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float disttowp
        {
            get { return _disttowp; }
            set
            {
                if (_disttowp != value)
                {
                    _disttowp = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public string mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public int wpno
        {
            get { return _wpno; }
            set
            {
                if (_wpno != value)
                {
                    _wpno = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float groundcourse
        {
            get { return _groundcourse; }
            set
            {
                if (_groundcourse != value)
                {
                    _groundcourse = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float xtrack_error
        {
            get { return _xtrack_error; }
            set
            {
                if (_xtrack_error != value)
                {
                    _xtrack_error = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float turnrate
        {
            get { return _turnrate; }
            set
            {
                if (_turnrate != value)
                {
                    _turnrate = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float verticalspeed
        {
            get { return _verticalspeed; }
            set
            {
                if (_verticalspeed != Math.Round(value, 1))
                {
                    _verticalspeed = (float)Math.Round(value, 1);
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float linkqualitygcs
        {
            get { return _linkqualitygcs; }
            set
            {
                if (_linkqualitygcs != value)
                {
                    _linkqualitygcs = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public DateTime datetime
        {
            get { return _datetime; }
            set
            {
                if (_datetime.Hour == value.Hour && _datetime.Minute == value.Minute && _datetime.Second == value.Second)
                    return;
                if (_datetime != value)
                {
                    _datetime = value;
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public bool failsafe { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public bool lowvoltagealert { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public bool connected { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float groundalt { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public bool status { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public string message { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public DateTime messagetime { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float vibex { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float vibey { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float vibez { get; set; }

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public float ekfstatus { get; set; }

        private bool statuslast = false;
        private DateTime armedtimer = DateTime.MinValue;

        public struct Custom
        {
            //public Point Position;
            //public float FontSize;
            public string Header;
            public System.Reflection.PropertyInfo Item;

            public double GetValue
            {
                get
                {
                    if (Item.PropertyType == typeof(Single))
                    {
                        return (double)(float)Item.GetValue(src, null);
                    }
                    if (Item.PropertyType == typeof(Int32))
                    {
                        return (double)(int)Item.GetValue(src, null);
                    }
                    if (Item.PropertyType == typeof(double))
                    {
                        return (double)Item.GetValue(src, null);
                    }

                    throw new Exception("Bad data type");
                }
            }

            public static object src { get; set; }
        }

        public Hashtable CustomItems = new Hashtable();

        [System.ComponentModel.Browsable(true), System.ComponentModel.Category("Values")]
        public Color hudcolor
        {
            get { return this._whitePen.Color; }
            set
            {
                _hudcolor = value;
                this._whitePen = new Pen(value, 2);
            }
        }

        private Color _hudcolor = Color.White;
        private Pen _whitePen = new Pen(Color.White, 2);
        private Pen SpeedLine = new Pen(Color.Yellow, 2);
        private Pen _pollLine = new Pen(Color.FromArgb(255, 255, 255), 2);
        private readonly SolidBrush SpeedBrush = new SolidBrush(Color.LightGreen);

        private readonly SolidBrush _whiteBrush = new SolidBrush(Color.White);
        private readonly SolidBrush _blackBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush _YellowBrush = new SolidBrush(Color.Yellow);
        private readonly SolidBrush _RollWhiteBrush = new SolidBrush(Color.FromArgb(63, 63, 63));
        private readonly SolidBrush _RollYellowBrush = new SolidBrush(Color.FromArgb(255, 0, 0));
        private readonly SolidBrush _BPStrBrush = new SolidBrush(Color.FromArgb(255, 255, 255));

        private static readonly SolidBrush SolidBrush = new SolidBrush(Color.FromArgb(0x55, 0xff, 0xff, 0xff));
        private static readonly SolidBrush BPBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        private static readonly SolidBrush PollStrBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        private static readonly SolidBrush SlightlyTransparentWhiteBrush = new SolidBrush(Color.FromArgb(220, 255, 255, 255));
        private static readonly SolidBrush AltGroundBrush = new SolidBrush(Color.FromArgb(100, Color.BurlyWood));

        private readonly object _bgimagelock = new object();

        public Image bgimage
        {
            set
            {
                lock (this._bgimagelock)
                {
                    try
                    {
                        _bgimage = (Image)value;
                    }
                    catch
                    {
                        _bgimage = null;
                    }
                    this.Invalidate();
                }
            }
            get { return _bgimage; }
        }

        private Image _bgimage;

        // move these global as they rarely change - reduce GC
        private Font font = new Font(HUDT.Font, 10);
        public Bitmap objBitmap = new Bitmap(1024, 1024, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        private int count = 0;
        private DateTime countdate = DateTime.Now;
        private HUD graphicsObject;
        private Graphics graphicsObjectGDIP;

        private DateTime starttime = DateTime.MinValue;

        private System.ComponentModel.ComponentResourceManager resources =
            new System.ComponentModel.ComponentResourceManager(typeof(HUD));



        public override void Refresh()
        {
            if (!ThisReallyVisible())
            {
                //  return;
            }

            //base.Refresh();
            using (Graphics gg = this.CreateGraphics())
            {
                OnPaint(new PaintEventArgs(gg, this.ClientRectangle));
            }
        }

        DateTime lastinvalidate = DateTime.MinValue;

        /// <summary>
        /// Override to prevent offscreen drawing the control - mono mac
        /// </summary>
        public new void Invalidate()
        {
            if (HoldInvalidation)
                return;

            if (!ThisReallyVisible())
            {
                //  return;
            }

            lastinvalidate = DateTime.Now;

            base.Invalidate();
        }

        /// <summary>
        /// this is to fix a mono off screen drawing issue
        /// </summary>
        /// <returns></returns>
        public bool ThisReallyVisible()
        {
            //Control ctl = Control.FromHandle(this.Handle);
            return this.Visible;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (opengl)
            {
                try
                {

                    OpenTK.Graphics.GraphicsMode test = this.GraphicsMode;
                    // log.Info(test.ToString());
                    log.Info("Vendor: " + GL.GetString(StringName.Vendor));
                    log.Info("Version: " + GL.GetString(StringName.Version));
                    log.Info("Device: " + GL.GetString(StringName.Renderer));
                    //Console.WriteLine("Extensions: " + GL.GetString(StringName.Extensions));

                    int[] viewPort = new int[4];

                    GL.GetInteger(GetPName.Viewport, viewPort);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(0, Width, Height, 0, -1, 1);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();

                    GL.PushAttrib(AttribMask.DepthBufferBit);
                    GL.Disable(EnableCap.DepthTest);
                    //GL.Enable(EnableCap.Texture2D); 
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    GL.Enable(EnableCap.Blend);

                    string versionString = GL.GetString(StringName.Version);
                    string majorString = versionString.Split(' ')[0];
                    var v = new Version(majorString);
                    npotSupported = v.Major >= 2;
                }
                catch (Exception ex) { log.Error("HUD opengl onload 1 ", ex); }

                try
                {
                    GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

                    GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
                    GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
                    GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);

                    GL.Hint(HintTarget.TextureCompressionHint, HintMode.Nicest);
                }
                catch (Exception ex) { log.Error("HUD opengl onload 2 ", ex); }

                try
                {

                    GL.Enable(EnableCap.LineSmooth);
                    GL.Enable(EnableCap.PointSmooth);
                    GL.Enable(EnableCap.PolygonSmooth);

                }
                catch (Exception ex) { log.Error("HUD opengl onload 3 ", ex); }
            }

            started = true;
        }

        public event EventHandler ekfclick;
        public event EventHandler vibeclick;

        Rectangle ekfhitzone = new Rectangle();
        Rectangle vibehitzone = new Rectangle();

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (ekfhitzone.IntersectsWith(new Rectangle(e.X, e.Y, 5, 5)))
            {
                if (ekfclick != null)
                    ekfclick(this, null);
            }

            if (vibehitzone.IntersectsWith(new Rectangle(e.X, e.Y, 5, 5)))
            {
                if (vibeclick != null)
                    vibeclick(this, null);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (ekfhitzone.IntersectsWith(new Rectangle(e.X, e.Y, 5, 5)))
            {
                Cursor.Current = Cursors.Hand;
            }
            else if (vibehitzone.IntersectsWith(new Rectangle(e.X, e.Y, 5, 5)))
            {
                Cursor.Current = Cursors.Hand;
            }
            else
            {
                Cursor.Current = DefaultCursor;
            }
        }

        bool inOnPaint = false;
        string otherthread = "";


        protected override void OnPaint(PaintEventArgs e)
        {
            //GL.Enable(EnableCap.AlphaTest)

            // Console.WriteLine("hud paint");

            // Console.WriteLine("hud ms " + (DateTime.Now.Millisecond));

            if (!started)
                return;

            if (this.DesignMode)
            {
                e.Graphics.Clear(this.BackColor);
                e.Graphics.Flush();
                opengl = false;
                doPaint(e);
                opengl = true;
                return;
            }

            if ((DateTime.Now - starttime).TotalMilliseconds < 30 && (_bgimage == null))
            {
                //Console.WriteLine("ms "+(DateTime.Now - starttime).TotalMilliseconds);
                //e.Graphics.DrawImageUnscaled(objBitmap, 0, 0);          
                return;
            }

            // force texture reset
            if (textureResetDateTime.Hour != DateTime.Now.Hour)
            {
                textureResetDateTime = DateTime.Now;
                doResize();
            }

            lock (this)
            {

                if (inOnPaint)
                {
                    log.Info("Was in onpaint Hud th:" + System.Threading.Thread.CurrentThread.Name + " in " + otherthread);
                    return;
                }

                otherthread = System.Threading.Thread.CurrentThread.Name;

                inOnPaint = true;

            }

            starttime = DateTime.Now;

            try
            {

                if (opengl)
                {
                    // make this gl window and thread current
                    if (!Context.IsCurrent)
                        MakeCurrent();

                    GL.Clear(ClearBufferMask.ColorBufferBit);

                }

                doPaint(e);

                if (opengl)
                {
                    this.SwapBuffers();

                    // free from this thread
                    //Context.MakeCurrent(null);
                }

            }
            catch (Exception ex) { log.Info(ex.ToString()); }

            count++;

            huddrawtime += (int)(DateTime.Now - starttime).TotalMilliseconds;

            if (DateTime.Now.Second != countdate.Second)
            {
                countdate = DateTime.Now;
                Console.WriteLine("HUD " + count + " hz drawtime " + (huddrawtime / count) + " gl " + opengl);
                if ((huddrawtime / count) > 1000)
                    opengl = false;

                count = 0;
                huddrawtime = 0;
            }

            lock (this)
            {
                inOnPaint = false;
            }
        }

        void Clear(Color color)
        {
            if (opengl)
            {
                GL.ClearColor(color);

            }
            else
            {
                graphicsObjectGDIP.Clear(color);
            }
        }

        const float rad2deg = (float)(180 / Math.PI);
        const float deg2rad = (float)(1.0 / rad2deg);

        public void DrawArc(Pen penn, RectangleF rect, float start, float degrees)
        {
            if (opengl)
            {
                GL.LineWidth(penn.Width);
                GL.Color4(penn.Color);

                GL.Begin(PrimitiveType.LineStrip);

                start = 360 - start;
                start -= 30;

                float x = 0, y = 0;
                for (float i = start; i <= start + degrees; i++)
                {
                    x = (float)Math.Sin(i * deg2rad) * rect.Width / 2;
                    y = (float)Math.Cos(i * deg2rad) * rect.Height / 2;
                    x = x + rect.X + rect.Width / 2;
                    y = y + rect.Y + rect.Height / 2;
                    GL.Vertex2(x, y);
                }
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.DrawArc(penn, rect, start, degrees);
            }
        }

        public void DrawEllipse(Pen penn, Rectangle rect)
        {
            if (opengl)
            {
                GL.LineWidth(penn.Width);
                GL.Color4(penn.Color);

                GL.Begin(PrimitiveType.LineLoop);
                float x, y;
                for (float i = 0; i < 360; i += 1)
                {
                    x = (float)Math.Sin(i * deg2rad) * rect.Width / 2;
                    y = (float)Math.Cos(i * deg2rad) * rect.Height / 2;
                    x = x + rect.X + rect.Width / 2;
                    y = y + rect.Y + rect.Height / 2;
                    GL.Vertex2(x, y);
                }
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.DrawEllipse(penn, rect);
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.ClearOutputChannelColorProfile();
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private character[] _texture = new character[2];

        public void DrawImage(Image img, int x, int y, int width, int height, int textureno = 0)
        {
            if (opengl)
            {
                if (img == null)
                    return;

                if (_texture[textureno] == null)
                    _texture[textureno] = new character();

                // If the image is already a bitmap and we support NPOT textures then simply use it.
                if (npotSupported && img is Bitmap)
                {
                    _texture[textureno].bitmap = (Bitmap)img;
                }
                else
                {
                    // Otherwise we have to resize img to be POT.
                    _texture[textureno].bitmap = ResizeImage(img, _texture[textureno].bitmap.Width, _texture[textureno].bitmap.Height);
                }

                // generate the texture
                if (_texture[textureno].gltextureid == 0)
                {
                    GL.GenTextures(1, out _texture[textureno].gltextureid);
                }

                GL.BindTexture(TextureTarget.Texture2D, _texture[textureno].gltextureid);

                BitmapData data = _texture[textureno].bitmap.LockBits(
                    new Rectangle(0, 0, _texture[textureno].bitmap.Width, _texture[textureno].bitmap.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // create the texture type/dimensions
                if (_texture[textureno].width != _texture[textureno].bitmap.Width)
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                    _texture[textureno].width = data.Width;
                }
                else
                {
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, data.Width, data.Height,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                }

                _texture[textureno].bitmap.UnlockBits(data);

                bool polySmoothEnabled = GL.IsEnabled(EnableCap.PolygonSmooth);
                if (polySmoothEnabled)
                    GL.Disable(EnableCap.PolygonSmooth);

                GL.Enable(EnableCap.Texture2D);

                GL.BindTexture(TextureTarget.Texture2D, _texture[textureno].gltextureid);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                GL.Begin(PrimitiveType.TriangleStrip);

                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(x, y);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(x, y + height);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(x + width, y);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(x + width, y + height);

                GL.End();

                GL.Disable(EnableCap.Texture2D);

                if (polySmoothEnabled)
                    GL.Enable(EnableCap.PolygonSmooth);
            }
            else
            {
                graphicsObjectGDIP.DrawImage(img, x, y, width, height);
            }
        }

        public void DrawPath(Pen penn, GraphicsPath gp)
        {
            try
            {
                DrawPolygon(penn, gp.PathPoints);
            }
            catch { }
        }

        public void FillPath(Brush brushh, GraphicsPath gp)
        {
            try
            {
                FillPolygon(brushh, gp.PathPoints);
            }
            catch { }
        }

        public void SetClip(Rectangle rect)
        {

        }

        public void ResetClip()
        {

        }

        public void ResetTransform()
        {
            if (opengl)
            {
                GL.LoadIdentity();
            }
            else
            {
                graphicsObjectGDIP.ResetTransform();
            }
        }

        public void RotateTransform(float angle)
        {
            if (opengl)
            {
                GL.Rotate(angle, 0, 0, 1);
            }
            else
            {
                graphicsObjectGDIP.RotateTransform(angle);
            }
        }

        public void TranslateTransform(float x, float y)
        {
            if (opengl)
            {
                GL.Translate(x, y, 0f);
            }
            else
            {
                graphicsObjectGDIP.TranslateTransform(x, y);
            }
        }

        public void FillPolygon(Brush brushh, Point[] list)
        {
            if (opengl)
            {
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Color4(((SolidBrush)brushh).Color);
                foreach (Point pnt in list)
                {
                    GL.Vertex2(pnt.X, pnt.Y);
                }
                GL.Vertex2(list[list.Length - 1].X, list[list.Length - 1].Y);
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.FillPolygon(brushh, list);
            }
        }

        public void FillPolygon(Brush brushh, PointF[] list)
        {
            if (opengl)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color4(((SolidBrush)brushh).Color);
                foreach (PointF pnt in list)
                {
                    GL.Vertex2(pnt.X, pnt.Y);
                }
                GL.Vertex2(list[0].X, list[0].Y);
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.FillPolygon(brushh, list);
            }
        }

        public void DrawPolygon(Pen penn, Point[] list)
        {
            if (opengl)
            {
                GL.LineWidth(penn.Width);
                GL.Color4(penn.Color);

                GL.Begin(PrimitiveType.LineLoop);
                foreach (Point pnt in list)
                {
                    GL.Vertex2(pnt.X, pnt.Y);
                }
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.DrawPolygon(penn, list);
            }
        }

        public void DrawPolygon(Pen penn, PointF[] list)
        {
            if (opengl)
            {
                GL.LineWidth(penn.Width);
                GL.Color4(penn.Color);

                GL.Begin(PrimitiveType.LineLoop);
                foreach (PointF pnt in list)
                {
                    GL.Vertex2(pnt.X, pnt.Y);
                }

                GL.End();
            }
            else
            {
                graphicsObjectGDIP.DrawPolygon(penn, list);
            }
        }


        public void FillRectangle(Brush brushh, RectangleF rectf)
        {
            if (opengl)
            {
                float x1 = rectf.X;
                float y1 = rectf.Y;

                float width = rectf.Width;
                float height = rectf.Height;

                GL.Begin(PrimitiveType.Quads);

                GL.LineWidth(0);

                if (((Type)brushh.GetType()) == typeof(LinearGradientBrush))
                {
                    LinearGradientBrush temp = (LinearGradientBrush)brushh;
                    GL.Color4(temp.LinearColors[0]);
                }
                else
                {
                    GL.Color4(((SolidBrush)brushh).Color.R / 255f, ((SolidBrush)brushh).Color.G / 255f, ((SolidBrush)brushh).Color.B / 255f, ((SolidBrush)brushh).Color.A / 255f);
                }

                GL.Vertex2(x1, y1);
                GL.Vertex2(x1 + width, y1);

                if (((Type)brushh.GetType()) == typeof(LinearGradientBrush))
                {
                    LinearGradientBrush temp = (LinearGradientBrush)brushh;
                    GL.Color4(temp.LinearColors[1]);
                }
                else
                {
                    GL.Color4(((SolidBrush)brushh).Color.R / 255f, ((SolidBrush)brushh).Color.G / 255f, ((SolidBrush)brushh).Color.B / 255f, ((SolidBrush)brushh).Color.A / 255f);
                }

                GL.Vertex2(x1 + width, y1 + height);
                GL.Vertex2(x1, y1 + height);
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.FillRectangle(brushh, rectf);
            }
        }

        public void DrawRectangle(Pen penn, RectangleF rect)
        {
            DrawRectangle(penn, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(Pen penn, double x1, double y1, double width, double height)
        {

            if (opengl)
            {
                GL.LineWidth(penn.Width);
                GL.Color4(penn.Color);

                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex2(x1, y1);
                GL.Vertex2(x1 + width, y1);
                GL.Vertex2(x1 + width, y1 + height);
                GL.Vertex2(x1, y1 + height);
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.DrawRectangle(penn, (float)x1, (float)y1, (float)width, (float)height);
            }
        }

        public void DrawLine(Pen penn, double x1, double y1, double x2, double y2)
        {

            if (opengl)
            {
                GL.Color4(penn.Color);
                GL.LineWidth(penn.Width);

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x1, y1);
                GL.Vertex2(x2, y2);
                GL.End();
            }
            else
            {
                graphicsObjectGDIP.DrawLine(penn, (float)x1, (float)y1, (float)x2, (float)y2);
            }
        }

        private readonly Pen _blackPen = new Pen(Color.Black, 2);
        private readonly Pen _greenPen = new Pen(Color.Green, 2);
        private readonly Pen _redPen = new Pen(Color.Red, 2);

        //QYJ
        private readonly Pen ConnectPen = new Pen(Color.FromArgb(134, 175, 75), 3);
        private readonly Pen DisConnectPen = new Pen(Color.FromArgb(250, 250, 250), 3);
        private readonly Pen _rollwhtiePen = new Pen(Color.FromArgb(63, 63, 63), 2);
        private readonly Pen _rollyellowPen = new Pen(Color.FromArgb(255, 62, 120), 2);
        private readonly Pen _yellowPen = new Pen(Color.FromArgb(255, 62, 120), 2);
        //QYJ

        void doPaint(PaintEventArgs e)
        {
            //Console.WriteLine("hud paint "+DateTime.Now.Millisecond);
            bool isNaN = false;
            try
            {
                if (graphicsObjectGDIP == null || !opengl && (objBitmap.Width != this.Width || objBitmap.Height != this.Height))
                {
                    objBitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    objBitmap.MakeTransparent();
                    graphicsObjectGDIP = Graphics.FromImage(objBitmap);

                    graphicsObjectGDIP.SmoothingMode = SmoothingMode.HighSpeed;
                    graphicsObjectGDIP.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphicsObjectGDIP.CompositingMode = CompositingMode.SourceOver;
                    graphicsObjectGDIP.CompositingQuality = CompositingQuality.HighSpeed;
                    graphicsObjectGDIP.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    graphicsObjectGDIP.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                }

                graphicsObjectGDIP.InterpolationMode = InterpolationMode.Bilinear;

                try
                {
                    graphicsObject.Clear(Color.Transparent);
                }
                catch
                {
                    // this is the first posible opengl call
                    // in vmware fusion on mac, this fails, so switch back to legacy
                    opengl = false;
                }

                if (_bgimage != null)
                {
                    bgon = false;
                    lock (this._bgimagelock)
                        lock (_bgimage)
                        {
                            try
                            {
                                graphicsObject.DrawImage(_bgimage, 0, 0, this.Width, this.Height, 1);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                _bgimage = null;
                            }
                        }

                    if (hudon == false)
                    {
                        return;
                    }
                }
                else
                {
                    bgon = true;
                }

                // localize it
                float _roll = this._roll;

                if (float.IsNaN(_roll) || float.IsNaN(_pitch) || float.IsNaN(_heading))
                {
                    isNaN = true;

                    _roll = 0;
                    _pitch = 0;
                    _heading = 0;
                }

                graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);

                if (!Russian)
                {
                    // horizon
                    graphicsObject.RotateTransform(-_roll);
                }
                else
                {
                    _roll *= -1;
                }


                int fontsize = this.Height / 30; // = 10
                int fontoffset = fontsize - 10;

                float every5deg = -this.Height / 85;

                float pitchoffset = -_pitch * every5deg;

                int halfwidth = this.Width / 2;
                int halfheight = this.Height / 2;

                //QYJ
                //相关变量定义           
                int WidthShort = this.Width / 16;
                int WidthLong = this.Width / 13;

                int HeightShort = this.Height / 14;
                int HeightLong = this.Height / 10;

                float WidthShort1 = this.Width / 14;


                float HeightShort1 = this.Height / 14;
                float HeightLong1 = this.Height / 10;
                int Distance = (int)(this.Height / 9.0 * 1.1f);
                float EllipseRT1_height = (Distance + WidthLong * 3) * 2 + pitchoffset;
                float EllipseRT_height = (Distance + WidthLong * 3) * 2 - pitchoffset;
                //QYJ
                this._whiteBrush.Color = this._whitePen.Color;

                // Reset pens
                this._blackPen.Width = 2;
                this._greenPen.Width = 2;
                this._redPen.Width = 2;

                if (!connected)
                {
                    this._whiteBrush.Color = Color.LightGray;
                    this._whitePen.Color = Color.LightGray;
                }
                else
                {
                    this._whitePen.Color = _hudcolor;
                }

                // draw sky
                if (bgon == true)
                {
                    RectangleF bg = new RectangleF(-halfwidth * 2, -halfheight * 2, this.Width * 2, halfheight * 2 + pitchoffset);

                    if (bg.Height != 0)
                    {
                        using (LinearGradientBrush linearBrush = new LinearGradientBrush(
                            bg, Color.Blue, Color.LightBlue, LinearGradientMode.Vertical))
                        {
                            graphicsObject.FillRectangle(linearBrush, bg);
                        }
                    }
                    // draw ground

                    bg = new RectangleF(-halfwidth * 2, pitchoffset, this.Width * 2, halfheight * 2 - pitchoffset);

                    if (bg.Height != 0)
                    {
                        using (
                            LinearGradientBrush linearBrush = new LinearGradientBrush(
                                bg, Color.FromArgb(250, 145, 0), Color.FromArgb(161, 94, 0), LinearGradientMode.Vertical))
                        {
                            graphicsObject.FillRectangle(linearBrush, bg);
                        }
                    }

                    //draw centerline
                    graphicsObject.DrawLine(this._whitePen, -halfwidth * 2, pitchoffset + 0, halfwidth * 2, pitchoffset + 0);
                }

                graphicsObject.ResetTransform();

                if (displayrollpitch)
                {
                    //QYJ
                    graphicsObject.SetClip(new Rectangle(0, this.Height / 14, this.Width, this.Height - this.Height / 14));

                    graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);

                    graphicsObject.RotateTransform(-_roll);

                    //draw pitch           

                    int lengthshort = this.Width / 14;
                    int lengthlong = this.Width / 10;

                    for (int a = -90; a <= 90; a += 5)
                    {
                        // limit to 40 degrees
                        if (a >= _pitch - 19 && a <= _pitch + 10)
                        {
                            if (a % 10 == 0)
                            {
                                if (a == 0)
                                {
                                    graphicsObject.DrawLine(_rollyellowPen, -WidthLong * 1.0f, pitchoffset + a * every5deg, WidthLong * 1.0f, pitchoffset + a * every5deg);
                                }
                                else
                                {
                                    graphicsObject.DrawLine(_pollLine, -WidthLong * 1.0f, pitchoffset + a * every5deg, WidthLong * 1.0f, pitchoffset + a * every5deg);
                                }
                                graphicsObjectGDIP.DrawString(a.ToString(), new Font(new FontFamily("黑体"), 13, FontStyle.Bold, GraphicsUnit.Pixel), PollStrBrush, new PointF(-WidthLong * 1.0f - 20, pitchoffset + a * every5deg - 8));
                            }
                            else
                            {

                                graphicsObject.DrawLine(_pollLine, -WidthLong * 1.0f + 10, pitchoffset + a * every5deg, WidthLong * 1.0f - 10, pitchoffset + a * every5deg);
                            }
                        }
                    }

                    graphicsObject.ResetTransform();

                    // draw roll ind needle

                    graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);

                    lengthlong = this.Height / 66;

                    int extra = (int)(this.Height / 19.0 * 3.5f);

                    int lengthlongex = lengthlong + 2;

                    PointF[] pointlist = new PointF[4];
                    pointlist[0] = new PointF(0, -WidthLong * 2.3f + 3);
                    pointlist[1] = new PointF(-this.Width / 30, -WidthLong * 2.3f + 20);
                    pointlist[2] = new PointF(0, -WidthLong * 2.3f + 13);
                    pointlist[3] = new PointF(this.Width / 30, -WidthLong * 2.3f + 20);

                    graphicsObject.FillPolygon(_RollYellowBrush, pointlist);

                    this._redPen.Width = 2;

                    int[] array = new int[] { -60, -55, -50, -45, -40, -34, -30, -25, -20, -15, -10, -5, 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60 };

                    foreach (int a in array)
                    {
                        graphicsObject.ResetTransform();
                        graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);
                        graphicsObject.RotateTransform(a - _roll);
                        if (Math.Abs(a) >= 50)
                        {
                            if (Math.Abs(a) % 10 == 0)
                            {
                                graphicsObject.DrawLine(this._rollyellowPen, 0, -WidthLong * 2.3f, 0, -WidthLong * 2.3f - 10);
                                graphicsObjectGDIP.DrawString(String.Format("{0,2}", Math.Abs(a)), new Font(new FontFamily("黑体"), 10, FontStyle.Bold, GraphicsUnit.Pixel), _RollYellowBrush, new PointF(-6, -WidthLong * 2.6f - 10));
                            }
                            else
                            {
                                graphicsObject.DrawLine(this._rollyellowPen, 0, -WidthLong * 2.3f, 0, -WidthLong * 2.3f - 5);
                            }
                        }
                        else
                        {
                            if (Math.Abs(a) % 10 == 0)
                            {
                                graphicsObject.DrawLine(this._rollwhtiePen, 0, -WidthLong * 2.3f, 0, -WidthLong * 2.3f - 10);
                                graphicsObjectGDIP.DrawString(String.Format("{0,2}", Math.Abs(a)), new Font(new FontFamily("黑体"), 10, FontStyle.Bold, GraphicsUnit.Pixel), _RollWhiteBrush, new PointF(-6, -WidthLong * 2.6f - 10));
                            }
                            else
                            {
                                graphicsObject.DrawLine(this._rollwhtiePen, 0, -WidthLong * 2.3f, 0, -WidthLong * 2.3f - 5);
                            }
                        }
                    }

                    graphicsObject.ResetTransform();
                    graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);

                    // draw roll ind
                    RectangleF arcrect = new RectangleF(-WidthLong * 2.5f, -WidthLong * 2.5f, WidthLong * 5, WidthLong * 5f);

                    //DrawRectangle(Pens.Beige, arcrect);

                    //graphicsObject.DrawArc(this._whitePen, arcrect, 180 + 30 + -_roll, 120); // 120

                    graphicsObject.ResetTransform();

                    //draw centre / current att

                    graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);//  +this.Height / 14);

                    // plane wings
                    if (Russian)
                        graphicsObject.RotateTransform(-_roll);

                    RectangleF CenteEsp = new RectangleF(-(this.Width / 16), -this.Width / 20, (this.Width / 16) * 2, (this.Width / 16) * 2);
                    using (Pen redtemp = new Pen(Color.FromArgb(255, 255, 255), 3.0f))
                    {
                        graphicsObjectGDIP.DrawImage(bmp, CenteEsp);
                    }
                    //QYJ
                }

                //draw heading ind
                Rectangle headbg = new Rectangle(-WidthLong * 3 - Distance, -WidthLong * 3 - Distance, (Distance + WidthLong * 3) * 2, (Distance + WidthLong * 3) * 2);
                graphicsObject.ResetClip();

                if (displayheading)
                {

                    GraphicsPath gp3 = new GraphicsPath();
                    gp3.AddLine(-WidthLong, -WidthLong * 4 - 23, WidthLong, -WidthLong * 4 - 23);
                    gp3.AddLine(-WidthLong, -WidthLong * 4 - 23, -WidthLong, -WidthLong * 3 - 30);
                    gp3.AddLine(-WidthLong, -WidthLong * 3 - 30, -WidthLong / 4 - 3, -WidthLong * 3 - 30);
                    gp3.AddLine(-WidthLong / 4 + 4, -WidthLong * 3 - 25, WidthLong / 4 - 4, -WidthLong * 3 - 25);
                    gp3.AddLine(WidthLong / 4 + 3, -WidthLong * 3 - 30, WidthLong, -WidthLong * 3 - 30);
                    gp3.AddLine(WidthLong, -WidthLong * 3 - 30, WidthLong, -WidthLong * 4 - 23);
                    RectangleF headshow = new RectangleF(-WidthLong, -WidthLong * 4 - 23, WidthLong * 2, WidthLong);
                    using (LinearGradientBrush HeadShowBrush = new LinearGradientBrush(
                           headshow, Color.Black, Color.DarkSlateGray, LinearGradientMode.Vertical))
                    {
                        graphicsObjectGDIP.FillPath(HeadShowBrush, gp3);
                    }
                    using (Pen redtemp = new Pen(Color.FromArgb(27, 46, 46), 4.0f))
                    {
                        graphicsObjectGDIP.DrawLine(redtemp, 0, -WidthLong * 3 - Distance + 31, 0, -WidthLong * 3 - Distance + 5);
                    }

                    int headvalue = (int)_heading;
                    graphicsObjectGDIP.DrawString("偏航", new Font(new FontFamily("华文楷体"), 15, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, new PointF(-WidthLong * 3 + 15, -WidthLong * 4 - 23));
                    graphicsObjectGDIP.DrawString(headvalue.ToString(), new Font(new FontFamily("黑体"), 18, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, new PointF(-WidthLong + 10, -WidthLong * 4 - 23));

                    int[] array = new int[] { 0,5,10,15,20,25,30,35,40,45,50,55,60,65,70,75,80,85,90,95,100,105,110,115,120,125,130,135,140,145,150,155,160,165,170,175,180,185,190,
                                             195,200,205,210,215,220,225,230,235,240,245,250,255,260,265,270,275,280,285,290,295,300,305,310,315,320,325,330,330,335,340,345,350,355};

                    using (LinearGradientBrush linearBrush = new LinearGradientBrush(
                    headbg, Color.FromArgb(30, 0, 0, 255), Color.FromArgb(30, 0, 255, 0), LinearGradientMode.Vertical))
                    {
                        graphicsObjectGDIP.FillEllipse(linearBrush, headbg);
                    }

                    foreach (int b in array)
                    {

                        graphicsObject.ResetTransform();
                        graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);
                        graphicsObject.RotateTransform(b - _heading);
                        if (b == 0)
                        {
                            graphicsObjectGDIP.DrawString("N", new Font(new FontFamily("黑体"), 18, FontStyle.Bold, GraphicsUnit.Pixel), _BPStrBrush, new PointF(-7, this.Height / (-2) + 18));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 30)
                        {
                            graphicsObjectGDIP.DrawString("3", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 60)
                        {
                            graphicsObjectGDIP.DrawString("6", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 90)
                        {
                            graphicsObjectGDIP.DrawString("E", new Font(new FontFamily("黑体"), 18, FontStyle.Bold, GraphicsUnit.Pixel), _BPStrBrush, new PointF(-7, this.Height / (-2) + 18));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 120)
                        {
                            graphicsObjectGDIP.DrawString("12", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 150)
                        {
                            graphicsObjectGDIP.DrawString("15", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 180)
                        {
                            graphicsObjectGDIP.DrawString("S", new Font(new FontFamily("黑体"), 18, FontStyle.Bold, GraphicsUnit.Pixel), _BPStrBrush, new PointF(-7, this.Height / (-2) + 18));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 210)
                        {
                            graphicsObjectGDIP.DrawString("21", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 240)
                        {
                            graphicsObjectGDIP.DrawString("24", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 270)
                        {
                            graphicsObjectGDIP.DrawString("W", new Font(new FontFamily("黑体"), 18, FontStyle.Bold, GraphicsUnit.Pixel), _BPStrBrush, new PointF(-7, this.Height / (-2) + 18));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 300)
                        {
                            graphicsObjectGDIP.DrawString("30", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else if (b == 330)
                        {
                            graphicsObjectGDIP.DrawString("33", new Font(new FontFamily("黑体"), 13, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.White, new PointF(-7, this.Height / (-2) + 21));
                            graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 4, 11));
                        }
                        else
                        { graphicsObjectGDIP.FillRectangle(BPBrush, new Rectangle(-3, this.Height / (-2) + 35, 2, 8)); }

                    }

                }
                //                Console.WriteLine("HUD 0 " + (DateTime.Now - starttime).TotalMilliseconds + " " + DateTime.Now.Millisecond);

                // xtrack error
                // center
                graphicsObject.ResetTransform();


                // left scroller
                Rectangle scrollbg = new Rectangle(5, halfheight - halfheight + 50, this.Width / 8, this.Height - 100);

                if (displayspeed)
                {
                    graphicsObjectGDIP.DrawString("校正空速", new Font(new FontFamily("华文楷体"), 15, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, new PointF(3, halfheight - halfheight + 28));
                    graphicsObject.DrawRectangle(this._blackPen, scrollbg);

                    Point[] arrow = new Point[5];

                    arrow[0] = new Point(5, -10);
                    arrow[1] = new Point(scrollbg.Width - 10, -10);
                    arrow[2] = new Point(scrollbg.Width - 5, 0);
                    arrow[3] = new Point(scrollbg.Width - 10, 10);
                    arrow[4] = new Point(5, 10);

                    double viewrange = 26;
                    double speed = _airspeed;
                    if (speed == 0)
                        speed = _groundspeed;

                    double space = (scrollbg.Height) / viewrange;
                    double start = (long)(speed - viewrange / 2);

                    if (start > _targetspeed)
                    {
                        this._greenPen.Color = Color.FromArgb(0, 0, 0);
                        this._greenPen.Width = 6;
                        graphicsObject.DrawLine(this._blackPen, scrollbg.Left, scrollbg.Bottom, scrollbg.Left + scrollbg.Width / 5, scrollbg.Bottom);
                        this._greenPen.Color = Color.FromArgb(0, 0, 0);
                    }
                    if ((speed + viewrange / 2) < _targetspeed)
                    {
                        this._greenPen.Color = Color.FromArgb(0, 0, 0);
                        this._greenPen.Width = 6;
                        graphicsObject.DrawLine(this._blackPen, scrollbg.Left, scrollbg.Bottom - space * viewrange, scrollbg.Left + scrollbg.Width / 5, scrollbg.Bottom - space * viewrange);
                        this._greenPen.Color = Color.FromArgb(0, 0, 0);
                    }

                    long end = (long)(speed + viewrange / 2);
                    for (long a = (long)start; a <= end; a += 1)
                    {
                        if (a == (long)_targetspeed && _targetspeed != 0)
                        {
                            this._greenPen.Width = 6;
                            graphicsObject.DrawLine(this._blackPen, scrollbg.Left, scrollbg.Bottom - space * (a - start), scrollbg.Left + scrollbg.Width / 5, scrollbg.Bottom - space * (a - start));
                        }
                        if (a % 5 == 0)
                        {
                            //Console.WriteLine(a + " " + scrollbg.Right + " " + (scrollbg.Top - space * (a - start)) + " " + (scrollbg.Right - 20) + " " + (scrollbg.Top - space * (a - start)));
                            graphicsObject.DrawLine(this._blackPen, scrollbg.Left, scrollbg.Bottom - space * (a - start), scrollbg.Left + scrollbg.Width / 5, scrollbg.Bottom - space * (a - start));
                            drawstring(graphicsObject, String.Format("{0,5}", a), font, fontsize, _whiteBrush, 8, (float)(scrollbg.Bottom - space * (a - start) - 6 - fontoffset));
                        }
                    }
                    graphicsObject.TranslateTransform(0, this.Height / 2);
                    graphicsObject.DrawPolygon(this._blackPen, arrow);
                    graphicsObject.FillPolygon(Brushes.Black, arrow);
                    graphicsObject.DrawLine(SpeedLine, 5, 0, scrollbg.Width - 5, 0);
                    drawstring(graphicsObject, (speed).ToString("0"), font, 10, SpeedBrush, scrollbg.Right - 25, -9);

                    graphicsObject.ResetTransform();

                    //// extra text data

                    //if (_lowairspeed)
                    //{
                    //    drawstring(graphicsObject, HUDT.AS + _airspeed.ToString("0.0"), font, fontsize, (SolidBrush)Brushes.Red, 1, scrollbg.Bottom + 5);
                    //}
                    //else
                    //{
                    //    drawstring(graphicsObject, HUDT.AS + _airspeed.ToString("0.0"), font, fontsize, _whiteBrush, 1, scrollbg.Bottom + 5);
                    //}

                    //if (_lowgroundspeed)
                    //{
                    //    drawstring(graphicsObject, HUDT.GS + _groundspeed.ToString("0.0"), font, fontsize, (SolidBrush)Brushes.Red, 1, scrollbg.Bottom + fontsize + 2 + 10);
                    //}
                    //else
                    //{
                    //    drawstring(graphicsObject, HUDT.GS + _groundspeed.ToString("0.0"), font, fontsize, _whiteBrush, 1, scrollbg.Bottom + fontsize + 2 + 10);
                    //}
                }

                //drawstring(e,, new Font("Arial", fontsize + 2), whiteBrush, 1, scrollbg.Bottom + fontsize + 2 + 10);

                // right scroller
                scrollbg = new Rectangle(this.Width - this.Width / 8 - 5, halfheight - halfheight + 50, this.Width / 8, this.Height - 100);

                if (displayalt)
                {
                    graphicsObjectGDIP.DrawString("相对高度", new Font(new FontFamily("华文楷体"), 15, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, new PointF(this.Width - this.Width / 8 - 23, halfheight - halfheight + 28));
                    graphicsObject.DrawRectangle(this._blackPen, scrollbg);

                    Point[] arrow = new Point[5];
                    arrow[0] = new Point(5, -10);
                    arrow[1] = new Point(scrollbg.Width - 10, -10);
                    arrow[2] = new Point(scrollbg.Width - 5, 0);
                    arrow[3] = new Point(scrollbg.Width - 10, 10);
                    arrow[4] = new Point(5, 10);

                    int viewrange = 26;

                    float space = (scrollbg.Height) / (float)viewrange;
                    long start = ((int)_alt - viewrange / 2);

                    if (start > _targetalt)
                    {
                        this._greenPen.Color = Color.FromArgb(128, this._greenPen.Color);
                        this._greenPen.Width = 6;
                        graphicsObject.DrawLine(this._blackPen, scrollbg.Left, scrollbg.Bottom, scrollbg.Left + scrollbg.Width, scrollbg.Bottom);
                        this._greenPen.Color = Color.FromArgb(255, this._greenPen.Color);
                    }
                    if ((_alt + viewrange / 2) < _targetalt)
                    {
                        this._greenPen.Color = Color.FromArgb(128, this._greenPen.Color);
                        this._greenPen.Width = 6;
                        graphicsObject.DrawLine(this._blackPen, scrollbg.Left, scrollbg.Bottom - space * viewrange, scrollbg.Left + scrollbg.Width, scrollbg.Bottom - space * viewrange);
                        this._greenPen.Color = Color.FromArgb(255, this._greenPen.Color);
                    }

                    bool ground = false;

                    for (long a = start; a <= (_alt + viewrange / 2); a += 1)
                    {
                        if (a == Math.Round(_targetalt) && _targetalt != 0)
                        {
                            this._greenPen.Width = 6;
                            graphicsObject.DrawLine(this._blackPen, scrollbg.Left, scrollbg.Bottom - space * (a - start), scrollbg.Left + scrollbg.Width, scrollbg.Bottom - space * (a - start));
                        }


                        // ground doesnt appear if we are not in view or below ground level
                        if (a == Math.Round(groundalt) && groundalt != 0 && ground == false)
                        {
                            graphicsObject.FillRectangle(AltGroundBrush, new RectangleF(scrollbg.Left, scrollbg.Bottom - space * (a - start), scrollbg.Width, (space * (a - start))));
                        }

                        if (a % 5 == 0)
                        {
                            //Console.WriteLine(a + " " + scrollbg.Left + " " + (scrollbg.Top - space * (a - start)) + " " + (scrollbg.Left + 20) + " " + (scrollbg.Top - space * (a - start)));
                            graphicsObject.DrawLine(this._blackPen, scrollbg.Right, scrollbg.Bottom - space * (a - start), scrollbg.Right - 10, scrollbg.Bottom - space * (a - start));
                            drawstring(graphicsObject, String.Format("{0,5}", a), font, fontsize, _whiteBrush, scrollbg.Left + 0 + (int)(0 * fontoffset), scrollbg.Bottom - space * (a - start) - 6 - fontoffset);
                        }

                    }

                    this._greenPen.Width = 4;

                    // vsi

                    graphicsObject.ResetTransform();

                    PointF[] poly = new PointF[4];

                    poly[0] = new PointF(scrollbg.Left, scrollbg.Top);
                    poly[1] = new PointF(scrollbg.Left - scrollbg.Width / 4, scrollbg.Top + scrollbg.Width / 4);
                    poly[2] = new PointF(scrollbg.Left - scrollbg.Width / 4, scrollbg.Bottom - scrollbg.Width / 4);
                    poly[3] = new PointF(scrollbg.Left, scrollbg.Bottom);

                    ////verticalspeed

                    //viewrange = 12;

                    //_verticalspeed = Math.Min(viewrange / 2, _verticalspeed);
                    //_verticalspeed = Math.Max(viewrange / -2, _verticalspeed);

                    //float scaledvalue = _verticalspeed / -viewrange * (scrollbg.Bottom - scrollbg.Top);

                    //float linespace = (float)1 / -viewrange * (scrollbg.Bottom - scrollbg.Top);

                    //PointF[] polyn = new PointF[4];

                    //polyn[0] = new PointF(scrollbg.Left, scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2);
                    //polyn[1] = new PointF(scrollbg.Left - scrollbg.Width / 4, scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2);
                    //polyn[2] = polyn[1];
                    //float peak = 0;
                    //if (scaledvalue > 0)
                    //{
                    //    peak = -scrollbg.Width / 4;
                    //    if (scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2 + scaledvalue + peak < scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2)
                    //        peak = -scaledvalue;
                    //}
                    //else if (scaledvalue < 0)
                    //{
                    //    peak = +scrollbg.Width / 4;
                    //    if (scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2 + scaledvalue + peak > scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2)
                    //        peak = -scaledvalue;
                    //}

                    //polyn[2] = new PointF(scrollbg.Left - scrollbg.Width / 4, scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2 + scaledvalue + peak);
                    //polyn[3] = new PointF(scrollbg.Left, scrollbg.Top + (scrollbg.Bottom - scrollbg.Top) / 2 + scaledvalue);

                    //graphicsObject.DrawPolygon(redPen, poly);
                    //graphicsObject.FillPolygon(Brushes.Blue, polyn);

                    // draw outsidebox
                    //graphicsObject.DrawPolygon(this._whitePen, poly);

                    //for (int a = 1; a < viewrange; a++)
                    //{
                    //    graphicsObject.DrawLine(this._whitePen, scrollbg.Left - scrollbg.Width / 4, scrollbg.Top - linespace * a, scrollbg.Left - scrollbg.Width / 8, scrollbg.Top - linespace * a);
                    //}

                    // draw arrow and text

                    graphicsObject.ResetTransform();
                    graphicsObject.TranslateTransform(this.Width, this.Height / 2);
                    graphicsObject.RotateTransform(180);

                    graphicsObject.DrawPolygon(this._blackPen, arrow);
                    graphicsObject.FillPolygon(Brushes.Black, arrow);
                    graphicsObject.DrawLine(SpeedLine, 5, 0, scrollbg.Width - 5, 0);
                    graphicsObject.ResetTransform();
                    graphicsObject.TranslateTransform(0, this.Height / 2);

                    drawstring(graphicsObject, ((int)_alt).ToString("0"), font, 10, SpeedBrush, scrollbg.Left + 12, -9);
                    graphicsObject.ResetTransform();

                    // mode and wp dist and wp
                    //drawstring(graphicsObject, _mode, font, fontsize, _whiteBrush, scrollbg.Left - 30, scrollbg.Bottom + 5);
                    //drawstring(graphicsObject, (int)_disttowp + ">" + _wpno, font, fontsize, _whiteBrush, scrollbg.Left - 30, scrollbg.Bottom + fontsize + 2 + 10);
                }

                if (displayconninfo)
                {
                    //drawstring(graphicsObject, _linkqualitygcs.ToString("0") + "%", font, fontsize, _whiteBrush, scrollbg.Left, scrollbg.Top - (int)(fontsize * 2.2) - 2 - 20);
                    if (_linkqualitygcs == 0)
                    {
                        //graphicsObject.DrawLine(this._redPen, scrollbg.Left, scrollbg.Top - (int)(fontsize * 2.2) - 2 - 20, scrollbg.Left + 50, scrollbg.Top - (int)(fontsize * 2.2) - 2);

                        //graphicsObject.DrawLine(this._redPen, scrollbg.Left, scrollbg.Top - (int)(fontsize * 2.2) - 2, scrollbg.Left + 50, scrollbg.Top - (int)(fontsize * 2.2) - 2 - 20);
                        graphicsObject.DrawLine(DisConnectPen, scrollbg.Right, scrollbg.Top - 28, scrollbg.Right, scrollbg.Top - 50);
                        graphicsObject.DrawLine(DisConnectPen, scrollbg.Right - 5, scrollbg.Top - 28, scrollbg.Right - 5, scrollbg.Top - 45);
                        graphicsObject.DrawLine(DisConnectPen, scrollbg.Right - 10, scrollbg.Top - 28, scrollbg.Right - 10, scrollbg.Top - 40);
                        graphicsObject.DrawLine(DisConnectPen, scrollbg.Right - 15, scrollbg.Top - 28, scrollbg.Right - 15, scrollbg.Top - 35);
                        graphicsObject.DrawLine(DisConnectPen, scrollbg.Right - 20, scrollbg.Top - 28, scrollbg.Right - 20, scrollbg.Top - 31);


                    }
                    else
                    {
                        graphicsObject.DrawLine(ConnectPen, scrollbg.Right, scrollbg.Top - 28, scrollbg.Right, scrollbg.Top - 50);
                        graphicsObject.DrawLine(ConnectPen, scrollbg.Right - 5, scrollbg.Top - 28, scrollbg.Right - 5, scrollbg.Top - 45);
                        graphicsObject.DrawLine(ConnectPen, scrollbg.Right - 10, scrollbg.Top - 28, scrollbg.Right - 10, scrollbg.Top - 40);
                        graphicsObject.DrawLine(ConnectPen, scrollbg.Right - 15, scrollbg.Top - 28, scrollbg.Right - 15, scrollbg.Top - 35);
                        graphicsObject.DrawLine(ConnectPen, scrollbg.Right - 20, scrollbg.Top - 28, scrollbg.Right - 20, scrollbg.Top - 31);
                    }
                    //drawstring(graphicsObject, _datetime.ToString("HH:mm:ss"), font, fontsize, _whiteBrush, scrollbg.Left - 30, scrollbg.Top - fontsize - 2 - 20);
                }
                Rectangle batteryonTop = new Rectangle((this.Width / 52 + this.Width / 27) / 2, 2, this.Width / 45, this.Width / 75);
                RectangleF batteryonB = new RectangleF(this.Width / 52, 2 + this.Width / 75, this.Width / 27, this.Width / 21);
                graphicsObjectGDIP.FillRectangle(Brushes.White, batteryonTop);
                graphicsObject.DrawRectangle(this._whitePen, batteryonB);

                graphicsObjectGDIP.DrawString("V", new Font(new FontFamily("黑体"), 13, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.WhiteSmoke, new PointF(this.Width / 52 + this.Width / 8, 2));
                graphicsObjectGDIP.DrawString("A", new Font(new FontFamily("黑体"), 13, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.WhiteSmoke, new PointF(this.Width / 52 + this.Width / 8, 14));
                // battery
                if (batteryon)
                {
                    graphicsObject.ResetTransform();
                    double bianhuaALT = this.Width / 21 - this.Width / 21 * _batteryremaining / 100;
                    string text = _batterylevel.ToString("0.00");
                    string text1 = _current.ToString("0.0");

                    graphicsObjectGDIP.DrawString(text, new Font(new FontFamily("黑体"), 10, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, new PointF(this.Width / 52 + this.Width / 16, 2));
                    graphicsObjectGDIP.DrawString(text1, new Font(new FontFamily("黑体"), 10, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Black, new PointF(this.Width / 52 + this.Width / 16, 14));

                    if (lowvoltagealert)
                    {
                        graphicsObject.FillRectangle(Brushes.White, batteryonB);
                        graphicsObject.DrawLine(_yellowPen, this.Width / 52, 2 + this.Width / 75, this.Width / 52 + this.Width / 27, 2 + this.Width / 75 + this.Width / 21);
                        graphicsObject.DrawLine(_yellowPen, this.Width / 52 + this.Width / 27, 2 + this.Width / 75, this.Width / 52, 2 + this.Width / 75 + this.Width / 21);
                    }
                    else
                    {
                        batteryonB = new RectangleF(this.Width / 52, 2 + this.Width / 75 + (float)bianhuaALT, this.Width / 27, this.Width / 21 - (float)bianhuaALT);
                        graphicsObject.FillRectangle(Brushes.LightGreen, batteryonB);
                    }
                }
                // gps
                if (displaygps)
                {
                    string gps = "";
                    SolidBrush col = _whiteBrush;
                    var _fix = Math.Max(_gpsfix, _gpsfix2);

                    if (_fix == 0)
                    {
                        gps = (HUDT.GPS0);
                        col = (SolidBrush)Brushes.Red;
                    }
                    else if (_fix == 1)
                    {
                        gps = (HUDT.GPS1);
                        col = (SolidBrush)Brushes.Red;
                    }
                    else if (_fix == 2)
                    {
                        gps = (HUDT.GPS2);
                    }
                    else if (_fix == 3)
                    {
                        gps = (HUDT.GPS3);
                    }
                    else if (_fix == 4)
                    {
                        gps = (HUDT.GPS4);
                    }
                    else if (_fix == 5)
                    {
                        gps = (HUDT.GPS5);
                    }
                    else if (_fix == 6)
                    {
                        gps = (HUDT.GPS6);
                    }
                    else
                    {
                        gps = _fix.ToString();
                    }
                    graphicsObjectGDIP.DrawString("G", new Font(new FontFamily("黑体"), 20, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Red, new PointF(this.Width / 2 + this.Width / 5 - 15, 1));
                    graphicsObjectGDIP.DrawString("PS", new Font(new FontFamily("黑体"), 16, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Red, new PointF(this.Width / 2 + this.Width / 5 - 3, 7));
                    graphicsObjectGDIP.DrawString("：", new Font(new FontFamily("黑体"), 20, FontStyle.Regular, GraphicsUnit.Pixel), Brushes.Red, new PointF(this.Width / 2 + this.Width / 5 + 10, 2));
                    graphicsObjectGDIP.DrawString(gps, new Font(new FontFamily("华文楷体"), 15, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.WhiteSmoke, new PointF(this.Width / 2 + this.Width / 4 + 2, 5));
                }

                if (isNaN)
                    drawstring(graphicsObject, "NaN Error " + DateTime.Now, font, this.Height / 30 + 10, (SolidBrush)Brushes.Red, 50, 50);

                // custom user items
                graphicsObject.ResetTransform();
                int height = this.Height - 30 - fontoffset - fontsize - 8;
                foreach (string key in CustomItems.Keys)
                {
                    try
                    {
                        Custom item = (Custom)CustomItems[key];
                        if (item.Item == null)
                            continue;
                        if (item.Item.Name.Contains("lat") || item.Item.Name.Contains("lng"))
                        {
                            drawstring(graphicsObject, item.Header + item.GetValue.ToString("0.#######"), font, fontsize + 2, _whiteBrush, this.Width / 8, height);
                        }
                        else if (item.Item.Name == "battery_usedmah")
                        {
                            drawstring(graphicsObject, item.Header + item.GetValue.ToString("0"), font, fontsize + 2, _whiteBrush, this.Width / 8, height);
                        }
                        else if (item.Item.Name == "timeInAir")
                        {
                            double stime = item.GetValue;
                            int hrs = (int)(stime / (60 * 60));
                            //stime -= hrs * 60 * 60;
                            int mins = (int)(stime / (60)) % 60;
                            //stime = mins * 60;
                            int secs = (int)(stime % 60);
                            drawstring(graphicsObject, item.Header + hrs.ToString("00") + ":" + mins.ToString("00") + ":" + secs.ToString("00"), font, fontsize + 2, _whiteBrush, this.Width / 8, height);
                        }
                        else
                        {
                            drawstring(graphicsObject, item.Header + item.GetValue.ToString("0.##"), font, fontsize + 2, _whiteBrush, this.Width / 8, height);
                        }
                        height -= fontsize + 5;
                    }
                    catch { }

                }




                graphicsObject.TranslateTransform(this.Width / 2, this.Height / 2);

                // draw armed
                RectangleF RectUnLock = new RectangleF(scrollbg.Left + 10, scrollbg.Bottom + 8, 35, 35);

                if (status != statuslast)
                {
                    armedtimer = DateTime.Now;
                }

                if (status == false) // not armed
                {
                    //if ((armedtimer.AddSeconds(8) > DateTime.Now))
                    {
                        graphicsObjectGDIP.DrawImage(bmpLock, RectUnLock);
                        statuslast = status;
                    }
                }
                else if (status == true) // armed
                {
                    if ((armedtimer.AddSeconds(8) > DateTime.Now))
                    {
                        graphicsObjectGDIP.DrawImage(bmpUnLock, RectUnLock);
                        statuslast = status;
                    }
                }

                if (failsafe == true)
                {
                    drawstring(graphicsObject, HUDT.FAILSAFE, font, fontsize + 20, (SolidBrush)Brushes.Red, -85, halfheight / -HUDT.FailsafeH);
                    statuslast = status;
                }

                if (message != "" && messagetime.AddSeconds(10) > DateTime.Now)
                {
                    if (message == "PreArm: Accels not calibrated")
                    {


                        graphicsObjectGDIP.DrawString("加速度未校准", new Font(new FontFamily("华文楷体"), 20, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Red, new PointF(scrollbg.Left - 150, scrollbg.Bottom + 25));
                    }
                    else
                    {

                        graphicsObjectGDIP.DrawString(message, new Font(new FontFamily("华文楷体"), 20, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Red, new PointF(scrollbg.Left - 150, scrollbg.Bottom + 25));
                    }

                }

                graphicsObject.ResetTransform();

                //if (displayvibe)
                //{
                //    vibehitzone = new Rectangle(this.Width - 18*fontsize, this.Height - 30 - fontoffset, 40, fontsize*2);

                //    if (vibex > 30 || vibey > 30 || vibez > 30)
                //    {
                //        drawstring(graphicsObject, "Vibe", font, fontsize + 2, (SolidBrush) Brushes.Red, vibehitzone.X,
                //            vibehitzone.Y);
                //    }
                //    else
                //    {
                //        drawstring(graphicsObject, "Vibe", font, fontsize + 2, _whiteBrush, vibehitzone.X,
                //            vibehitzone.Y);
                //    }
                //}

                //if (displayekf)
                //{
                //    ekfhitzone = new Rectangle(this.Width - 23*fontsize, this.Height - 30 - fontoffset, 40, fontsize*2);

                //    if (ekfstatus > 0.5)
                //    {
                //        if (ekfstatus > 0.8)
                //        {
                //            drawstring(graphicsObject, "EKF", font, fontsize + 2, (SolidBrush) Brushes.Red, ekfhitzone.X,
                //                ekfhitzone.Y);
                //        }
                //        else
                //        {
                //            drawstring(graphicsObject, "EKF", font, fontsize + 2, (SolidBrush) Brushes.Orange,
                //                ekfhitzone.X,
                //                ekfhitzone.Y);
                //        }
                //    }
                //    else
                //    {
                //        drawstring(graphicsObject, "EKF", font, fontsize + 2, _whiteBrush, ekfhitzone.X, ekfhitzone.Y);
                //    }
                //}

                if (!opengl)
                {
                    e.Graphics.DrawImageUnscaled(objBitmap, 0, 0);
                }

                if (DesignMode)
                {
                    return;
                }

                //                Console.WriteLine("HUD 1 " + (DateTime.Now - starttime).TotalMilliseconds + " " + DateTime.Now.Millisecond);

                lock (streamlock)
                {
                    if (streamjpgenable || streamjpg == null) // init image and only update when needed
                    {
                        if (opengl)
                        {
                            objBitmap = GrabScreenshot();
                        }

                        streamjpg = new MemoryStream();
                        objBitmap.Save(streamjpg, ici, eps);
                        //objBitmap.Save(streamjpg,ImageFormat.Bmp);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("hud error " + ex.ToString());
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        static ImageCodecInfo GetImageCodec(string mimetype)
        {
            foreach (ImageCodecInfo ici in ImageCodecInfo.GetImageEncoders())
            {
                if (ici.MimeType == mimetype) return ici;
            }
            return null;
        }

        // Returns a System.Drawing.Bitmap with the contents of the current framebuffer
        public new Bitmap GrabScreenshot()
        {
            if (OpenTK.Graphics.GraphicsContext.CurrentContext == null)
                throw new OpenTK.Graphics.GraphicsContextMissingException();

            Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }


        float wrap360(float noin)
        {
            if (noin < 0)
                return noin + 360;
            return noin;
        }

        /// <summary>
        /// pen for drawstring
        /// </summary>
        private readonly Pen _p = new Pen(Color.FromArgb(0x26, 0x27, 0x28), 2f);
        /// <summary>
        /// pth for drawstring
        /// </summary>
        private readonly GraphicsPath pth = new GraphicsPath();

        void drawstring(HUD e, string text, Font font, float fontsize, SolidBrush brush, float x, float y)
        {
            if (!opengl)
            {
                drawstring(graphicsObjectGDIP, text, font, fontsize, brush, x, y);
                return;
            }

            if (text == null || text == "")
                return;
            /*
            OpenTK.Graphics.Begin(); 
            GL.PushMatrix(); 
            GL.Translate(x, y, 0);
            printer.Print(text, font, c); 
            GL.PopMatrix(); printer.End();
            */

            float maxy = 1;

            foreach (char cha in text)
            {
                int charno = (int)cha;

                int charid = charno ^ (int)(fontsize * 1000) ^ brush.Color.ToArgb();

                if (!charDict.ContainsKey(charid))
                {
                    charDict[charid] = new character() { bitmap = new Bitmap(128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb), size = (int)fontsize };

                    charDict[charid].bitmap.MakeTransparent(Color.Transparent);

                    //charbitmaptexid

                    float maxx = this.Width / 150; // for space


                    // create bitmap
                    using (Graphics gfx = Graphics.FromImage(charDict[charid].bitmap))
                    {
                        pth.Reset();

                        if (text != null)
                            pth.AddString(cha + "", font.FontFamily, 0, fontsize + 5, new Point((int)0, (int)0), StringFormat.GenericTypographic);

                        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        gfx.DrawPath(this._p, pth);

                        //Draw the face

                        gfx.FillPath(brush, pth);


                        if (pth.PointCount > 0)
                        {
                            foreach (PointF pnt in pth.PathPoints)
                            {
                                if (pnt.X > maxx)
                                    maxx = pnt.X;

                                if (pnt.Y > maxy)
                                    maxy = pnt.Y;
                            }
                        }
                    }

                    charDict[charid].width = (int)(maxx + 2);

                    //charbitmaps[charid] = charbitmaps[charid].Clone(new RectangleF(0, 0, maxx + 2, maxy + 2), charbitmaps[charid].PixelFormat);

                    //charbitmaps[charno * (int)fontsize].Save(charno + " " + (int)fontsize + ".png");

                    // create texture
                    int textureId;
                    GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvModeCombine.Replace);//Important, or wrong color on some computers

                    Bitmap bitmap = charDict[charid].bitmap;
                    GL.GenTextures(1, out textureId);
                    GL.BindTexture(TextureTarget.Texture2D, textureId);

                    BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                    //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
                    //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                    GL.Finish();
                    bitmap.UnlockBits(data);

                    charDict[charid].gltextureid = textureId;
                }

                float scale = 1.0f;

                // dont draw spaces
                if (cha != ' ')
                {
                    //GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

                    GL.Enable(EnableCap.Texture2D);
                    GL.BindTexture(TextureTarget.Texture2D, charDict[charid].gltextureid);

                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(x, y);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(x + charDict[charid].bitmap.Width * scale, y);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(x + charDict[charid].bitmap.Width * scale, y + charDict[charid].bitmap.Height * scale);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(x + 0, y + charDict[charid].bitmap.Height * scale);
                    GL.End();

                    //GL.Disable(EnableCap.Blend);
                    GL.Disable(EnableCap.Texture2D);
                }
                x += charDict[charid].width * scale;
            }
        }

        void drawstring(Graphics e, string text, Font font, float fontsize, SolidBrush brush, float x, float y)
        {
            if (text == null || text == "")
                return;

            float maxy = 0;

            foreach (char cha in text)
            {
                int charno = (int)cha;

                int charid = charno ^ (int)(fontsize * 1000) ^ brush.Color.ToArgb();

                if (!charDict.ContainsKey(charid))
                {
                    charDict[charid] = new character() { bitmap = new Bitmap(128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb), size = (int)fontsize };

                    charDict[charid].bitmap.MakeTransparent(Color.Transparent);

                    //charbitmaptexid

                    float maxx = this.Width / 150; // for space


                    // create bitmap
                    using (Graphics gfx = Graphics.FromImage(charDict[charid].bitmap))
                    {
                        pth.Reset();

                        if (text != null)
                            pth.AddString(cha + "", font.FontFamily, 0, fontsize + 5, new Point((int)0, (int)0), StringFormat.GenericTypographic);

                        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        gfx.DrawPath(this._p, pth);

                        //Draw the face

                        gfx.FillPath(brush, pth);


                        if (pth.PointCount > 0)
                        {
                            foreach (PointF pnt in pth.PathPoints)
                            {
                                if (pnt.X > maxx)
                                    maxx = pnt.X;

                                if (pnt.Y > maxy)
                                    maxy = pnt.Y;
                            }
                        }
                    }

                    charDict[charid].width = (int)(maxx + 2);
                }

                // draw it

                float scale = 1.0f;
                // dont draw spaces
                if (cha != ' ')
                {
                    DrawImage(charDict[charid].bitmap, (int)x, (int)y, charDict[charid].bitmap.Width, charDict[charid].bitmap.Height, charDict[charid].gltextureid);
                }
                else
                {

                }

                x += charDict[charid].width * scale;
            }

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            try
            {
                if (opengl)
                {
                    base.OnHandleCreated(e);
                }
            }
            catch (Exception ex) { log.Error("Expected failure on max/linux due to opengl support"); log.Error(ex); opengl = false; } // macs/linux fail here
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            try
            {
                if (opengl)
                {
                    base.OnHandleDestroyed(e);
                }
            }
            catch (Exception ex) { log.Info(ex.ToString()); opengl = false; }
        }

        public void doResize()
        {
            OnResize(EventArgs.Empty);
        }

        protected override void OnResize(EventArgs e)
        {
            if (DesignMode || !IsHandleCreated || !started)
                return;

            base.OnResize(e);

            if (SixteenXNine)
            {
                int ht = (int)(this.Width / 1.777f);
                if (ht >= this.Height + 5 || ht <= this.Height - 5)
                {
                    this.Height = ht;
                    return;
                }
            }
            else
            {
                // 4x3
                int ht = (int)(this.Width / 1.333f);
                if (ht >= this.Height + 5 || ht <= this.Height - 5)
                {
                    this.Height = ht;
                    return;
                }
            }

            graphicsObjectGDIP = Graphics.FromImage(objBitmap);

            try
            {
                foreach (character texid in charDict.Values)
                {
                    try
                    {
                        texid.bitmap.Dispose();
                    }
                    catch { }
                }

                if (opengl)
                {
                    foreach (character texid in _texture)
                    {
                        if (texid != null && texid.gltextureid != 0)
                            GL.DeleteTexture(texid.gltextureid);
                    }
                    this._texture = new character[_texture.Length];

                    foreach (character texid in charDict.Values)
                    {
                        if (texid.gltextureid != 0)
                            GL.DeleteTexture(texid.gltextureid);
                    }
                }

                charDict.Clear();
            }
            catch { }

            try
            {
                if (opengl)
                {
                    MakeCurrent();

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(0, Width, Height, 0, -1, 1);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();

                    GL.Viewport(0, 0, Width, Height);
                }
            }
            catch { }

            Refresh();
        }

        [Browsable(false)]
        public new bool VSync
        {
            get { return base.VSync; }
            set { base.VSync = value; }
        }
    }
}