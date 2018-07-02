using System;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.TeamService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.TeamService.Persistence;
using System.Threading.Tasks;
using StatlerWaldorfCorp.TeamService.LocationClient;

namespace StatlerWaldorfCorp.TeamService
{
    [Route("api/teams/{teamId}/[controller]")]
    public class MembersController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILocationClient _locationClient;

        public MembersController(ITeamRepository teamRepository, ILocationClient locationClient)
        {
            _teamRepository = teamRepository;
            _locationClient = locationClient;
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
        public async Task<IActionResult> GetTeamMember(Guid teamId, Guid memberId)
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

            return Ok(new LocatedMember
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                LastLocation = await _locationClient.GetLatestForMemberAsync(member.Id)
            });
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
            _teamRepository.UpdateTeam(team);
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
            _teamRepository.UpdateTeam(team);
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
            var member = team.Members.FirstOrDefault(m => m.Id.Equals(memberId));
            if (member == null)
            {
                return NotFound();
            }
            team.Members.Remove(member);
            _teamRepository.UpdateTeam(team);
            return Ok(team.Members);
        }

        
		[HttpGet]
		[Route("/api/members/{memberId}/team")]
		public IActionResult GetMemberTeamId(Guid memberId) 
		{
			Guid result = GetTeamIdForMember(memberId);
			if (result != Guid.Empty) {
				return this.Ok(new TeamIdResponse{
					TeamId = result
				});
			} else {
				return NotFound();
			}
        }

        private Guid GetTeamIdForMember(Guid memberId) 
		{
			foreach (var team in _teamRepository.GetTeams()) {
				var member = team.Members.FirstOrDefault( m => m.Id == memberId);
				if (member != null) {
					return team.Id;
				}
			}
			return Guid.Empty;
        }
    }
}