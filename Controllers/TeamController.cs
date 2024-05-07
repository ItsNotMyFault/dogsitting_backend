using dogsitting_backend.ApplicationServices;
using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dogsitting_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private TeamService teamService;
        public TeamController(TeamService teamService)
        {
            this.teamService = teamService;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetTeams()
        {
            List<Team> teams = this.teamService.GetTeamsWithAdmins().Result.ToList();
            string json = JsonConvert.SerializeObject(teams);

            return Ok(json);
        }

        [HttpGet("user/{UserId}")]
        [AllowAnonymous]
        public ActionResult GetUserTeams([FromRoute] Guid UserId)
        {
            List<Team> teams = this.teamService.GetUserTeams(UserId).Result.ToList();
            string json = JsonConvert.SerializeObject(teams);

            return Ok(json);
        }

        [HttpGet("{teamNormalizedName}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTeamByNormalizedName([FromRoute] string teamNormalizedName)
        {
            Team team = await this.teamService.GetTeamByNormalizedName(teamNormalizedName);
            string json = JsonConvert.SerializeObject(team);
            return Ok(json);
        }



        [HttpGet("id/{id}")]
        public async Task<ActionResult> GetTeamById([FromRoute] Guid id)
        {
            Team team = await this.teamService.GetTeamById(id);
            string json = JsonConvert.SerializeObject(team);
            return Ok(json);
        }


        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] UpdateTeamDto teamDto)
        {
            await this.teamService.UpdateTeamAsync(id, teamDto);
            return Ok();
        }



        [HttpPost]
        [Authorize(Policy = "PolicyAdmin")]
        [Route("create")]
        public async Task<ActionResult> Create([FromBody] Team team)
        {
            await this.teamService.PostTeamAsync(team);

            return Ok(JsonConvert.SerializeObject(team));
        }
    }
}
