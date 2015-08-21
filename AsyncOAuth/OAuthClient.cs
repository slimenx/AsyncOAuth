using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncOAuth
{
    public class OAuthClient
    {
        protected readonly string _consumerKey;
        protected readonly string _consumerSecret;

        private RequestToken _requestToken;
        protected AccessToken _accessToken;

        private OAuthAuthorizer _authorizer;

        public OAuthClient(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;

            _authorizer = new OAuthAuthorizer(_consumerKey, _consumerSecret);
        }

        public string GetPinRequestUrl(string requestTokenUrl, string authorizeUrl)
        {
            // get request token
            var request = _authorizer.GetRequestToken(requestTokenUrl);
            //request.Start();
            //request.Wait();
            _requestToken = request.Result.Token;

            return _authorizer.BuildAuthorizeUrl(authorizeUrl, _requestToken);
        }

        public AccessToken GetAccessToken(string accessTokenUrl, string pin)
        {
            var accessTokenResponse = _authorizer.GetAccessToken(accessTokenUrl, _requestToken, pin);
            //accessTokenResponse.RunSynchronously();
            _accessToken = accessTokenResponse.Result.Token;

            return _accessToken;
        }

        public string GetMethod(string url)
        {
            var client = OAuthUtility.CreateOAuthClient(_consumerKey, _consumerSecret, _accessToken);
            var requestTask = client.GetStringAsync(url);
            requestTask.RunSynchronously();

            return requestTask.Result;
        }
    }
}
