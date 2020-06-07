using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorContracts.Web.Services
{
    public class ApiTokenCacheService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        private static readonly Object _lock = new Object();

        private const int cacheExpirationInDays = 1;

        private class AccessTokenItem
        {
            public string AccessToken { get; set; } = string.Empty;
            public DateTime Expiry { get; set; }
        }

        public ApiTokenCacheService(IHttpClientFactory httpClientFactory, IDistributedCache cache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _cache = cache;
        }

        public async Task<string> GetAccessTokenAsync(string clientName, string apiScope, string secret)
        {
            AccessTokenItem accessToken = GetFromCache(clientName);

            if (accessToken != null && accessToken.Expiry > DateTime.UtcNow)
            {
                return accessToken.AccessToken;
            }

            AccessTokenItem accessTokenItem = await RequestNewToken(clientName, apiScope, secret);
            AddToCache(clientName, accessTokenItem);

            return accessTokenItem.AccessToken;
        }

        private AccessTokenItem GetFromCache(string key)
        {
            var item = _cache.GetString(key);
            if (item != null)
            {
                return JsonConvert.DeserializeObject<AccessTokenItem>(item);
            }
            return null;
        }

        private void AddToCache(string key, AccessTokenItem accessTokenItem)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(cacheExpirationInDays));

            lock (_lock)
            {
                _cache.SetString(key, JsonConvert.SerializeObject(accessTokenItem), options);
            }
        }

        private async Task<AccessTokenItem> RequestNewToken(string clientName, string apiScope, string secret)
        {
            try
            {
                var disco = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(_httpClient, "http://localhost:5000");

                if (disco.IsError)
                {
                    throw new ApplicationException($"Error: {disco.Error}");
                }

                var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(_httpClient, new ClientCredentialsTokenRequest
                {
                    Scope = apiScope,
                    ClientId = clientName,
                    ClientSecret = secret,
                    Address = disco.TokenEndpoint
                });

                if (tokenResponse.IsError)
                {
                    throw new ApplicationException($"Error: {tokenResponse.Error}");
                }

                return new AccessTokenItem
                {
                    AccessToken = tokenResponse.AccessToken,
                    Expiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exceptions: {ex}");
            }
        }
    }
}