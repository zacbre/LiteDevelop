using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LiteDevelop.Gui.Forms
{
    public partial class SplashScreen : Form
    {
        public event EventHandler FadedIn;
        private ShadowForm _shadow;

        public SplashScreen()
        {
            InitializeComponent();

            _shadow = new ShadowForm();
            _shadow.Enabled = false;
            _shadow.Show();
            

            this.BringToFront();
            Disposed += SplashScreen_Disposed;
            Shown += new EventHandler(SplashScreen_Shown);
#if DEBUG
            versionLabel.Text = string.Format("v{0} (Debug)", Application.ProductVersion);
#else
            versionLabel.Text = string.Format("v{0}", Application.ProductVersion);
#endif


        }

        private void SplashScreen_Shown(object sender, EventArgs e)
        {
            if (_shadow != null)
            {
                _shadow.Left = (this.Left + (this.Width / 2)) - _shadow.Width / 2;
                _shadow.Top = (this.Top + (this.Height / 2)) - _shadow.Height / 2;
            }
        }

        private void SplashScreen_Disposed(object sender, EventArgs e)
        {
            if (_shadow != null)
            {
                _shadow.Dispose();
            }
        }

        private void fadeInTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity == 1)
            {
                if (fadeInTimer.Interval == 1000)
                {
                    fadeInTimer.Stop();
                    OnFadedIn(EventArgs.Empty);
                }
                else
                {
                    fadeInTimer.Interval = 1000;
                }
            }
            else
            {
                this.Opacity += 0.05;
                if (_shadow != null)
                {
                    _shadow.UpdateLayeredWindow((byte)(this.Opacity * 200));
                }
            }
        }

        protected virtual void OnFadedIn(EventArgs e)
        {
            if (FadedIn != null)
                FadedIn(this, e);
        }
        
        private class ShadowForm : Form
        {
            #region WinAPI

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct BLENDFUNCTION
            {
                public byte BlendOp;
                public byte BlendFlags;
                public byte SourceConstantAlpha;
                public byte AlphaFormat;
            }

            public const Int32 ULW_ALPHA = 0x00000002;
            public const byte AC_SRC_OVER = 0x00;
            public const byte AC_SRC_ALPHA = 0x01;

            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("user32.dll", ExactSpelling = true)]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern int DeleteDC(IntPtr hdc);

            [DllImport("gdi32.dll", ExactSpelling = true)]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern int DeleteObject(IntPtr hObject);

            #endregion

            private IntPtr _screenDc;
            private IntPtr _compatibleDc;
            private IntPtr _bitmapHandle;
            private IntPtr _oldBitmap;
            private Size _bitmapSize;

            public ShadowForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                SetLayeredWindowBitmap(Properties.Resources.shadow);
                StartPosition = FormStartPosition.CenterScreen;
                ShowInTaskbar = false;
                this.CenterToScreen();
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x00080000;
                    return cp;
                }
            }

            public void SetLayeredWindowBitmap(Bitmap bitmap)
            {
                _screenDc = GetDC(IntPtr.Zero);
                _compatibleDc = CreateCompatibleDC(_screenDc);
                _bitmapHandle = IntPtr.Zero;
                _oldBitmap = IntPtr.Zero;

                _bitmapHandle = bitmap.GetHbitmap(Color.FromArgb(0)); 
                _oldBitmap = SelectObject(_compatibleDc, _bitmapHandle);

                _bitmapSize = new Size(bitmap.Width, bitmap.Height);

                Size = _bitmapSize;
            }

            public void UpdateLayeredWindow(byte opacity)
            {
                Point pointSource = new Point(0, 0);
                Point topPos = new Point(Left, Top);
                var blend = new BLENDFUNCTION();
                blend.BlendOp = AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = AC_SRC_ALPHA;

                UpdateLayeredWindow(Handle, _screenDc, ref topPos, ref _bitmapSize, _compatibleDc, ref pointSource, 0, ref blend, ULW_ALPHA);
            }

            protected override void Dispose(bool disposing)
            {
                ReleaseDC(IntPtr.Zero, _screenDc);
                if (_bitmapHandle != IntPtr.Zero)
                {
                    SelectObject(_compatibleDc, _oldBitmap);
                    DeleteObject(_bitmapHandle);
                }
                DeleteDC(_compatibleDc);
                base.Dispose(disposing);
            }

        }
    }


}
