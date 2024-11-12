using System.Windows.Forms;

namespace NovemberClock
{
    partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Clock = new NovemberClock.Components.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // Clock
            // 
            this.Clock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Clock.Location = new System.Drawing.Point(0, 0);
            this.Clock.Name = "Clock";
            this.Clock.Size = new System.Drawing.Size(400, 400);
            this.Clock.TabIndex = 0;
            this.Clock.Paint += new System.Windows.Forms.PaintEventHandler(this.Clock_Paint);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(402, 403);
            this.Controls.Add(this.Clock);
            this.Name = "Main";
            this.Text = "November Clock";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Components.DoubleBufferedPanel Clock;
    }
}

