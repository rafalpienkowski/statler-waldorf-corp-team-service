using System;
using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;
using StatlerWaldorfCorp.TeamService.Persistence;

namespace StatlerWaldorfCorp.TeamService.Tests
{
    public class TestMemoryTeamRepository : MemoryTeamRepository {
		public TestMemoryTeamRepository() : base(CreateInitialFake()) {
			 
		}

		private static ICollection<Team> CreateInitialFake()
		{
			var teams = new List<Team>();
			teams.Add(new Team("one", Guid.NewGuid()));
			teams.Add(new Team("two", Guid.NewGuid()));

			return teams;
		}
} 
}