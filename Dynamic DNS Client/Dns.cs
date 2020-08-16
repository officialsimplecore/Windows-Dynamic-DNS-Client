using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Dynamic_DNS_Client
{
    class Dns
    {
        public Dns()
        {
            // Initializes Last IP
            lastIp = GetExternalIP();
            Console.WriteLine("Initializing IP: " + lastIp);

            // Create Authentication Header
            client.DefaultRequestHeaders.Add($"Authorization", $"Basic {Base64Encode($"wZSSX0NygCGtQRIm:3ONaSGNdxkOkralg")}");

            // Runs DNS checker every 30 minutes
            Timer timer = new Timer(DnsChecker, null, 1800000, 1800000);
        }
        static private void DnsChecker(Object state)
        {
            if (lastIp != GetExternalIP())
            {
                UpdateDNS();
                lastIp = GetExternalIP();
            }
            else
            {
                Console.WriteLine("IP is same " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt"));
            }
        }

        static async private void UpdateDNS()
        {
            var result = await client.PostAsync("https://domains.google.com/nic/update?hostname=insulinrxcalculator.com&myip=" + GetExternalIP(), null);
            Console.WriteLine(result.StatusCode);
        }

        static public string GetExternalIP()
        {
            String externalAddress = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                externalAddress = stream.ReadToEnd();
            }
            int first = externalAddress.IndexOf("Address: ") + 9;
            int last = externalAddress.LastIndexOf("</body>");
            externalAddress = externalAddress.Substring(first, last - first);
            return externalAddress;
        }

        public static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }

        private static readonly HttpClient client = new HttpClient();
        private static string lastIp;
    }
}
