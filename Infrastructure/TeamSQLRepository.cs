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
        public DogsittingDBContext Context { get; set; }
        public MediaSQLRepository MediaRepository { get; set; }

        public TeamSQLRepository(DogsittingDBContext context, MediaSQLRepository mediaRepository)
        {
            this.Context = context;
            this.MediaRepository = mediaRepository;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await this.Context.Teams.Include(t => t.Admins).ToListAsync();
        }

        public async Task<List<Team>> GetAllTeamsWithMediaAsync()
        {
            return await this.Context.Teams.Include(t => t.TeamMedias).ThenInclude(x => x.Media).ToListAsync();
        }

        public async Task<Team> GetByIdAsync(Guid id)
        {
            return await this.Context.Teams.Include(team => team.Admins).Include(team => team.Calendar).FirstAsync(t => t.Id == id);
        }

        public Task<Team?> GetTeamByNormalizedName(string teamName)
        {
            return this.Context.Teams.FirstOrDefaultAsync(team => team.NormalizedName == teamName);
        }

        public Task<List<Team>> GetUserTeams(Guid userId)
        {
            return this.Context.Teams.Include(t => t.Admins).Where(team => team.Admins.Any(admin => admin.Id == userId)).ToListAsync();
        }

        public async Task<Object> Create(Team team)
        {
            this.Context.Teams.Add(team);
            await this.Context.SaveChangesAsync();
            return team;
        }

        public Team GetTeamByUser(ApplicationUser user)
        {
            return Context.Teams.Include(t => t.Admins).FirstOrDefault(team => team.Admins.Any(admin => admin.Id == user.Id));
        }



    }
}
