using System;
using Xunit;
using StatlerWaldorfCorp.TeamService;
using StatlerWaldorfCorp.TeamService.Models;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace StatlerWaldorfCorp.TeamService.Tests
{
    public class TeamsControllerTests
    {
        TeamsController _controller = new TeamsController(new MemoryTeamRepository());

        [Fact]
        public async void QueryTeamListReturnsCorrectTeamsAsync()
        {
            var teams = ((await _controller.GetAllTeamsAsync() as OkObjectResult).Value as ICollection<Team>);
            Assert.Equal(teams.Count, 3);
        }

        [Fact]
        public async void CreateTeamAddsTeamToList()
        {
            var controller = new TeamsController(new MemoryTeamRepository());
            var teams = ((await controller.GetAllTeamsAsync()) as OkObjectResult).Value as ICollection<Team>;
            var original = new List<Team>(teams);

            var t = new Team("sample");
            var result = controller.CreateTeam(t);

            var newTeamsRaw = ((await controller.GetAllTeamsAsync() as OkObjectResult).Value as ICollection<Team>);

            var newTeams = new List<Team>(newTeamsRaw);
            Assert.Equal(newTeams.Count, original.Count + 1);
            var sampleTeam = newTeams.FirstOrDefault(st => st.Name == "sample");
            Assert.NotNull(sampleTeam);
        }
    }
}
