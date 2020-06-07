using BlazorContracts.Shared.Models;
using IdentityModel.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorContracts.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        //private readonly ApiTokenCacheService _apiTokenCacheService;

        private const string ClientId = "blazorcontracts-web";
        private const string Scope = "blazorcontracts-api";
        private const string Secret = "secret";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Contract>> GetContractsAsync()
        {
            //var token = await _apiTokenCacheService.GetAccessTokenAsync(
            //    clientName: ClientId,
            //    apiScope: Scope,
            //    secret: Secret);

            //_httpClient.SetBearerToken(token);

            var response = await _httpClient.GetAsync("api/contracts");
            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<List<Contract>>(responseContent);
        }

        public async Task<Contract> GetContractByIdAsync(int id)
        {
            //var token = await _apiTokenCacheService.GetAccessTokenAsync(
            //    clientName: ClientId,
            //    apiScope: Scope,
            //    secret: Secret);

            //_httpClient.SetBearerToken(token);

            var response = await _httpClient.GetAsync($"api/contracts/{id}");

            response.EnsureSuccessStatusCode();

            using var responseContent = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<Contract>(responseContent);
        }

        private async Task<string> RequestToken()
        {
            var disco = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(_httpClient, "http://localhost:5000");

            var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(_httpClient, new ClientCredentialsTokenRequest
            {
                Scope = "blazorcontracts-api",
                ClientSecret = "secret",
                Address = disco.TokenEndpoint,
                ClientId = "blazorcontracts-web"
            });

            return tokenResponse.AccessToken;
        }
    }
}