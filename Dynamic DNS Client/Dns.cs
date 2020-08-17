using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        }
        static public void RunDnsListen(string _domain, string user, string pass, int _interval)
        {
            // Set instance variables to parameters
            interval = _interval;
            domain = _domain;
            Console.WriteLine($"User: {user} Pass: {pass} Domain: {domain}");
            // Create Authentication Header
            client.DefaultRequestHeaders.Add($"Authorization", $"Basic {Base64Encode($"{user}:{pass}")}");

            // Start DNSChecker through timer
            SetTimer(true);
        }

        static public void StopDnsListen()
        {
            client.DefaultRequestHeaders.Remove($"Authorization");
            SetTimer(false);
        }

        static private void SetTimer(bool setting)
        {
            
            if (setting == true)
            {
                // Runs DNS checker on ms interval
                _timer.Change(interval, interval);
            } else
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        static private void DnsChecker(Object state)
        {
            // Checks if there is a change
            if (lastIp != GetExternalIP())
            {
                // Sends an update record when there is a change
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
            var result = await client.PostAsync("https://domains.google.com/nic/update?hostname=" + domain + "&myip=" + GetExternalIP(), null);
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

        private static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }

        private static readonly HttpClient client = new HttpClient();
        private static string lastIp;

        private static string domain;
        private static int interval;

        private static Timer _timer = new Timer(DnsChecker, null, Timeout.Infinite, Timeout.Infinite);
    }
}
