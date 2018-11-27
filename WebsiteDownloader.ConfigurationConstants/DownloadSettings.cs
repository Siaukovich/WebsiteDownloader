using System;
using System.Collections.Generic;

namespace WebsiteDownloader.ConfigurationConstants
{
    public class DownloadSettings
    {
        public DownloadSettings(int depth, UrlRestriction restriction, List<string> resourceExtensions)
        {
            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(depth), $"{nameof(depth)} must be at least 0.");
            }

            this.Depth = depth;
            this.Restriction = restriction;
            this.ResourceExtensions = resourceExtensions ?? throw new ArgumentNullException(nameof(resourceExtensions));
        }

        public int Depth { get; }

        public UrlRestriction Restriction { get; }

        public List<string> ResourceExtensions { get; }
    }

    public enum UrlRestriction
    {
        NoRestrictions,
        InsideCurrentDomainOnly,
        NotHigherThanSourceUrl
    }
}