using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.TeamService.Models;
using Xunit;

namespace StatlerWaldorfCorp.TeamService.Tests
{
    public class MembersControllerTests
    {
        private Models.Member _johnSmith = new Models.Member("John", "Smith", Guid.NewGuid());

        [Fact]
        public void GetMemberTeamIdExistingMemberTeamIdReturned()
        {
            var repository = new TestMemoryTeamRepository();
            var team = repository.GetTeams().First();
            team.Members.Add(_johnSmith);
            var membersController = new MembersController(repository, null);

            var result = (TeamIdResponse)(membersController.GetMemberTeamId(_johnSmith.Id) as ObjectResult).Value;

            Assert.NotNull(result);
            Assert.Equal(team.Id, result.TeamId);
        }

        [Fact]
        public void GetMemberTeamIdNonExistingTeamNotFoundReturned()
        {
            var membersController = new MembersController(new TestMemoryTeamRepository(), null);

            var result = membersController.GetMemberTeamId(_johnSmith.Id) as NotFoundResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}