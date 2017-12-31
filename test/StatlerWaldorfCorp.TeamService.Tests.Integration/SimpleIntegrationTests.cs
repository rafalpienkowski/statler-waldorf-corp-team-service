using System;
using Xunit;
using StatlerWaldorfCorp.TeamService.Models;
using StatlerWaldorfCorp.TeamService;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace StatlerWaldorfCorp.TeamService.Tests.Integration
{
    public class SimpleIntegrationTests
    {
        private readonly TestServer _testServer;
        private readonly HttpClient _testClient;
        private readonly Team _teamZombie;

        public SimpleIntegrationTests()
        {
            _testServer = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());

            _testClient = _testServer.CreateClient();
            _teamZombie = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Zombie"
            };
        }

        [Fact]
        public async void TestTeamPostAndGet()
        {
            var stringContent = new StringContent(
                JsonConvert.SerializeObject(_teamZombie),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            var postMessage = await _testClient.PostAsync("/api/teams", stringContent);
            postMessage.EnsureSuccessStatusCode();

            var getMessage = await _testClient.GetAsync("/api/teams");
            getMessage.EnsureSuccessStatusCode();

            var raw = await getMessage.Content.ReadAsStringAsync();
            var teams = JsonConvert.DeserializeObject<List<Team>>(raw);

            Assert.Equal(1, teams.Count);
            Assert.Equal("Zombie", teams[0].Name);
            Assert.Equal(_teamZombie.Id, teams[0].Id);
        }
    }
}
