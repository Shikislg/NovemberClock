using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;

namespace NovemberClock.Logic
{
    public class Clock
    {
        private static Image BackgroundImage;

        private static bool IsAM = true;

        private static Color color = Color.White;

        private static readonly int LineWidth = 5;

        private static readonly int ClockHeight = Main.GetInstance().GetClockHeight();
        private static readonly int ClockWidth = Main.GetInstance().GetClockWidth();

        private static readonly Point Center = new Point(ClockWidth / 2 - (LineWidth/2), ClockHeight/2 - (LineWidth/2));

        private static Pen myPen = new Pen(color, LineWidth);

        private static Pen HandPen = new Pen(Color.FromArgb(200,255,255,255), 5);

        private static Font ClockFont = new Font("Segoe UI", 15, FontStyle.Bold);

        static Clock()
        {
        }

        public static Bitmap PaintClock()
        {
            Bitmap bitmap = new Bitmap(ClockWidth, ClockHeight, PixelFormat.Format32bppPArgb);

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.Save();
            double[] time = UpdateHands();


            DrawBackground(g);

            InitializeClock(g);
            InitializeMovingComponents(g, time[0], time[1], time[2]);




            return bitmap;
        }
        public static Bitmap PaintMovingClock()
        {
            Bitmap bitmap = new Bitmap(ClockWidth, ClockHeight, PixelFormat.Format32bppPArgb);

            Graphics g = Graphics.FromImage(bitmap);
            g.Save();

            return bitmap;
        }

        private static void InitializeClock(Graphics g)
        {
            DrawBorder(g);
            DrawHourMarkers(g);
            DrawClockNumbers(g);
            DrawCenter(g);
        }
        private static void InitializeMovingComponents(Graphics g, double HourValue, double MinuteValue, double SecondValue)
        {
            DrawHourHand(g, HourValue);
            DrawMinuteHand(g, MinuteValue);
            DrawSecondHand(g, SecondValue);
        }

        #region Drawing
        private static void DrawBorder(Graphics g)
        {
            g.DrawEllipse(myPen, new Rectangle(LineWidth / 2, LineWidth / 2, ClockWidth - LineWidth, ClockWidth - LineWidth));
        }

        private static void DrawHourMarkers(Graphics g)
        {
            // Save the original state of the graphics object
            var originalState = g.Save();

            // Translate the origin to the center of the clock
            g.TranslateTransform(ClockWidth / 2, ClockHeight / 2);

            // Set the number of hour markers and angle per marker
            int totalMarkers = 12;
            float rotationAngle = 360f / totalMarkers;

            for (int i = 0; i < totalMarkers; i++)
            {
                // Draw major markers (12, 3, 6, 9)
                if (i % 3 == 0)
                {
                    g.DrawLine(myPen,
                        new Point(0, -ClockHeight / 2 + LineWidth),
                        new Point(0, -ClockHeight / 2 + LineWidth + 10));
                }
                else
                {
                    // Draw minor markers
                    g.DrawLine(myPen,
                        new Point(0, -ClockHeight / 2 + LineWidth),
                        new Point(0, -ClockHeight / 2 + LineWidth + 5));
                }

                // Rotate the graphics context by 30 degrees
                g.RotateTransform(rotationAngle);
            }

            // Restore the original graphics state
            g.Restore(originalState);
        }


        private static void DrawClockNumbers(Graphics g)
        {
            // Calculate the radius for placing the numbers
            float radius = Math.Min(ClockWidth, ClockHeight) / 2 - LineWidth - 20;

            // Set the number of hours (12-hour clock)
            int totalHours = 12;

            for (int i = 0; i < totalHours; i++)
            {
                // Calculate the angle for each hour marker in radians
                // Adjusting by -90 degrees to start from the top
                double angle = (i * (360.0 / totalHours) - 90) * Math.PI / 180.0;

                // Calculate the position of each number based on angle and radius
                float x = (ClockWidth / 2) + (float)(radius * Math.Cos(angle));
                float y = (ClockHeight / 2) + (float)(radius * Math.Sin(angle));

                // Draw the number, offset slightly to center it
                string number = (i == 0) ? "12" : i.ToString();  // Start at "12" at the top
                SizeF numberSize = g.MeasureString(number, ClockFont);
                g.DrawString(number, ClockFont, new SolidBrush(Color.White), x - numberSize.Width / 2, y - numberSize.Height / 2);
            }
        }

        private static void DrawHourHand(Graphics g, double HourValue)
        {
            // Save the original state of the graphics object
            var originalState = g.Save();

            // Calculate the angle for the hour hand based on the hour value
            // Each hour is 30 degrees, so multiply by 30. Subtract 90 to start from the top.
            double angle = (HourValue * 30f);

            // Move the origin to the center of the clock for rotation
            g.TranslateTransform(Center.X, Center.Y);

            // Rotate the graphics context by the calculated angle
            g.RotateTransform((float)angle);

            // Draw the hour hand
            //g.DrawLine(HandPen, 0, 0, 0, -LineWidth - 100);

            // Define the hand shape as a polygon
            PointF[] handPoints = {
                new PointF(-3, 0),                  // Left side at base
                new PointF(3, 0),                   // Right side at base
                new PointF(2, -LineWidth),     // Right side at tip
                new PointF(-2, -LineWidth - 100)     // Left side at tip
            };

            PointF[] glowPoints = {
                new PointF(-7, 0),                  // Left side at base (wider)
                new PointF(7, 0),                   // Right side at base (wider)
                new PointF(4, - LineWidth- 5), // Right side at tip (longer)
                new PointF(-4, -LineWidth - 105) // Left side at tip (longer)
            };

            // Draw the glow with a semi-transparent brush
            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(60, 255, 255, 255))) // Soft white glow
            {
                g.FillPolygon(glowBrush, glowPoints);
            }

            // Create a semi-transparent brush
            using (LinearGradientBrush brush = new LinearGradientBrush(
            new PointF(0, 0),
            new PointF(0, -LineWidth - 100),
            Color.FromArgb(200, 255, 255, 255), // Start color
            Color.FromArgb(50, 255, 255, 255))) // End color
            {
                g.FillPolygon(brush, handPoints);
            }

            // Restore the original graphics state
            g.Restore(originalState);

        }

        private static void DrawMinuteHand(Graphics g, double MinuteValue)
        { 

            // Save the original state of the graphics object
            var originalState = g.Save();

            // Calculate the angle for the hour hand based on the hour value
            // Each hour is 30 degrees, so multiply by 30. Subtract 90 to start from the top.
            double angle = (MinuteValue * 30f);

            // Move the origin to the center of the clock for rotation
            g.TranslateTransform(Center.X, Center.Y);

            // Rotate the graphics context by the calculated angle
            g.RotateTransform((float)angle);

            // Draw the hour hand
            //g.DrawLine(HandPen, 0, 0, 0, -LineWidth - 150);

            // Define the hand shape as a polygon
            PointF[] handPoints = {
                new PointF(-3, 0),                  // Left side at base
                new PointF(3, 0),                   // Right side at base
                new PointF(2, -LineWidth),     // Right side at tip
                new PointF(-2, -LineWidth - 150)     // Left side at tip
            };

            // Create a semi-transparent brush
            using (LinearGradientBrush brush = new LinearGradientBrush(
            new PointF(0, 0),
            new PointF(0, -LineWidth - 150),
            Color.FromArgb(200, 255, 255, 255), // Start color
            Color.FromArgb(50, 255, 255, 255))) // End color
            {
                g.FillPolygon(brush, handPoints);
            }

            // Restore the original graphics state
            g.Restore(originalState);
        }

        private static void DrawSecondHand(Graphics g, double SecondValue)
        {

            // Save the original state of the graphics object
            var originalState = g.Save();

            // Calculate the angle for the hour hand based on the hour value
            // Each hour is 30 degrees, so multiply by 30. Subtract 90 to start from the top.
            double angle = (SecondValue * 30f);

            // Move the origin to the center of the clock for rotation
            g.TranslateTransform(Center.X, Center.Y);

            // Rotate the graphics context by the calculated angle
            g.RotateTransform((float)angle);

            // Draw the hour hand
            g.DrawLine(new Pen(new SolidBrush(Color.Red)), 0, 0, 0, -LineWidth - 100);

            // Restore the original graphics state
            g.Restore(originalState);
        }
        private static void DrawCenter(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(200,255,255,255)), new Rectangle(Center.X-2, Center.Y-2, LineWidth, LineWidth));
            
        }

        private static void DrawBackground(Graphics g)
        {
            //Get Image from files

            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Resources", "background.png");
            BackgroundImage = Image.FromFile(imagePath);


            int centerX = ClockWidth / 2;
            int centerY = ClockHeight / 2;
            int clockRadius = Math.Min(ClockWidth, ClockHeight) / 2 - LineWidth;

            using (GraphicsPath path = new GraphicsPath())
            {
                // Define the circular region for the clock face
                path.AddEllipse(centerX - clockRadius, centerY - clockRadius, clockRadius * 2, clockRadius * 2);

                // Set the clipping region to restrict drawing to the circle
                g.SetClip(path);

                // Draw the background image inside the circle
                //g.DrawImage(BackgroundImage, new Rectangle(0, 0, ClockWidth, ClockHeight));
                g.DrawImage(BackgroundImage, new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height));

                // Reset the clipping region for the outer area
                g.ResetClip();
            }

            // Draw the outer area with the form's background color
            using (GraphicsPath outerPath = new GraphicsPath())
            {
                outerPath.AddEllipse(centerX - clockRadius, centerY - clockRadius, clockRadius * 2, clockRadius * 2);
                Region outerRegion = new Region(new Rectangle(0, 0, ClockWidth, ClockHeight));

                // Exclude the clock face circle from the region
                outerRegion.Exclude(outerPath);

                // Fill the excluded region (outside the circle) with the form's background color
                g.FillRegion(new SolidBrush(Main.BackgroundColor), outerRegion);
            }
        }

        #endregion
        private static double[] UpdateHands()
        {
            int DaysInThisMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            double HoursRemainingThisMonth = GetHoursPassedInMonth();

            double HourValue = HoursRemainingThisMonth / DaysInThisMonth;

            double MinuteValue = (HourValue % 1) * 12;

            double SecondValue = (MinuteValue % 1) * 12;

            Debug.WriteLine($"Hour: {HourValue}\nMinute: {MinuteValue}\nSecond: {SecondValue}");

            if (HourValue > 12) IsAM = false;

            return new double[] { HourValue, MinuteValue, SecondValue };
        }

        public static double GetHoursPassedInMonth()
        {
            // Get the current date and time
            DateTime Now = DateTime.Now;

            // Find the last day of the current month at 23:59:59
            DateTime EndOfMonth = new DateTime(Now.Year, Now.Month, DateTime.DaysInMonth(Now.Year, Now.Month), 23, 59, 59);

            // Calculate the difference in hours
            TimeSpan TimeRemaining = EndOfMonth - Now;
            double HoursRemaining = TimeRemaining.TotalHours;

            int HoursInMonth = DateTime.DaysInMonth(Now.Year, Now.Month) * 24;

            return HoursInMonth - HoursRemaining;
        }

    }
}
