using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.CognitiveServices.Search.VisualSearch;
using Microsoft.Azure.CognitiveServices.Search.VisualSearch.Models;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;


using Xamarin.Forms;

namespace MarinaNotesApp
{
    public class BingImageSearch : ContentPage
    {

        static string urlImage = "https://yandex.ru/search/?text=%D0%BA%D0%BE%D1%82%D1%8B&clid=2233627&lr=213";
        static string subscriptionKey = "1fdacb8b5f1b4c8ebc28832e57a72ce8";
        VisualSearchClient client = new VisualSearchClient(new ApiKeyServiceClientCredentials(subscriptionKey));
        public BingImageSearch()
        {
            
            Button FindPhotoBtn = new Button { Text = "Найти фото", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            Image img = new Image();
            Label editor = new Label();
            FindPhotoBtn.Clicked += async (o, e) =>
            {

                var stream = await DownloadStream(urlImage);
                // The knowledgeRequest parameter is not required if an image binary is passed in the request body
                var visualSearchResults = client.Images.VisualSearchMethodAsync(image: stream, knowledgeRequest: (string)null).Result;
                // Visual Search results
                if (visualSearchResults.Image?.ImageInsightsToken != null)
                {
                    Console.WriteLine($"Uploaded image insights token: {visualSearchResults.Image.ImageInsightsToken}");
                }
                else
                {
                    Console.WriteLine("Couldn't find image insights token!");
                }

                // List of tags
                if (visualSearchResults.Tags.Count > 0)
                {
                    var firstTagResult = visualSearchResults.Tags[0];
                    Console.WriteLine($"Visual search tag count: {visualSearchResults.Tags.Count}");

                    // List of actions in first tag
                    if (firstTagResult.Actions.Count > 0)
                    {
                        var firstActionResult = firstTagResult.Actions[0];
                        await DisplayAlert("Резы", $"First tag action count: {firstTagResult.Actions.Count}","OK");
                        await DisplayAlert("Резы", $"First tag action type: {firstActionResult.ActionType}", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Резы", "Couldn't find tag actions!", "OK");
                    }
                }
            };




            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout
                    {
                        Children = { FindPhotoBtn },
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                    },
                    img,
                    editor,
                },
            };


        }

        static async Task<MemoryStream> DownloadStream(string url)
        {
            var ms = new MemoryStream();
            using (var http = new System.Net.Http.HttpClient())
            using (var res = await http.GetAsync(url))
                if (res.IsSuccessStatusCode)
                {
                    await res.Content.CopyToAsync(ms);
                    ms.Position = 0;
                }
            return ms;
        }
    }   
}