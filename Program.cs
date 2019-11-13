using System;
using System.Net;
using System.IO;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            string html = client.DownloadString("https://yandex.ru/images/search?source=collections&cbir_id=2357542%2FSE0FVm1Uz83UaNwTNAKNEg&rpt=imageview");
            File.WriteAllText("html.txt", html);
            Console.ReadLine();
        }
    }
}
