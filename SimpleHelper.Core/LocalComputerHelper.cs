using System;
using System.Linq;
// TODO : System.Management not existing for now on NetStandard
// using System.Management;
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
            // TODO : System.Management not existing for now on NetStandard
            return 0;
            //return new ManagementObjectSearcher("Select NumberOfCores from Win32_Processor").Get().Cast<ManagementBaseObject>().Sum(item => Convert.ToInt32(item.GetPropertyValue("NumberOfCores")));
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
         
        #endregion
    }
}
