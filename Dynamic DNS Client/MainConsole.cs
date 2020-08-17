using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
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
            domain = ConfigurationManager.AppSettings["domain"];
            user = ConfigurationManager.AppSettings["user"];
            pass = ConfigurationManager.AppSettings["pass"];
            interval = ConfigurationManager.AppSettings["interval"];

            if (ConfigurationManager.AppSettings["autostart"] == "true")
            {
                label12.Text = "Enabled";
            } else
            {
                label12.Text = "Disabled";
            }

            // Initialize external IP label
            label7.Text = Dns.GetExternalIP();

            if (ConfigurationManager.AppSettings["interval"] == "true" && domain != "" && user != "" && pass != "")
            {
                textBox1.Text = domain;
                textBox2.Text = user;
                textBox3.Text = pass;
                textBox4.Text = interval;
                Dns.RunDnsListen(domain, user, pass, 1800000);
                SetRunningUI();
            } else
            {
                SetStoppingUI();
            }
        }

        // Called when the button is clicked
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                // Sets config values
                domain = config.AppSettings.Settings["domain"].Value = textBox1.Text;
                user = config.AppSettings.Settings["user"].Value = textBox2.Text;
                pass = config.AppSettings.Settings["pass"].Value = textBox3.Text;
                interval = textBox4.Text;
                double _interval = Double.Parse(interval);
                // Runs listener with interval and converts interval to milliseconds
                Dns.RunDnsListen(domain, user, pass, Convert.ToInt32(_interval * 60000));

                // Prevents re-setting of Authentication Header with running UI
                SetRunningUI();
            } else
            {
                MessageBox.Show("Please fill out all fields");
            }
        }

        private void SetRunningUI()
        {
            Console.WriteLine("Running");
            button1.Enabled = false;
            button2.Enabled = true;
            label5.Text = "Running";
            label5.ForeColor = System.Drawing.Color.Green;
        }

        private void SetStoppingUI()
        {
            Console.WriteLine("Stopped");
            button1.Enabled = true;
            button2.Enabled = false;
            label5.Text = "Stopped";
            label5.ForeColor = System.Drawing.Color.Red;
            Dns.StopDnsListen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetStoppingUI();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label12.Text = "Enabled";
            config.AppSettings.Settings["autostart"].Value = "true";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label12.Text = "Disabled";
            config.AppSettings.Settings["autostart"].Value = "false";

        }

        private static string user;
        private static string pass;
        private static string domain;
        private static string interval;
        private static Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
    }
}
