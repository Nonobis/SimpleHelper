using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleHelper.Core
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
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Company;
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
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
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
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
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
                return Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
            }
        }

        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        /// <value>The assembly directory.</value>
        public static string AssemblyDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        /// <summary>
        /// Gets the assembly product.
        /// </summary>
        /// <value>The assembly product.</value>
        public static string AssemblyProduct
        {
            get
            {
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;
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
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
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
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyVersionAttribute>().Version;
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
                return Assembly.GetEntryAssembly();
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
                return Path.GetFileName(Assembly.GetEntryAssembly().Location);
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
                return Assembly.GetEntryAssembly().Location;
            }
        }

        #endregion Public Properties
    }
}