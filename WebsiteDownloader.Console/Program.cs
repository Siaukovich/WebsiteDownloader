using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Media.Imaging;

using HtmlAgilityPack;


namespace WebsiteDownloader.ConsolePL
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Net;
    using System.Threading.Tasks;

    using WebsiteDownloader.BLL.PageFinders;

    class Program
    {
        static void Main(string[] args)
        {
            var finder = new InsideCurrentDomainOnlyPageFinder(1);

            foreach (var webPage in finder.GetWebPages(new Uri("https://stopgame.ru")))
            {
                Console.WriteLine(webPage.Uri.AbsoluteUri);
            }

            Console.WriteLine("done");

            Console.ReadKey();
        }

        private static async void MainAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://stopgame.ru/");
                var body = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(body);

                var imageNodes = doc.DocumentNode.SelectNodes("//img[@src]");

                Console.WriteLine("Images: ");
                foreach (var node in imageNodes)
                {
                    Console.WriteLine(node.Attributes["src"].Value);

                    var url = new Uri(node.Attributes["src"].Value);
                    var filename = url.Segments.Last();

                    node.SetAttributeValue("src", $"images/{filename}");
                    //node.Attributes["src"].Value = $"images/{filename}";
                    using (var file = File.Create($"images/{filename}"))
                    { // create a new file to write to
                        if (url.Scheme != "http" && url.Scheme != "https")
                            continue;

                        response = await client.GetAsync(url);

                        var contentStream = await response.Content.ReadAsStreamAsync(); // get the actual content stream
                        await contentStream.CopyToAsync(file); // copy that stream to the file stream
                    }

                    //await LoadImage(url);
                    Console.WriteLine("-------------------------------------------");
                }

                var cssNodes = doc.DocumentNode.SelectNodes("//link[@rel='stylesheet' and @href]");
                Console.WriteLine("Styles:");
                foreach (var node in cssNodes)
                {
                    Console.WriteLine(node.Attributes["href"].Value);

                    var url = new Uri(node.Attributes["href"].Value);
                    var filename = url.PathAndQuery.Replace("/", ".").Replace("?", ",");

                    node.SetAttributeValue("href", $"styles/{filename}");
                    //node.Attributes["src"].Value = $"images/{filename}";
                    using (var file = File.Create($"styles/{filename}"))
                    { // create a new file to write to
                        //if (url.Scheme != "http" && url.Scheme != "https")
                        //    continue;

                        response = await client.GetAsync(url);

                        var contentStream = await response.Content.ReadAsStreamAsync(); // get the actual content stream
                        await contentStream.CopyToAsync(file); // copy that stream to the file stream
                    }

                    //await LoadImage(url);
                    Console.WriteLine("-------------------------------------------");
                }

                var jsNodes = doc.DocumentNode.SelectNodes("//script[@src]");
                Console.WriteLine("Scripts:");
                foreach (var node in jsNodes)
                {
                    Console.WriteLine(node.Attributes["src"].Value);

                    var url = new Uri(node.Attributes["src"].Value);
                    var filename = url.PathAndQuery.Replace("/", ".").Replace("?", ",");

                    node.SetAttributeValue("src", $"scripts/{filename}");
                    //node.Attributes["src"].Value = $"images/{filename}";
                    using (var file = File.Create($"scripts/{filename}"))
                    { // create a new file to write to
                        if (url.Scheme != Uri.UriSchemeHttp && url.Scheme != Uri.UriSchemeHttps)
                            continue;

                        response = await client.GetAsync(url);

                        var contentStream = await response.Content.ReadAsStreamAsync(); // get the actual content stream
                        await contentStream.CopyToAsync(file); // copy that stream to the file stream
                    }

                    //await LoadImage(url);
                    Console.WriteLine("-------------------------------------------");
                }

                using (var fs = new FileStream("stopgame.ru.html", FileMode.Create))
                {
                    doc.Save(fs);
                }

                Console.WriteLine("done");
                //Console.WriteLine(body);
            }
        }

        private static async Task LoadImage(Uri url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                BitmapImage bitmap = new BitmapImage();
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    bitmap.StreamSource = await response.Content.ReadAsStreamAsync();
                    var image = new Bitmap((int)bitmap.Height, (int)bitmap.Width);
                    image.Save($"images/{url}", ImageFormat.Jpeg);
                }
            }
        }
    }
}
