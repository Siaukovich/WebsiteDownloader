using System;
using HtmlAgilityPack;

namespace WebsiteDownloader
{
    public class WebPage
    {
        public HtmlDocument Html { get; }

        public Uri Uri { get; }

        public WebPage(HtmlDocument html, Uri uri)
        {
            this.Html = html;
            this.Uri = uri;
        }
    }
}
