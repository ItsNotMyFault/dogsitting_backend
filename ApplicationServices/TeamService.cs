using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using MySql.Data.MySqlClient;

namespace dogsitting_backend.ApplicationServices
{
    public class TeamService
    {
        private readonly IGenericRepository<Team> _teamGenericRepository;
        private readonly TeamSQLRepository _teamSQLRepository;

        public TeamService(IGenericRepository<Team> teamGenereicRepository, TeamSQLRepository teamSQLRepository)
        {
            _teamGenericRepository = teamGenereicRepository;
            _teamSQLRepository = teamSQLRepository;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamGenericRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsInclude()
        {
            return await _teamSQLRepository.GetAllTeamsAsync();
        }

        public async Task<Team> PostTeamAsync(Team team)
        {
            this.ComplexCommand();
            await _teamGenericRepository.AddAsync(team);
            return team;
        }

        public void ComplexCommand()
        {
            this._teamSQLRepository.GetTeamByUserId();
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
