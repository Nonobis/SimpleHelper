using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleHelper
{
    /// <summary>
    /// Class AssemblyHelper
    /// </summary>
    public static class AssemblyHelper
    {
        #region Public Properties

        /// <summary>
        /// Gets the assembly company.
        /// </summary>
        /// <value>The assembly company.</value>
        public static string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                var attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                {
                    return "";
                }
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        /// <summary>
        /// Determines whether this instance is debug.
        /// </summary>
        /// <returns></returns>
        public static bool IsDebug
        {
            get
            {
                var customAttributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(DebuggableAttribute), false);
                if ((customAttributes.Length == 1))
                {
                    var attribute = customAttributes[0] as DebuggableAttribute;
                    return attribute != null && (attribute.IsJITOptimizerDisabled && attribute.IsJITTrackingEnabled);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the assembly copyright.
        /// </summary>
        /// <value>The assembly copyright.</value>
        public static string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                var attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                {
                    return "";
                }
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets the assembly description.
        /// </summary>
        /// <value>The assembly description.</value>
        public static string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                var attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                {
                    return "";
                }
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// Gets the get referenced assembly.
        /// </summary>
        /// <value>
        /// The get referenced assembly.
        /// </value>
        public static List<AssemblyName> GetReferencedAssembly
        {
            get {
                return Assembly.GetCallingAssembly().GetReferencedAssemblies().ToList();
            }
        }

        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        /// <value>The assembly directory.</value>
        public static string AssemblyDirectory => Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

        /// <summary>
        /// Gets the assembly product.
        /// </summary>
        /// <value>The assembly product.</value>
        public static string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                var attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                {
                    return "";
                }
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>The assembly title.</value>
        public static string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                var attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the assembly version major.
        /// </summary>
        /// <value>The assembly version major.</value>
        public static int AssemblyVersionMajor
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.Major;
            }
        }

        /// <summary>
        /// Gets the assembly version minor.
        /// </summary>
        /// <value>The assembly version minor.</value>
        public static int AssemblyVersionMinor
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.Minor;
            }
        }

        /// <summary>
        /// Gets the assembly version revision.
        /// </summary>
        /// <value>The assembly version revision.</value>
        public static int AssemblyVersionRevision
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.Revision;
            }
        }

        /// <summary>
        /// Gets the current assembly.
        /// </summary>
        /// <value>The current assembly.</value>
        public static Assembly CurrentAssembly
        {
            get
            {
                return Assembly.GetCallingAssembly();
            }
        }

        /// <summary>
        /// Gets the name of the executable.
        /// </summary>
        /// <value>The name of the executable.</value>
        public static string ExecutableName
        {
            get
            {
                return Path.GetFileName(Assembly.GetCallingAssembly().Location);
            }
        }

        /// <summary>
        /// Gets the executable path.
        /// </summary>
        /// <value>The executable path.</value>
        public static string ExecutablePath
        {
            get
            {
                return Assembly.GetCallingAssembly().Location;
            }
        }

        #endregion Public Properties
    }
}