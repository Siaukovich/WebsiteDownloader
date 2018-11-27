using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebsiteDownloader.BLL
{
    public class DownloaderService : IDownloaderService
    {
        public async Task DownloadAsync(string site)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://stopgame.ru/");
                Console.WriteLine(response);
            }
        }
    }
}
