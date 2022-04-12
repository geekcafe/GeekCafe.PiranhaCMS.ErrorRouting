using System;
using Piranha.Extend;

namespace GeekCafe.PiranhaCMS.ErrorRouting
{
    /// <summary>
    /// The identity module.
    /// </summary>
    public class Module : IModule
    {
        /// <summary>
        /// Gets the Author
        /// </summary>
        public string Author => "Eric Wilson";

        /// <summary>
        /// Gets the Name
        /// </summary>
        public string Name => $"{GetType().Assembly.GetName().Name}";

        /// <summary>
        /// Gets the Version
        /// </summary>
        public string Version => $"{Piranha.Utils.GetAssemblyVersion(GetType().Assembly)}";

        /// <summary>
        /// Gets the description
        /// </summary>
        public string Description => "Error routing for 404 & 500 pages.";

        /// <summary>
        /// Gets the package url.
        /// </summary>
        public string PackageUrl => "N/A";

        /// <summary>
        /// Gets the icon url.
        /// </summary>
        public string IconUrl => "https://cdn-media.geekcafe.com/assets/img/logos/gc-main.png";

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public void Init()
        {
            // nothing special right now
        }
    }
}
