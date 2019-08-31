using GopherServer.Core.Helpers;
using GopherServer.Core.Models;
using GopherServer.Core.Results;
using GopherServer.Providers.MacintoshGarden.Models;

namespace GopherServer.Providers.MacintoshGarden.Results
{
    public class SoftwareResult : DirectoryResult
    {
        public SoftwareResult(SoftwareItem item) : base()
        {
            Items.Add(new DirectoryItem(new string('*', item.Title.Length + 4)));
            Items.Add(new DirectoryItem("* " + item.Title + " *"));
            Items.Add(new DirectoryItem(new string('*', item.Title.Length + 4)));

            if (item.Screenshots != null)
            {
                foreach (var screenshot in item.Screenshots) Items.Add(new DirectoryItem(ItemType.GIF, "Screenshot", "GIF:" + screenshot));
            }

            Items.AddRange(item.Description.WrapToDirectoryItems());
            Items.Add(new DirectoryItem(""));

            Items.Add(new DirectoryItem("Downloads"));
            Items.Add(new DirectoryItem("========="));
            foreach (var download in item.Downloads)
            {
                //Items.Add(new DirectoryItem(new string('-', download.Title.Length)));
                //Items.Add(new DirectoryItem(download.Title));
                //Items.Add(new DirectoryItem(new string('-', download.Title.Length)));
                foreach (var link in download.Links)
                {
                    Items.Add(new DirectoryItem(link.Text + ":"));
                    Items.Add(new DirectoryItem(Core.Helpers.FileTypeHelpers.GetItemTypeFromFileName(link.Url), download.Title, "BIN:" + link.Url));
                }
                Items.Add(new DirectoryItem("Filesize: " + download.Size));
                Items.Add(new DirectoryItem("OS:       " + download.Os));
                Items.Add(new DirectoryItem(""));
            }
        }
    }
}
