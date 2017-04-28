using System;
using System.Net;
using System.Net.NetworkInformation;

namespace SimpleHelper
{
    public static class NetworkHelper
    {
        /// <summary>
        /// Gets the name of the host.
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// Gets the local ip.
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            string ip = null;
            IPAddress[] addressList = Dns.GetHostEntryAsync(Dns.GetHostName()).Result.AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                IPAddress ipAddress = addressList[i];
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip = ipAddress.ToString();
                }
            }
            return ip;
        }

        /// <summary>
        /// Gets the mac address.
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            string str;
            PhysicalAddress mac = null;
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            int num = 0;
            while (num < allNetworkInterfaces.Length)
            {
                NetworkInterface nic = allNetworkInterfaces[num];
                if ((nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet ? false : nic.OperationalStatus == OperationalStatus.Up))
                {
                    mac = nic.GetPhysicalAddress();
                }
                else if ((nic.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 ? false : nic.OperationalStatus == OperationalStatus.Up))
                {
                    mac = nic.GetPhysicalAddress();
                }

                if (mac == null)
                {
                    num++;
                }
                else
                {
                    break;
                }
            }
            if (mac == null)
            {
                str = null;
            }
            else
            {
                str = BitConverter.ToString(mac.GetAddressBytes()).Replace('-', ':');
            }
            return str;
        }

        /// <summary>
        /// Gets the first mac address.
        /// </summary>
        /// <param name="pType">Type of the p.</param>
        /// <returns></returns>
        public static string GetFirstMacAddress(NetworkInterfaceType pType)
        {
            string str;
            PhysicalAddress mac = null;
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            int num = 0;
            while (num < allNetworkInterfaces.Length)
            {
                NetworkInterface nic = allNetworkInterfaces[num];
                if ((nic.NetworkInterfaceType == pType && nic.OperationalStatus == OperationalStatus.Up))
                {
                    mac = nic.GetPhysicalAddress();
                    break;
                }

                if (mac == null)
                {
                    num++;
                }
                else
                {
                    break;
                }
            }
            if (mac == null)
            {
                str = null;
            }
            else
            {
                str = BitConverter.ToString(mac.GetAddressBytes()).Replace('-', ':');
            }
            return str;
        }


        /// <summary>
        /// Determines whether [is connected to internet].
        /// </summary>
        /// <returns><c>true</c> if [is connected to internet]; otherwise, <c>false</c>.</returns>
        public static bool IsConnectedToInternet()
        {
            try
            {
                const string myAddress = "www.google.com";
                var addresslist = Dns.GetHostAddressesAsync(myAddress);
                if (addresslist!= null && addresslist.Result!=null && addresslist.Result[0].ToString().Length > 6)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the public ip.
        /// </summary>
        /// <returns></returns>
        public static string GetPublicIp()
        {
            // TODO : No implemented for now on NetStandard
            /*
            string publicIpAddress;
            string str;
            try
            {
                publicIpAddress = (new WebClient()).DownloadString("http://bot.whatismyipaddress.com");
            }
            catch
            {
                publicIpAddress = (new WebClient()).DownloadString("http://bs.itoo.me/KeepAlive.aspx");
            }
            str = publicIpAddress;
            return str;
            */
            return string.Empty;
        }
    }
}