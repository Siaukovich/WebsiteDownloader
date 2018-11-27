using System;
using WebsiteDownloader.BLL.Base;

namespace WebsiteDownloader.BLL.PageFinders
{
    public class NoRestrictionsPageFinder : AbstractPageFinder
    {
        public NoRestrictionsPageFinder(int maxDepth)
            : base(maxDepth)
        {
        }

        protected override bool MatchesRule(Uri uri) => true;
    }
}
