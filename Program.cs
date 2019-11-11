using System;
using System.Net;
using System.IO;
using AngleSharp;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

namespace NetConsoleApp
{
    class Program
    {

        static public string URI { set; get; }
        static void Main(string[] args)
        {
            URI = "https://avatars.mds.yandex.net/get-pdb/910569/88f6e016-cefc-49e7-987f-b0ae90a54e9c/s1200";

            Parser();
            Console.WriteLine("mew - parser finished working");
            Console.ReadLine();

        }//http://images.google.com/searchbyimage?image_url=https://avatars.mds.yandex.net/get-pdb/910569/88f6e016-cefc-49e7-987f-b0ae90a54e9c/s1200
        static async void Parser()
        {
            var client = new HttpClient();
            string html = client.GetStringAsync("http://images.google.com/searchbyimage?image_url=https://avatars.mds.yandex.net/get-pdb/910569/88f6e016-cefc-49e7-987f-b0ae90a54e9c/s1200").Result;
            Console.WriteLine("http://images.google.com/searchbyimage?image_url=" + URI);
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));
            var selector = document.GetElementsByClassName("iu-card-header");
            Console.WriteLine(html);
            foreach(var i in selector)
            {
                Console.WriteLine(i.InnerHtml);
            }

        }
    }
}