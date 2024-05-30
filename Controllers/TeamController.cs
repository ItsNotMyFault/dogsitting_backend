using dogsitting_backend.ApplicationServices;
using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;
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
            List<TeamResponse> teams = this.teamService.GetTeamsWithMedias().Result.ToList();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(teams, settings);
            return Ok(json);
        }

        //todo: move this in applicationUserController
        //user/id/teams
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
            TeamResponse team = await this.teamService.GetTeamById(id);
            string json = JsonConvert.SerializeObject(team);
            return Ok(json);
        }


        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] UpdateTeamDto teamDto)
        {
            await this.teamService.UpdateTeamAsync(id, teamDto);
            return Ok();
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateTeamDto team)
        {
            await this.teamService.CreateTeamAsync(team);

            return Ok(JsonConvert.SerializeObject(team));
        }

        [HttpGet("{Id}/media")]
        public async Task<ActionResult> GetTeamMedias([FromRoute] Guid Id)
        {
            List<TeamMediaResponse> mediaresponses = await this.teamService.GetTeamMedias(Id);
            if(mediaresponses.Count > 4)
            {
                throw new Exception("Too Many pictures shouldn't happen.");
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(mediaresponses, settings);
            return Ok(json);
        }

        [HttpPost("{Id}/media", Name = "AddTeamMedia")]
        public async Task<ActionResult> AddMedia([FromRoute] Guid Id)
        {
            List<IFormFile> files = Request.Form.Files.ToList();
            var positions = Request.Form["positions"].Select(p => int.Parse(p)).ToList();
            if (files.Count > 4)
            {
                throw new Exception("Too many files.");
            }
            var filePositionPairs = files.Zip(positions, (file, position) => (file, position)).ToList();

            await this.teamService.UpdateTeamMedia(Id, filePositionPairs);
            return Ok();
        }

        [HttpDelete("{Id}/media", Name = "RemoveTeamMedia")]
        public async Task<ActionResult> RemoveMedia([FromBody] IEnumerable<Guid> fileIds)
        {

            await this.teamService.RemoveMediaFromReservation(fileIds);
            return Ok();
        }
    }
}
