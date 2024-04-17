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
    [AllowAnonymous]
    //[ApiController]
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
        //}

        [HttpGet(Name ="Team")]
        [AllowAnonymous]
        public ActionResult Jambon()
        {
            List<Team> teams = this.teamService.GetTeamsWithAdmins().Result.ToList();
            string json = JsonConvert.SerializeObject(teams);

            return Ok(json);
        }

        [HttpGet("Boom")]
        [AllowAnonymous]
        public ActionResult Jambon2()
        {
            List<Team> teams = this.teamService.GetTeamsWithAdmins().Result.ToList();
            string json = JsonConvert.SerializeObject(teams);

            return Ok(json);
        }


        [HttpPost(Name = "PostTeam")]
        [AllowAnonymous]
        public async Task<ActionResult> Post(Team team)
        {
            await this.teamService.PostTeamAsync(team);
            return Ok(JsonConvert.SerializeObject(team));
        }
    }
}
