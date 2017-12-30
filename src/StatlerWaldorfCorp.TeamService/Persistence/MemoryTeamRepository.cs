using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;

namespace StatlerWaldorfCorp.TeamService.Persistence
{
    public class MemoryTeamRepository : ITeamRepository
    {
        protected static ICollection<Team> _teams;

        public MemoryTeamRepository()
        {
            if(_teams == null)
            {
                _teams = new List<Team>
                {
                    new Team("one"),
                    new Team("two")
                };
            }
        }

        public MemoryTeamRepository(ICollection<Team> teams) => _teams = teams;

        public void AddTeam(Team team)
        {
            _teams.Add(team);
        }

        public IEnumerable<Team> GetTeams() => _teams;
    }
}