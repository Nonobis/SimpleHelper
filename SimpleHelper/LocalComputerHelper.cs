using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Principal;

/// <summary>
/// The Helper namespace.
/// </summary>
namespace SimpleHelper
{
    /// <summary>
    /// Class LocalComputerHelper.
    /// </summary>
    public static class LocalComputerHelper
    {

        /// <summary>
        /// Check if a software is installed
        /// </summary>
        /// <param name="pSoftwareName">Name of the p software.</param>
        /// <param name="pOnlyDisplayedSoftware">if set to <c>true</c> [p only displayed software].</param>
        /// <param name="pIgnoreCase">if set to <c>true</c> [p ignore case].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckIfSoftwareIsInstalled(string pSoftwareName, bool pOnlyDisplayedSoftware = false, bool pIgnoreCase = true)
        {
            return GetInstalledSoftwares(pOnlyDisplayedSoftware).Any(p => string.Compare(p, pSoftwareName, pIgnoreCase) == 0);

        }

        /// <summary>
        /// Retrieving System Account Name.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetAccountName()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");
            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("Name").ToString();
                }
                catch
                {
                    // ignored
                }
            }
            return "User Account Name: Unknown";

        }

        /// <summary>
        /// Retrieving BIOS Caption.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetBioScaption()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("Caption").ToString();

                }
                catch
                {
                    // ignored
                }
            }
            return "BIOS Caption: Unknown";
        }

        /// <summary>
        /// Retrieving BIOS Maker.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetBioSmaker()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();

                }
                catch
                {
                    // ignored
                }
            }

            return "BIOS Maker: Unknown";

        }

        /// <summary>
        /// Retrieving BIOS Serial No.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetBioSserNo()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("SerialNumber").ToString();
                }
                catch
                {
                    // ignored
                }
            }

            return "BIOS Serial Number: Unknown";

        }

        /// <summary>
        /// Retrieving Motherboard Manufacturer.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetBoardMaker()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();
                }
                catch
                {
                    // ignored
                }
            }

            return "Board Maker: Unknown";

        }

        /// <summary>
        /// Retrieving Motherboard Product Id.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetBoardProductId()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("Product").ToString();

                }
                catch
                {
                    // ignored
                }
            }

            return "Product: Unknown";

        }

        /// <summary>
        /// Retrieving CD-DVD Drive Path.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetCdRomDrive()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("Drive").ToString();

                }
                catch
                {
                    // ignored
                }
            }

            return "CD ROM Drive Letter: Unknown";

        }

        /// <summary>
        /// Retrieving Computer Name.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetComputerName()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            string info = string.Empty;
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                info = (string)mo["Name"];
            }
            return info;
        }

        /// <summary>
        /// method to retrieve the CPU's current
        /// clock speed using the WMI class
        /// </summary>
        /// <returns>Clock speed</returns>
        public static int GetCpuCurrentClockSpeed()
        {
            var cpuClockSpeed = 0;

            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");

            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();

            //start our loop for all processors found
            foreach (var o in objCol)
            {
                var obj = (ManagementObject)o;
                if (cpuClockSpeed == 0)
                {
                    // only return cpuStatus from first CPU
                    cpuClockSpeed = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
                }
            }
            //return the status
            return cpuClockSpeed;
        }

        //Get CPU Temprature.
        /// <summary>
        /// method for retrieving the CPU Manufacturer
        /// using the WMI class
        /// </summary>
        /// <returns>CPU Manufacturer</returns>
        public static string GetCpuManufacturer()
        {
            string cpuMan = string.Empty;

            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");

            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();

            //start our loop for all processors found
            foreach (var o in objCol)
            {
                var obj = (ManagementObject)o;
                if (cpuMan == string.Empty)
                {
                    // only return manufacturer from first CPU
                    cpuMan = obj.Properties["Manufacturer"].Value.ToString();
                }
            }
            return cpuMan;
        }

        /// <summary>
        /// Retrieve CPU Speed.
        /// </summary>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? GetCpuSpeedInGHz()
        {
            double? gHz = null;
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                foreach (var o in mc.GetInstances())
                {
                    var mo = (ManagementObject)o;
                    gHz = 0.001 * (uint)mo.Properties["CurrentClockSpeed"].Value;
                    break;
                }
            }
            return gHz;
        }

        /// <summary>
        /// Retrieving Current Language
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetCurrentLanguage()
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return wmi.GetPropertyValue("CurrentLanguage").ToString();

                }
                catch
                {
                    // ignored
                }
            }
            return "BIOS Maker: Unknown";

        }

        /// <summary>
        /// method to retrieve the network adapters
        /// default IP gateway using WMI
        /// </summary>
        /// <returns>adapters default IP gateway</returns>
        public static string GetDefaultIpGateway()
        {
            //create out management class object using the
            //Win32_NetworkAdapterConfiguration class to get the attributes
            //of the network adapter
            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");

            //create our ManagementObjectCollection to get the attributes with
            ManagementObjectCollection objCol = mgmt.GetInstances();
            string gateway = string.Empty;

            //loop through all the objects we find
            foreach (var o in objCol)
            {
                var obj = (ManagementObject)o;
                if (gateway == string.Empty)  // only return MAC Address from first card
                {
                    //grab the value from the first network adapter we find
                    //you can change the string to an array and get all
                    //network adapters found as well
                    //check to see if the adapter's IPEnabled
                    //equals true
                    if ((bool)obj["IPEnabled"])
                    {
                        gateway = obj["DefaultIPGateway"].ToString();
                    }
                }
                //dispose of our object
                obj.Dispose();
            }

            //replace the ":" with an empty space, this could also
            //be removed if you wish
            gateway = gateway.Replace(":", "");

            //return the mac address
            return gateway;
        }

        /// <summary>
        /// Retrieving HDD Serial No.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetHddSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (var o in mcol)
            {
                var strt = (ManagementObject)o;
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }

        /// <summary>
        /// Return installed Software
        /// </summary>
        /// <param name="pOnlyDisplayedSoftware">if set to <c>true</c> [p only displayed software].</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetInstalledSoftwares(bool pOnlyDisplayedSoftware)
        {
            List<string> lstSoftwares = new List<string>();

            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                    {
                        if (pOnlyDisplayedSoftware && IsProgramVisible(subkey))
                        {
                            if (subkey != null)
                                lstSoftwares.Add(subkey.GetValue("DisplayName").ToString());
                        }
                        else
                        {
                            if (subkey != null)
                                lstSoftwares.Add(subkey.GetValue("DisplayName").ToString());
                        }
                    }
                    key.Close();
                }
            }

            if (Environment.Is64BitOperatingSystem)
            {
                registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
                {
                    if (key != null)
                    {
                        foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                        {
                            if (subkey != null)
                                lstSoftwares.Add(subkey.GetValue("DisplayName").ToString());
                        }
                        key.Close();
                    }
                }
            }

            if (lstSoftwares.Any())
            {
                lstSoftwares = lstSoftwares.Distinct().ToList();
            }
            return lstSoftwares.Distinct().ToList();
        }

        /// <summary>
        /// Gets the name of the localized administrators groups account.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetLocalizedAdministratorsGroupsAccountName()
        {
            string adminsSid = new SecurityIdentifier("S-1-5-32-544").ToString();
            string localizedAdmin = new SecurityIdentifier(adminsSid).Translate(typeof(NTAccount)).ToString();
            localizedAdmin = localizedAdmin.Replace(@"BUILTIN\", "");
            return localizedAdmin;
        }

        /// <summary>
        /// Gets the localized everyone account.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetLocalizedEveryoneAccount()
        {
            string sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null).ToString();
            string localizedsid = new SecurityIdentifier(sid).Translate(typeof(NTAccount)).ToString();
            return localizedsid.Replace(@"BUILTIN\", "").Replace("AUTORITE NT", "");

        }

        /// <summary>
        /// Gets the name of the localized system admin  account.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetLocalizedSystemAccountName()
        {
            string sid = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null).ToString();
            string localizedsid = new SecurityIdentifier(sid).Translate(typeof(NTAccount)).ToString();
            return localizedsid.Replace(@"BUILTIN\", "").Replace("AUTORITE NT", "");
        }

        /// <summary>
        /// Retrieving System MAC Address.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string macAddress = string.Empty;
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                if (macAddress == string.Empty)
                {
                    if ((bool)mo["IPEnabled"]) macAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }

            macAddress = macAddress.Replace(":", "");
            return macAddress;
        }

        /// <summary>
        /// Retrieving No of Ram Slot on Motherboard.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetNoRamSlots()
        {

            int memSlots = 0;
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
            ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
            ManagementObjectCollection oCollection2 = oSearcher2.Get();
            foreach (var o in oCollection2)
            {
                var obj = (ManagementObject)o;
                memSlots = Convert.ToInt32(obj["MemoryDevices"]);
            }
            return memSlots.ToString();
        }

        /// <summary>
        /// Retrieving Current Language.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetOsInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (var o in searcher.Get())
            {
                var wmi = (ManagementObject)o;
                try
                {
                    return ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["Version"] + ", " + (string)wmi["OSArchitecture"];
                }
                catch
                {
                    // ignored
                }
            }
            return "BIOS Maker: Unknown";
        }

        /// <summary>
        /// Retrieving Physical Ram Memory.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetPhysicalMemory()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            long memSize = 0;

            // In case more than one Memory sticks are installed
            foreach (var o in oCollection)
            {
                var obj = (ManagementObject)o;
                var mCap = Convert.ToInt64(obj["Capacity"]);
                memSize += mCap;
            }
            memSize = (memSize / 1024) / 1024;
            return memSize + "MB";
        }

        /// <summary>
        /// Gets the physical processor.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static int GetPhysicalProcessor()
        {
            return new ManagementObjectSearcher("Select NumberOfCores from Win32_Processor").Get().Cast<ManagementBaseObject>().Sum(item => Convert.ToInt32(item.GetPropertyValue("NumberOfCores")));
        }
        /// <summary>
        /// Retrieving Processor Id.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetProcessorId()
        {

            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string id = string.Empty;
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;

                id = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return id;

        }

        /// <summary>
        /// Retrieving Processor Information.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetProcessorInformation()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string info = string.Empty;
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                var name = (string)mo["Name"];
                name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");
                info = name + ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"];
            }
            return info;
        }

        /// <summary>
        /// Get local computer Windows License Key
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetWindowsProductKey()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix
                || Environment.OSVersion.Platform == PlatformID.MacOSX
                || Environment.OSVersion.Platform == PlatformID.WinCE
                || Environment.OSVersion.Platform == PlatformID.Xbox)
                return string.Empty;

            RegistryKey localKey;
            if (Environment.Is64BitOperatingSystem)
            {
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            }
            else
            {
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            }
            var value = (byte[])localKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion")?.GetValue("DigitalProductId");
            var digitalProductId = value;
            var isWin8OrUp =
                (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2)
                ||
                (Environment.OSVersion.Version.Major > 6);
            var productKey = isWin8OrUp ? DecodeProductKeyWin8AndUp(digitalProductId) : DecodeProductKey(digitalProductId);
            localKey.Close();
            return productKey;

        }
        /// <summary>
        /// Decodes the product key.
        /// </summary>
        /// <param name="digitalProductId">The digital product identifier.</param>
        /// <returns>System.String.</returns>
        private static string DecodeProductKey(byte[] digitalProductId)
        {
            const int keyStartIndex = 52;
            const int keyEndIndex = keyStartIndex + 15;
            var digits = new[]
            {
                'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'M', 'P', 'Q', 'R',
                'T', 'V', 'W', 'X', 'Y', '2', '3', '4', '6', '7', '8', '9',
            };
            const int decodeLength = 29;
            const int decodeStringLength = 15;
            var decodedChars = new char[decodeLength];
            var hexPid = new ArrayList();
            for (var i = keyStartIndex; i <= keyEndIndex; i++)
            {
                hexPid.Add(digitalProductId[i]);
            }

            for (var i = decodeLength - 1; i >= 0; i--)
            {
                // Every sixth char is a separator.
                if ((i + 1) % 6 == 0)
                {
                    decodedChars[i] = '-';
                }
                else
                {
                    // Do the actual decoding.
                    var digitMapIndex = 0;
                    for (var j = decodeStringLength - 1; j >= 0; j--)
                    {
                        var byteValue = (digitMapIndex << 8) | (byte)hexPid[j];
                        hexPid[j] = (byte)(byteValue / 24);
                        digitMapIndex = byteValue % 24;
                        decodedChars[i] = digits[digitMapIndex];
                    }
                }
            }
            return new string(decodedChars);
        }

        /// <summary>
        /// Decodes the product key win8 and up.
        /// </summary>
        /// <param name="digitalProductId">The digital product identifier.</param>
        /// <returns>System.String.</returns>
        private static string DecodeProductKeyWin8AndUp(byte[] digitalProductId)
        {
            var key = string.Empty;
            const int keyOffset = 52;
            var isWin8 = (byte)((digitalProductId[66] / 6) & 1);
            digitalProductId[66] = (byte)((digitalProductId[66] & 0xf7) | (isWin8 & 2) * 4);

            const string digits = "BCDFGHJKMPQRTVWXY2346789";
            int last = 0;
            for (var i = 24; i >= 0; i--)
            {
                var current = 0;
                for (var j = 14; j >= 0; j--)
                {
                    current = current * 256;
                    current = digitalProductId[j + keyOffset] + current;
                    digitalProductId[j + keyOffset] = (byte)(current / 24);
                    current = current % 24;
                    last = current;
                }
                key = digits[current] + key;
            }

            var keypart1 = key.Substring(1, last);
            var keypart2 = key.Substring(last + 1, key.Length - (last + 1));
            key = keypart1 + "N" + keypart2;

            for (var i = 5; i < key.Length; i += 6)
            {
                key = key.Insert(i, "-");
            }

            return key;
        }

        /// <summary>
        /// Check if a programe is visible in AddRemoveSoftware
        /// </summary>
        /// <param name="subkey">The subkey.</param>
        /// <returns><c>true</c> if [is program visible] [the specified subkey]; otherwise, <c>false</c>.</returns>
        private static bool IsProgramVisible(RegistryKey subkey)
        {
            var name = (string)subkey.GetValue("DisplayName");
            var releaseType = (string)subkey.GetValue("ReleaseType");
            //var unistallString = (string)subkey.GetValue("UninstallString");
            var systemComponent = subkey.GetValue("SystemComponent");
            var parentName = (string)subkey.GetValue("ParentDisplayName");

            return
                !string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(releaseType)
                && string.IsNullOrEmpty(parentName)
                && (systemComponent == null);
        }
    }
}
