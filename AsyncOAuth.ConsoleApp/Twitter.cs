using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AsyncOAuth.ConsoleApp
{
    // a sample of twitter client
    public class TwitterClient : OAuthClient
    {

        public TwitterClient(string consumerKey, string consumerSecret):
            base(consumerKey, consumerSecret)
        {

        }

        // sample flow for Twitter authroize
        public void AuthorizeSample()
        {
            //getting pin url
            var pinRequestUrl = GetPinRequestUrl("https://api.twitter.com/oauth/request_token", "https://api.twitter.com/oauth/authorize");

            // open browser and get PIN Code
            Process.Start(pinRequestUrl);

            // enter pin
            Console.WriteLine("ENTER PIN");
            var pinCode = Console.ReadLine();

            // save access token.
            var accessToken = GetAccessToken("https://api.twitter.com/oauth/access_token", pinCode);
            Console.WriteLine("Key:" + accessToken.Key);
            Console.WriteLine("Secret:" + accessToken.Secret);
        }

        public async Task<string> GetTimeline(int count, int page)
        {
            var client = OAuthUtility.CreateOAuthClient(_consumerKey, _consumerSecret, _accessToken);

            var json = await client.GetStringAsync("https://api.twitter.com/1.1/statuses/home_timeline.json?count=" + count + "&page=" + page);
            return json;
        }

        public async Task<string> PostUpdate(string status)
        {
            var client = OAuthUtility.CreateOAuthClient(_consumerKey, _consumerSecret, _accessToken);

            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("status", status) });

            var response = await client.PostAsync("https://api.twitter.com/1.1/statuses/update.json", content);
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }

        public async Task GetStream(Action<string> fetchAction)
        {
            var client = OAuthUtility.CreateOAuthClient(_consumerKey, _consumerSecret, _accessToken);
            client.Timeout = System.Threading.Timeout.InfiniteTimeSpan; // set infinite timespan

            using (var stream = await client.GetStreamAsync("https://userstream.twitter.com/1.1/user.json"))
            using (var sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    var s = await sr.ReadLineAsync();
                    fetchAction(s);
                }
            }
        }



        public async Task<string> UpdateWithMedia(string status, byte[] media, string fileName)
        {
            var client = OAuthUtility.CreateOAuthClient(_consumerKey, _consumerSecret, _accessToken);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(status), "\"status\"");
            content.Add(new ByteArrayContent(media), "media[]", "\"" + fileName + "\"");

            var response = await client.PostAsync("https://upload.twitter.com/1/statuses/update_with_media.json", content);
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}