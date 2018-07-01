using System;
using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;
using System.Linq;

namespace StatlerWaldorfCorp.TeamService.Persistence
{
    public class MemoryTeamRepository : ITeamRepository
    {
        protected ICollection<Team> _teams;

        public MemoryTeamRepository()
        {
            if(_teams == null)
            {
                _teams = new List<Team>();
            }
        }

        public MemoryTeamRepository(ICollection<Team> teams) => _teams = teams;

        public void AddTeam(Team team)
        {
            _teams.Add(team);
        }

        public void UpdateTeam(Team team)
        {
            if(DeleteTeam(team.Id) != null)
            {
                AddTeam(team);
            }
        }

        public Team GetTeam(Guid id) => _teams.FirstOrDefault(t => t.Id.Equals(id));

        public Team DeleteTeam(Guid id)
        {
            var teamToDelete = _teams.FirstOrDefault(t => t.Id.Equals(id));
            if (teamToDelete != null)
            {
                _teams.Remove(teamToDelete);
            }
            return teamToDelete;            
        }
        
        public IEnumerable<Team> GetTeams() => _teams;
    }
}