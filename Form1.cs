using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LinkStreamInVLC
{
    public partial class Form1 : Form
    {

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private List<SavedLink> links = new List<SavedLink>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetTempPath() + "LinkStreamVLC.strm";

            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please insert the link first", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _saveLink(filePath);

            try
            {
                Process.Start("vlc.exe", filePath);
            }
            catch
            {
                MessageBox.Show("Install VLC Player.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _saveLink(string _filePath)
        {
            SavedLink _link = new SavedLink();

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                StreamWriter sw = new StreamWriter(_filePath);
                sw.Write(textBox1.Text);
                sw.Close();
            }
            else if (File.Exists(_filePath))
            {
                TextWriter tw = new StreamWriter(_filePath);
                tw.Write(textBox1.Text);
                tw.Close();
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
                                + "A Sample video ink has been pasted into the Link textbox. Now you can open it in VLC player.\n\n"
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

            string filePath = Path.GetTempPath() + "LinkStreamVLC.strm";
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (panel1.Height == 26)
            {
                panel1.Height = 80;
            }
            else
            {
                panel1.Height = 26;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SavedLink _link = new SavedLink();
            string rawJSON = "[]";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SavedLinkStream.VLC");
            List<SavedLink> _links = new List<SavedLink>();

            if (File.Exists(filePath))
            {
                StreamReader tr = new StreamReader(filePath);
                rawJSON = tr.ReadToEnd();
                tr.Close();
            }
            else
            {
                File.Create(filePath).Close();
                StreamWriter sw = new StreamWriter(filePath);
                sw.Write(rawJSON);
                sw.Close();
            }

            _links = JsonConvert.DeserializeObject<List<SavedLink>>(rawJSON);

            int id;
            try
            {
                id = _links.Max(l => l.id);
            }
            catch
            {
                id = 0;
            }
            

            _link.id = id + 1;
            _link.link = textBox1.Text;
            _link.name = textBox1.Text.Substring(textBox1.Text.LastIndexOf("/")+1);
            _link.name = _link.name.Replace("%20", " ");
            _link.savedAt = DateTime.Now;
            _links.Add(_link);

            string json = JsonConvert.SerializeObject(_links);
            File.WriteAllText(filePath, json);
            panel1.Height = 26;
            MessageBox.Show("Saved Successfully!\n"
                             +"You can open saved link list and select the link to open in vlc later.");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LinkListForm l = new LinkListForm();
            l.ShowDialog();
            if(!String.IsNullOrEmpty(l.getSelectedLink()))
                textBox1.Text = l.getSelectedLink();
            panel1.Height = 26;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            panel1.Height = 26;
        }
    }
}
