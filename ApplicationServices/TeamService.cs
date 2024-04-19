using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.ApplicationServices
{
    public class TeamService
    {
        private readonly IGenericRepository<Team> _teamGenericRepository;
        private readonly TeamSQLRepository _teamSQLRepository;
        private readonly UserManager<AuthUser> _userManager;
        private readonly UserService _userService;

        public TeamService(
            IGenericRepository<Team> teamGenereicRepository,
            TeamSQLRepository teamSQLRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AuthUser> userManager,
             UserService userService
            )
        {
            _teamGenericRepository = teamGenereicRepository;
            _teamSQLRepository = teamSQLRepository;
            _userManager = userManager;
            this._userService = userService;
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
            AuthUser res = _userManager.GetUserAsync(claimsPrincipal).Result;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamGenericRepository.GetAllAsync();
        }

        public async Task<Team> GetTeamByNormalizedName(string teamNormalizedName)
        {
            return await _teamSQLRepository.GetTeamByNormalizedName(teamNormalizedName);
        }

        public async Task<IEnumerable<Team>> GetTeamsWithAdmins()
        {
            return await _teamSQLRepository.GetAllTeamsAsync();
            //return await _teamSQLRepository.GetAllTeamsAsync();
        }

        public async Task<Team> PostTeamAsync(Team team)
        {
            //get logged in user, check if he is already mapped to a team, if yes boom.
            //if not accept creation.
            AuthUser user = this._userService.GetCurrentUserAsync().Result;
            Team foundTeam = null;
            try
            {
                foundTeam = this._teamSQLRepository.GetTeamByUser(user.ApplicationUser);
            }
            catch (Exception ex)
            {
                var e = ex;
            }
            if (foundTeam != null)
            {
                throw new Exception("This user already has a team assigned to it.");
            }

            team.NormalizeTeamName();
            team.Admins.Add(user.ApplicationUser);
            await _teamGenericRepository.AddAsync(team);
            return team;
        }

        public List<Team> GetWithSQL()
        {
            List<Team> teams = new();
            MySqlConnection myConnection;
            try
            {
                myConnection = new MySqlConnection("server=127.0.0.1;database=dogsitting;uid=root;pwd=alexis;");
                //myConnection = new MySqlConnection(this.connectionstring);
                //open a connection
                myConnection.Open();

                // create a MySQL command and set the SQL statement with parameters
                MySqlCommand myCommand = new MySqlCommand
                {
                    Connection = myConnection,
                    //myCommand.CommandText = @"SELECT * FROM clients WHERE client_id = @clientId;";
                    CommandText = @"SELECT id, name FROM Teams;"
                };
                //myCommand.Parameters.AddWithValue("@clientId", clientId);

                // execute the command and read the results
                var myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {


                    teams.Add(new Team
                    {
                        Id = Guid.Parse(myReader.GetString("id")),
                        Name = myReader.GetString("name"),
                    });


                    var id = myReader.GetString("id");
                    var name = myReader.GetString("name");
                }
                myConnection.Close();
            }
            catch (MySqlException ex)
            {
                var tt = ex;
            }

            return teams;
        }

    }
}
