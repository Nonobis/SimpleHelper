using Microsoft.Win32;
using System;
using System.Collections;
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
        #region Public Methods

        /// <summary>
        /// Gets the physical processor.
        /// </summary>
        /// <returns></returns>
        public static int GetPhysicalProcessor()
        {
            return new ManagementObjectSearcher("Select NumberOfCores from Win32_Processor").Get().Cast<ManagementBaseObject>().Sum(item => Convert.ToInt32(item.GetPropertyValue("NumberOfCores")));
        }

        /// <summary>
        /// Gets the name of the localized administrators groups account.
        /// </summary>
        /// <returns></returns>
        public static string GetLocalizedAdministratorsGroupsAccountName()
        {
            string adminsSID = new SecurityIdentifier("S-1-5-32-544").ToString();
            string localizedAdmin = new SecurityIdentifier(adminsSID).Translate(typeof(NTAccount)).ToString();
            localizedAdmin = localizedAdmin.Replace(@"BUILTIN\", "");
            return localizedAdmin;
        }

        /// <summary>
        /// Gets the name of the localized system admin  account.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetLocalizedSystemAccountName()
        {
            string sid = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null).ToString();
            string localizedsid = new SecurityIdentifier(sid).Translate(typeof(NTAccount)).ToString();
            return localizedsid.Replace(@"BUILTIN\", "").Replace("AUTORITE NT","");
        }

        /// <summary>
        /// Gets the localized everyone account.
        /// </summary>
        /// <returns></returns>
        public static string GetLocalizedEveryoneAccount()
        {
            string sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null).ToString();
            string localizedsid = new SecurityIdentifier(sid).Translate(typeof(NTAccount)).ToString();
            return localizedsid.Replace(@"BUILTIN\", "").Replace("AUTORITE NT", "");

        }

        /// <summary>
        /// Get local computer Windows License Key
        /// </summary>
        /// <returns></returns>
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
            var value = (byte[])localKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("DigitalProductId");
            var digitalProductId = value;
            var isWin8OrUp =
                (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2)
                ||
                (Environment.OSVersion.Version.Major > 6);
            var productKey = isWin8OrUp ? DecodeProductKeyWin8AndUp(digitalProductId) : DecodeProductKey(digitalProductId);
            localKey.Close();
            return productKey;

        }

        #endregion

        #region Private Methods
        
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

        private static string DecodeProductKeyWin8AndUp(byte[] digitalProductId)
        {
            var key = String.Empty;
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

        #endregion
    }
}
