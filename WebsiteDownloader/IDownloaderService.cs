using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteDownloader
{
    public interface IDownloaderService
    {
        Task DownloadAsync(string site);

        //Task DownloadAsync(string site, DownloadSettings setting);
    }
}
