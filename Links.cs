using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LinkStreamInVLC
{
    public partial class LinkListForm : Form
    {
        private List<SavedLink> _links = new List<SavedLink>();
        string rawJSON = "[]";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SavedLinkStream.VLC");
        string selectedLink;

        public LinkListForm()
        {
            InitializeComponent();

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
        }

        private void LinkListForm_Load(object sender, EventArgs e)
        {
            var bindingList = new BindingList<SavedLink>(_links);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;
            dataGridView1.AutoGenerateColumns = false;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
                string json = JsonConvert.SerializeObject(_links);
                File.WriteAllText(filePath, json);
            }
            if (e.ColumnIndex == dataGridView1.Columns["Select"].Index && e.RowIndex >= 0)
            {
                selectedLink = dataGridView1.Rows[e.RowIndex].Cells["link"].Value.ToString();
                this.Hide();
            }
        }

        public string getSelectedLink()
        {
            return selectedLink; 
        }
    }
}
