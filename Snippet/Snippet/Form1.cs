using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Snippet
{
    public partial class Form1 : Form
    {
        Snippets snippets;

        public Form1()
        {
            InitializeComponent();
            snippets = new Snippets();
            Get();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            snippets.AddSnippet(new KeyValuePair<string, string>(txtName.Text,rtbSnip.Text));
            txtName.Clear();
            rtbSnip.Clear();
            Get();
        }

        private void Get()
        {
            lstSnip.Items.Clear();
            foreach (var snip in snippets.snippets.OrderBy(x => x.Key))
            {
                lstSnip.Items.Add(snip.Key);
            }
        }

        private void lstSnip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSnip.SelectedIndex == -1) return;
            rtbSnip.Text = snippets.snippets[lstSnip.Items[lstSnip.SelectedIndex].ToString()] ?? "";
            var cursnip = string.IsNullOrEmpty(snippets.snippets[lstSnip.Items[lstSnip.SelectedIndex].ToString()]) ? "File not found" : snippets.snippets[lstSnip.Items[lstSnip.SelectedIndex].ToString()];
            Clipboard.SetText(cursnip);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstSnip.SelectedIndex == -1) return;
            snippets.DeleteFile(lstSnip.Items[lstSnip.SelectedIndex].ToString());
            Get();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
