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
    [Route("api/teams/{teamId}/[controller]")]
    public class MembersController : Controller
    {
        private readonly ITeamRepository _teamRepository;

        public MembersController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetTeamMembers(Guid teamId)
        {
            var team = _teamRepository.GetTeam(teamId);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team.Members);
        }

        [HttpGet]
        [Route("{memberId}")]
        public IActionResult GetTeamMember(Guid teamId, Guid memberId)
        {
            var team = _teamRepository.GetTeam(teamId);
            if (team == null)
            {
                return NotFound();
            }
            
            var member = team.Members.FirstOrDefault(m => m.Id.Equals(memberId));
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpPut]
        [Route("{memberId}")]
        public IActionResult UpdateTeamMember(Guid teamId, Guid memberId, [FromBody] Member member)
        {
            var team = _teamRepository.GetTeam(teamId);
            if (team == null)
            {
                return NotFound();
            }
            var memberToUpdate = team.Members.FirstOrDefault(m => m.Id.Equals(member.Id));
            if (memberToUpdate == null)
            {
                return NotFound();
            }
            team.Members.Remove(memberToUpdate);
            team.Members.Add(member);
            return NoContent();
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddTeamMember(Guid teamId, [FromBody] Member member)
        {
            var team = _teamRepository.GetTeam(teamId);
            if (team == null)
            {
                return NotFound();
            }

            team.Members.Add(member);
            return Created($"/teams/{teamId}/[controller]/{member.Id}", member);
        }

        [HttpDelete]
        [Route("{memberId}")]
        public IActionResult DeleteTeamMember(Guid teamId, Guid memberId)
        {
            var team = _teamRepository.GetTeam(teamId);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team.Members);
        }
    }
}