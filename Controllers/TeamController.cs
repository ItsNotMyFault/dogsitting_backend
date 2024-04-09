using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;

namespace dogsitting_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private DogsittingDBContext context;
        private TeamService teamService;
        public TeamController(IConfiguration configuration, DogsittingDBContext context, TeamService teamService)
        {
            this.teamService = teamService;
            this.context = context;
        }


        [HttpGet(Name = "GetTeam")]
        public ActionResult Get()
        {
            List<Team> teams = this.teamService.GetTeamsInclude().Result.ToList();
            string json = JsonConvert.SerializeObject(teams);

            return Ok(json);
        }


        [HttpPost(Name = "PostTeam")]
        public async Task<ActionResult> Post(Team team)
        {
            await this.teamService.PostTeamAsync(team);
            return Ok(JsonConvert.SerializeObject(team));
        }
    }
}
