using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Domain.media;
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
        private readonly TeamSQLRepository _teamSQLRepository;
        private readonly MediaSQLRepository _mediaSQLRepository;
        private readonly AuthService _userService;

        public TeamService(
            IGenericRepository<Team> teamGenereicRepository,
            TeamSQLRepository teamSQLRepository,
            MediaSQLRepository mediaSQLRepository,
             AuthService userService
            )
        {
            this._mediaSQLRepository = mediaSQLRepository;
            _teamGenericRepository = teamGenereicRepository;
            _teamSQLRepository = teamSQLRepository;
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
            Team team = await _teamSQLRepository.GetByIdAsync(id);
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

        public async Task<IEnumerable<TeamResponse>> GetTeamsWithMedias()
        {
            List<TeamResponse> teamResponses = [];
            List<Team> teams = await _teamSQLRepository.GetAllTeamsWithMediaAsync();
            foreach (Team team in teams)
            {
                List<TeamMediaResponse> teamMediaResponses = await this.GetTeamMedias(team.Id);
                teamResponses.Add(new TeamResponse(team, teamMediaResponses));
            }
            return teamResponses;
        }

        public async Task<Team> CreateTeamAsync(CreateTeamDto team)
        {
            //get logged in user, check if he is already mapped to a team, if yes boom.
            //if not accept creation.
            Team newTeam = new(team);
            AuthUser user = this._userService.GetCurrentUserAsync().Result;

            Team foundTeam = this._teamSQLRepository.GetTeamByUser(user.ApplicationUser);
            if (foundTeam != null)
            {
                throw new Exception("This user already has a team assigned to it.");
            }

            foundTeam = this._teamSQLRepository.GetTeamByNormalizedName(newTeam.NormalizedName).Result;

            if (foundTeam != null)
            {
                throw new Exception("This team name is taken");
            }

            newTeam.NormalizeTeamName();
            newTeam.Admins.Add(user.ApplicationUser);
            await _teamGenericRepository.AddAsync(newTeam);
            return newTeam;
        }

        public async Task<Team> UpdateTeamAsync(Guid id, UpdateTeamDto team)
        {
            Team foundTeam = await this._teamSQLRepository.GetByIdAsync(id);
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

        public async Task<List<TeamMediaResponse>> GetTeamMedias(Guid teamId)
        {
            List<TeamMedia> teamMedias = await this._mediaSQLRepository.GetTeamMedias(teamId);
            return teamMedias.Select(x => new TeamMediaResponse(x.Media, x.Position)).ToList();
        }

        public async Task<List<MediaResponse>> UpdateTeamMedia(Guid teamId, List<(IFormFile file, int position)> filePositionPairs)
        {
            Team team = await this._teamSQLRepository.GetByIdAsync(teamId);
            if (team == null)
            {
                throw new Exception("Reservation not found.");
            }
            List<TeamMedia> exisitngMedias = await this._mediaSQLRepository.GetTeamMedias(teamId);


            Dictionary<int, TeamMedia> existingImagesDict = exisitngMedias.ToDictionary(img => img.Position);

            foreach (var filePositionPair in filePositionPairs)
            {
                Media newMedia = new(filePositionPair.file);
                int position = filePositionPair.position;

                // Replace existing image if there's one at this position, or add new one
                if (existingImagesDict.TryGetValue(position, out TeamMedia existingImage))
                {
                    await this._mediaSQLRepository.DeleteTeamMediaAsync(teamId, existingImage.MediaId);
                    await this._mediaSQLRepository.AddTeamMedia(teamId, newMedia, position);
                }
                else
                {
                    await this._mediaSQLRepository.AddTeamMedia(teamId, newMedia, position);
                }
            }
            return [];
        }
    }
}
