using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AsyncOAuth.ConsoleApp
{
    class Program
    {
        // set your token
        const string consumerKey = "";
        const string consumerSecret = "";



        static void Main(string[] args)
        {
            // initialize computehash function
            OAuthUtility.ComputeHash = (key, buffer) =>
            {
                using (var hmac = new HMACSHA1(key))
                {
                    return hmac.ComputeHash(buffer);
                }
            };

            var client = new TwitterClient(consumerKey, consumerSecret);

            // sample, twitter access flow
            client.AuthorizeSample();

            var tl = client.GetTimeline(10, 1).Result;
            Console.WriteLine(tl);

            Console.ReadKey();
        }
    }
}