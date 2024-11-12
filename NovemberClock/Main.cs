using NovemberClock.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovemberClock
{
    public partial class Main : Form
    {
        public static Color BackgroundColor= Color.FromArgb(255,13,13,13);
        private bool timerStarted = false;

        public static Main INSTANCE;

        public static Main GetInstance()
        {
            if(INSTANCE == null)
            {
                return new Main();
            }
            return INSTANCE;
        }
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }
        private void Main_Shown(object sender, EventArgs e)
        {
            Debug.WriteLine("Main Shown");
            StartClock();
        }
        public int GetClockHeight()
        {
            return Clock.Height;
        }
        public int GetClockWidth()
        {
            return Clock.Width;
        }
        public Panel GetClock()
        {
            return Clock;
        }
        private void Clock_Paint(object sender, PaintEventArgs e)
        {
            Debug.WriteLine("Paint method called!");
            Bitmap bitmap = NovemberClock.Logic.Clock.PaintClock();
            e.Graphics.DrawImage(bitmap, Point.Empty);


        }

        private void StartClock()
        {
            if (!timerStarted)
            {
                var timer = new System.Threading.Timer(Timer_Tick, null, 0, 10000);  // 1 second interval
                timerStarted = true;
            }
        }

        private void Timer_Tick(object state)
        {
            Debug.WriteLine("timer ticked");
            // Use Invoke to call Invalidate on the UI thread
            this.Invoke((MethodInvoker)delegate
            {
                this.Clock.Invalidate();  // This will trigger the Clock_Paint method
            });
        }
    }
}
