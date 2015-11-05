using System;
using System.Linq;
using Tmds.MDns;
using VirbControl.Data;

namespace VirbControl.Connection
{
    public class Network
    {
        private string _serviceType = "_garmin-virb._tcp";
        public string ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }

        private Services _serviceList = new Services();
        public Services ServiceList
        {
            get { return _serviceList; }
            set { _serviceList = value; }
        }

        private readonly ServiceBrowser _serviceBrowser;

        public Network()
        {
            _serviceBrowser = new ServiceBrowser();
            _serviceBrowser.ServiceAdded += OnServiceAdded;
            _serviceBrowser.ServiceRemoved += OnServiceRemoved;
            _serviceBrowser.ServiceChanged += OnServiceChanged;
        }

        public void StartSearch()
        {
            _serviceList.Clear();
            Console.WriteLine("Browsing for type: {0}", ServiceType);
            _serviceBrowser.StartBrowse(ServiceType);
        }

        public void StartSearch(string serviceType)
        {
            _serviceType = serviceType;
            Console.WriteLine("Browsing for type: {0}", ServiceType);
            _serviceBrowser.StartBrowse(ServiceType);
        }

        void OnServiceChanged(object sender, ServiceAnnouncementEventArgs e)
        {
            var item = _serviceList.FirstOrDefault(i => i.Hostname == e.Announcement.Hostname);
            if (item != null)
            {
                item = e.Announcement;
            }
            printService('~', e.Announcement);
        }

        void OnServiceRemoved(object sender, ServiceAnnouncementEventArgs e)
        {
            if (_serviceList.Contains(e.Announcement))
                _serviceList.Remove(e.Announcement);
            printService('-', e.Announcement);
        }

        void OnServiceAdded(object sender, ServiceAnnouncementEventArgs e)
        {
            if (!_serviceList.Contains(e.Announcement))
                _serviceList.Add(e.Announcement);
            printService('+', e.Announcement);
        }

        void printService(char startChar, ServiceAnnouncement service)
        {
            Console.WriteLine("{0} '{1}' on {2}", startChar, service.Instance, service.NetworkInterface.Name);
            foreach (var ipAddress in service.Addresses)
            {
                Console.WriteLine("\tHost: {0} ({1})", service.Hostname, ipAddress);
            }
            //Console.WriteLine("\tHost: {0} ({1})", service.Hostname, string.Join(", ", service.Addresses));
            Console.WriteLine("\tPort: {0}", service.Port);
            Console.WriteLine("\tTxt : [{0}]", service.Txt);
        }

        public void SendJson(ServiceAnnouncement service, string json)
        {
            
        }
    }
}
