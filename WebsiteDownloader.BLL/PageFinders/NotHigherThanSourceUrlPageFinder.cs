using System;

using WebsiteDownloader.BLL.Base;

namespace WebsiteDownloader.BLL.PageFinders
{
    public class NotHigherThanSourceUrlPageFinder : AbstractPageFinder
    {
        public NotHigherThanSourceUrlPageFinder(int maxDepth)
            : base(maxDepth)
        {
        }

        protected override bool MatchesRule(Uri uri)
        {
            const UriComponents comparisonComponents = UriComponents.Host | UriComponents.Path;
            var comparisonResult = Uri.Compare(
                this._rootUrl,
                uri,
                comparisonComponents,
                UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase);

            return comparisonResult < 0;
        }
    }
}