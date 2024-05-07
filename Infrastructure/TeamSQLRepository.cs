using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogsitting_backend.Infrastructure
{
    public class TeamSQLRepository
    {
        public DogsittingDBContext context { get; set; }

        public TeamSQLRepository(DogsittingDBContext context)
        {
            this.context = context;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {

            //return await this.context.Teams.ToListAsync();
            return await this.context.Teams.Include(t => t.Admins).ToListAsync();
        }

        public async Task<Team> GetTeamById(Guid id)
        {
            return await this.context.Teams.FindAsync(id);
            //return await this.context.Teams.Where(team => team.id == id).ToListAsync();
        }

        public Task<Team> GetTeamByNormalizedName(string teamName)
        {
            return this.context.Teams.FirstAsync(team => team.NormalizedName == teamName);
        }

        public Task<List<Team>> GetUserTeams(Guid userId)
        {
            return this.context.Teams.Include(t => t.Admins).Where(team => team.Admins.Any(admin => admin.Id == userId)).ToListAsync();
        }

        public async Task<Object> Create(Team team)
        {
            this.context.Teams.Add(team);
            await this.context.SaveChangesAsync();
            return team;
        }

        public Team GetTeamByUser(ApplicationUser user)
        {
            return context.Teams.Include(t => t.Admins).FirstOrDefault(team => team.Admins.Any(admin => admin.Id == user.Id));
        }


    }
}
