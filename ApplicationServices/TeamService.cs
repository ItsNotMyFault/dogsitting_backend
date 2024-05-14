using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Domain.Utils;
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
        private readonly IGenericRepository<Calendar> _calendarGenericSQLRepository;
        private readonly TeamSQLRepository _teamSQLRepository;
        private readonly CalendarSQLRepository _calendarSQLRepository;
        private readonly UserManager<AuthUser> _userManager;
        private readonly AuthService _userService;
        private readonly CalendarService _calendarService;

        public TeamService(
            IGenericRepository<Team> teamGenereicRepository,
            TeamSQLRepository teamSQLRepository,
            CalendarSQLRepository calendarSQLRepository,
            CalendarService calendarService,
            IGenericRepository<Calendar> calendarGenericSQLRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AuthUser> userManager,
             AuthService userService
            )
        {
            this._calendarService = calendarService;
            _teamGenericRepository = teamGenereicRepository;
            _calendarGenericSQLRepository = calendarGenericSQLRepository;
            _teamSQLRepository = teamSQLRepository;
            _calendarSQLRepository = calendarSQLRepository;
            _userManager = userManager;
            this._userService = userService;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamGenericRepository.GetAllAsync();
        }

        public async Task<Team> GetTeamByNormalizedName(string teamNormalizedName)
        {
            return await _teamSQLRepository.GetTeamByNormalizedName(teamNormalizedName);
        }

        public async Task<TeamResponse> GetTeamById(Guid id)
        {
            Team team = await _teamSQLRepository.GetTeamById(id);
            return new TeamResponse(team);
        }

        public async Task<List<Team>> GetUserTeams(Guid userId)
        {
            return await _teamSQLRepository.GetUserTeams(userId);
        }

        public async Task<IEnumerable<Team>> GetTeamsWithAdmins()
        {
            return await _teamSQLRepository.GetAllTeamsAsync();
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            //get logged in user, check if he is already mapped to a team, if yes boom.
            //if not accept creation.
            AuthUser user = this._userService.GetCurrentUserAsync().Result;

            Team foundTeam = this._teamSQLRepository.GetTeamByUser(user.ApplicationUser);
            if (foundTeam != null)
            {
                throw new Exception("This user already has a team assigned to it.");
            }

            foundTeam = this._teamSQLRepository.GetTeamByNormalizedName(team.NormalizedName).Result;

            if (foundTeam != null)
            {
                throw new Exception("This team name is taken");
            }

            team.NormalizeTeamName();
            team.Admins.Add(user.ApplicationUser);
            await _teamGenericRepository.AddAsync(team);
            return team;
        }

        public async Task<Team> UpdateTeamAsync(Guid id, UpdateTeamDto team)
        {
            Team foundTeam = await this._teamSQLRepository.GetTeamById(id);
            if (foundTeam == null)
            {
                throw new Exception("No team found");
            }
            foundTeam.Name = team.Name;
            foundTeam.NormalizeTeamName();

            foundTeam.Calendar.UseUnavailabilities = team.UseUnavailabilities;
            foundTeam.Calendar.UseAvailabilities = team.UseAvailabilities;
            foundTeam.Calendar.MaxWeekendDaysLodgerCount = team.MaxWeekendDaysLodgerCount;
            foundTeam.Calendar.MaxWeekDaysLodgerCount = team.MaxWeekDaysLodgerCount;
  

            await this._teamGenericRepository.UpdateAsync(foundTeam);

            return foundTeam;
        }

    }
}
