using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using StatlerWaldorfCorp.TeamService.Models;
using Newtonsoft.Json;

namespace StatlerWaldorfCorp.TeamService.LocationClient
{
    public class HttpLocationClient : ILocationClient
    {
        public string Url { get; set; }

        public HttpLocationClient(string url)
        {
            Url = url;
        }

        public async Task<LocationRecord> GetLatestForMemberAsync(Guid memberId)
        {
            LocationRecord locationRecord = null;

            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Url);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpResponseMessage = await httpClient.GetAsync($"api/locations/{memberId}/latest");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var json = await httpResponseMessage.Content.ReadAsStringAsync();
                    locationRecord = JsonConvert.DeserializeObject<LocationRecord>(json);
                }

                return locationRecord;
            }
        }
    }
}