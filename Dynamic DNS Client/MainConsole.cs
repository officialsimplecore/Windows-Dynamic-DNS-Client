using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dynamic_DNS_Client
{
    public partial class MainConsole : Form
    {
        public MainConsole()
        {
            InitializeComponent();
            Dns test = new Dns();
            label7.Text = Dns.GetExternalIP();

            var name = ConfigurationManager.AppSettings["name"];
            var surname = ConfigurationManager.AppSettings["surname"];

        }

        // Called when the button is clicked
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
