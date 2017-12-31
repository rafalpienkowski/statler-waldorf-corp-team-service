using System;
using System.Collections.Generic;
using StatlerWaldorfCorp.TeamService.Models;

namespace StatlerWaldorfCorp.TeamService.Persistence
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();
        Team GetTeam(Guid id);
        void AddTeam(Team team);
        void UpdateTeam(Team team);
        Team DeleteTeam(Guid id);
    }
}