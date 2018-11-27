using System;

using WebsiteDownloader.BLL.Base;

namespace WebsiteDownloader.BLL.PageFinders
{
    public class InsideCurrentDomainOnlyPageFinder : AbstractPageFinder
    {
        public InsideCurrentDomainOnlyPageFinder(int maxDepth)
            : base(maxDepth)
        {
        }

        protected override bool MatchesRule(Uri uri) => this._rootUrl.Host == uri.Host;
    }
}