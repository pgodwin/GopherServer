using GopherServer.Core.Helpers;
using GopherServer.Core.Results;
using GopherServer.Providers.MacintoshGarden.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Providers.MacintoshGarden.Results
{
    public class SoftwareResult : DirectoryResult
    {
        public SoftwareResult(SoftwareItem item) : base()
        {
            this.Items.Add(new DirectoryItem(new string('*', item.Title.Length + 4)));
            this.Items.Add(new DirectoryItem("* " + item.Title + " *"));
            this.Items.Add(new DirectoryItem(new string('*', item.Title.Length + 4)));


            if (item.Screenshots != null)
                foreach (var screenshot in item.Screenshots)
                {
                    this.Items.Add(new DirectoryItem(ItemType.GIF, "Screenshot", "GIF:" + screenshot));
                }


            this.Items.AddRange(item.Description.WrapToDirectoryItems());
            this.Items.Add(new DirectoryItem(""));

            this.Items.Add(new DirectoryItem("Downloads"));
            this.Items.Add(new DirectoryItem("========="));
            foreach (var download in item.Downloads)
            {
                //this.Items.Add(new DirectoryItem(new string('-', download.Title.Length)));
                //this.Items.Add(new DirectoryItem(download.Title));
                //this.Items.Add(new DirectoryItem(new string('-', download.Title.Length)));
                foreach (var link in download.Links)
                {
                    this.Items.Add(new DirectoryItem(link.Text + ":"));
                    this.Items.Add(new DirectoryItem(ItemType.BINHEX, download.Title, "BIN:" + link.Url));
                }
                this.Items.Add(new DirectoryItem("Filesize: " + download.Size));
                this.Items.Add(new DirectoryItem("OS:       " + download.Os));
                this.Items.Add(new DirectoryItem(""));

            }

            

        }
    }
}
