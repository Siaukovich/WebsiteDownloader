using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace WebsiteDownloader.BLL.Base
{
    public abstract class AbstractPageFinder
    {
        private readonly Queue<(Uri uri, int depth)> _uriQueue;

        private readonly HashSet<string> _proceededUris;

        private readonly int _maxDepth;

        protected Uri _rootUrl;

        protected AbstractPageFinder(int maxDepth)
        {
            this._maxDepth = maxDepth;
            this._uriQueue = new Queue<(Uri, int)>();
            this._proceededUris = new HashSet<string>();
        }

        public IEnumerable<WebPage> GetWebPages(Uri rootUrl)
        {
            this._rootUrl = rootUrl ?? throw new ArgumentNullException(nameof(rootUrl));

            this._uriQueue.Enqueue((rootUrl, -1));
            this._proceededUris.Add(rootUrl.AbsoluteUri);

            while (this._uriQueue.Count != 0)
            {
                var current = this._uriQueue.Dequeue();

                HtmlDocument htmlDoc = null;
                try
                {
                    htmlDoc = this.GetHtmlFromUriAsync(current.uri);
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCEPTION");
                    continue;
                }

                yield return new WebPage(htmlDoc, current.uri);

                var references = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
                if (references == null) // No "a" nodes.
                {
                    continue;
                }

                foreach (var href in references)
                {
                    this.EnqueueNewUri(current.uri, href, current.depth);
                }
            }
        }

        protected abstract bool MatchesRule(Uri uri);

        private void EnqueueNewUri(Uri currentUri, HtmlNode href, int depth)
        {
            var uri = href.Attributes["href"].Value;
            var newUri = this.GetNewUri(currentUri, uri);

            if (this.UriWasProceeded(newUri))
            {
                return;
            }

            var newUriDepth = depth + 1;

            if (newUriDepth < this._maxDepth && this.MatchesRule(newUri))
            {
                this._uriQueue.Enqueue((newUri, newUriDepth));
            }
        }

        private bool UriWasProceeded(Uri uri)
        {
            if (this._proceededUris.Contains(uri.AbsoluteUri))
            {
                return true;
            }

            this._proceededUris.Add(uri.AbsoluteUri);
            return false;
        }

        private Uri GetNewUri(Uri currentUri, string uri)
        {
            if (Uri.TryCreate(uri, UriKind.Absolute, out Uri result))
            {
                return result;
            }

            var abs = currentUri.AbsoluteUri;
            var currentUriStr = abs.Remove(abs.Length - 1); // Removes last '/'.
            return new Uri(currentUriStr + uri);
        }

        private  HtmlDocument GetHtmlFromUriAsync(Uri uri)
        {
            var htmlDoc = new HtmlDocument();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(uri).Result;
                var html = response.Content.ReadAsStringAsync().Result;
                htmlDoc.LoadHtml(html);
            }

            return htmlDoc;
        }
    }
}