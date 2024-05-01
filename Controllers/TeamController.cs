using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace dogsitting_backend.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private TeamService teamService;
        public TeamController(TeamService teamService)
        {
            this.teamService = teamService;
        }


        //[HttpGet(Name = "GetTeam")]
        //[AllowAnonymous]
        //public ActionResult Get()
        //{
        //    List<Team> teams = this.teamService.GetTeamsInclude().Result.ToList();
        //    string json = JsonConvert.SerializeObject(teams);

        //    return Ok(json);


        //[HttpGet(Name ="Team")]
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
