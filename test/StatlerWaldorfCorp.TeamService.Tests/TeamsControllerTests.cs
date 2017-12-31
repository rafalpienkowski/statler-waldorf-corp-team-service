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
        public void QueryTeamListReturnsCorrectTeams()
        {
            var teams = (_controller.GetAllTeams() as OkObjectResult).Value as ICollection<Team>;
            Assert.Equal(teams.Count, 3);
        }

        [Fact]
        public void CreateTeamAddsTeamToList()
        {
            var controller = new TeamsController(new MemoryTeamRepository());
            var teams = (controller.GetAllTeams() as OkObjectResult).Value as ICollection<Team>;
            var original = new List<Team>(teams);

            var team = new Team("sample");
            var result = controller.CreateTeam(team);

            var newTeamsRaw = (controller.GetAllTeams() as OkObjectResult).Value as ICollection<Team>;

            var newTeams = new List<Team>(newTeamsRaw);
            Assert.Equal(newTeams.Count, original.Count + 1);
            var sampleTeam = newTeams.FirstOrDefault(st => st.Name == "sample");
            Assert.NotNull(sampleTeam);
        }
    }
}
