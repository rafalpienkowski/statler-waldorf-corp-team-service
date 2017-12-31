using System;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.TeamService.Persistence;
using System.Threading.Tasks;

namespace StatlerWaldorfCorp.TeamService
{
    [Route("api/[controller]")]
    public class TeamsController : Controller
    {
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        public IActionResult GetAllTeams() => Ok(_teamRepository.GetTeams());

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTeam(Guid id)
        {
            var team = _teamRepository.GetTeam(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [HttpPost]
        public IActionResult CreateTeam([FromBody] Team team)
        {
            _teamRepository.AddTeam(team);
            return Created($"/teams/{team.Id}", team);
        }

        [HttpPut]
        public IActionResult UpdateTeam([FromBody] Team team)
        {
            _teamRepository.UpdateTeam(team);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTeam(Guid id)
        {
            var team = _teamRepository.GetTeam(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team.Id);
        }
    }
}