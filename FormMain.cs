using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyIPEasy
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public static IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                // .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                // .Where(a => Array.FindIndex(a.GetAddressBytes(), b => b != 0) >= 0)
                .FirstOrDefault();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            GatewayText.Text = GetDefaultGateway().ToString();
            IPText.Text = GetLocalIPAddress();
        }

        private void IPText_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(IPText.Text);
        }

        private void GatewayText_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(GatewayText.Text);
        }
    }
}
