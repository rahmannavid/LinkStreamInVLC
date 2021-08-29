using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetTempPath() + "streaming.strm";

            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please insert the link first", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                TextWriter tw = new StreamWriter(filePath);
                tw.Write(textBox1.Text);
                tw.Close();
            }
            else if (File.Exists(filePath))
            {
                TextWriter tw = new StreamWriter(filePath);
                tw.Write(textBox1.Text);
                tw.Close();
            }

            try
            {
                Process.Start("vlc.exe", filePath);
            }
            catch
            {
                MessageBox.Show("Install VLC Player.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            MessageBox.Show("About: This application helps you to stream any video from download link without downloading the file.\n\n"
                                + "How to Use: First copy a downloadable link of any video file and paste it into the Link Textbox."
                                + "Then click Open in VLC button, that's it! VLC player will start streaming your video file.\n\n"
                                + "N.B.: \n1. VLC player need to be installed in your computer.\n"
                                + "2. File name must be included in the link.\n\n"
                                + "Enjoy ... ツ - Navid\n\n"
                                + "A Sample video ink has been pasted into the Link textbox now you can open it in VLC player.\n\n"
                                + "**This app is completely free. Feel free to share with others.", "Help");

            Clipboard.SetText("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
            textBox1.Text = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.button5, "Paste From Clipboard");

            string filePath = Path.GetTempPath() + "streaming.strm";
            if (File.Exists(filePath))
            {
                StreamReader tr = new StreamReader(filePath);
                textBox1.Text = tr.ReadLine();
                tr.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
        }
    }
}
