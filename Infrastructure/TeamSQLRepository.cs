using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.Infrastructure
{
    public class TeamSQLRepository
    {
        public DogsittingDBContext _context { get; set; }
        public MediaSQLRepository _mediaRepository { get; set; }

        public TeamSQLRepository(DogsittingDBContext context, MediaSQLRepository mediaRepository)
        {
            this._context = context;
            this._mediaRepository = mediaRepository;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {

            //return await this.context.Teams.ToListAsync();
            return await this._context.Teams.Include(t => t.Admins).ToListAsync();
        }

        public async Task<Team> GetTeamById(Guid id)
        {
            return await this._context.Teams.Include(team => team.Admins).Include(team => team.Calendar).FirstAsync(t => t.Id == id);
            //return await this.context.Teams.Where(team => team.id == id).ToListAsync();
        }

        public Task<Team> GetTeamByNormalizedName(string teamName)
        {
            return this._context.Teams.FirstAsync(team => team.NormalizedName == teamName);
        }

        public Task<List<Team>> GetUserTeams(Guid userId)
        {
            return this._context.Teams.Include(t => t.Admins).Where(team => team.Admins.Any(admin => admin.Id == userId)).ToListAsync();
        }

        public async Task<Object> Create(Team team)
        {
            this._context.Teams.Add(team);
            await this._context.SaveChangesAsync();
            return team;
        }

        public Team GetTeamByUser(ApplicationUser user)
        {
            return _context.Teams.Include(t => t.Admins).FirstOrDefault(team => team.Admins.Any(admin => admin.Id == user.Id));
        }

        //public async Task LinkMediaAsync(Guid teamId, Media media)
        //{
        //    Team? team = await _context.Teams.FindAsync(teamId);
        //    if (team != null)
        //    {
        //        media.TeamId = teamId;
        //        await _mediaRepository.AddMediaAsync(media);
        //    }
        //}


    }
}
