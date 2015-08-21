AsyncOAuth
==========

Portable Client Library and HttpClient based OAuth library, including all platform(for PCL as .NET 4.0, .NET 4.5, Silverlight4, Silverlight5, Windows Phone 7.5, Windows Phone 8.0, Windows Store Apps).
Forked from https://github.com/neuecc/AsyncOAuth

Usage
---
at first, you must initialize hash function(ApplicationStart etc...)

```csharp
// Silverlight, Windows Phone, Console, Web, etc...
OAuthUtility.ComputeHash = (key, buffer) => { using (var hmac = new HMACSHA1(key)) { return hmac.ComputeHash(buffer); } };
 
// Windows Store Apps
AsyncOAuth.OAuthUtility.ComputeHash = (key, buffer) =>
{
    var crypt = Windows.Security.Cryptography.Core.MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
    var keyBuffer = Windows.Security.Cryptography.CryptographicBuffer.CreateFromByteArray(key);
    var cryptKey = crypt.CreateKey(keyBuffer);
 
    var dataBuffer = Windows.Security.Cryptography.CryptographicBuffer.CreateFromByteArray(buffer);
    var signBuffer = Windows.Security.Cryptography.Core.CryptographicEngine.Sign(cryptKey, dataBuffer);
 
    byte[] value;
    Windows.Security.Cryptography.CryptographicBuffer.CopyToByteArray(signBuffer, out value);
    return value;
};
```

Create OAuthClient

```csharp
var client = new OAuthClient("consumerKey", "consumerSecret");
string pinRequestUrl = client.GetPinRequestUrl("requestTokenUrl", "authorizeUrl");
 
//user goes to the link provided and get pin
string pin;

client.GetAccessToken("accessTokenUrl", pin);
```

License
---
under [MIT License](http://opensource.org/licenses/MIT)
