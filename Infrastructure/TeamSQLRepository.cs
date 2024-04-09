using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return await this.context.Teams.Include(t => t.Admins).ToListAsync();
        }

        public async Task<Object> GetTeamById(Guid id)
        {
            return await this.context.Teams.FirstOrDefaultAsync(team => team.Id == id);
            //return await this.context.Teams.Where(team => team.id == id).ToListAsync();
        }

        public async Task<Object> Create(Team team)
        {
            this.context.Teams.Add(team);
            await this.context.SaveChangesAsync();
            return team;
        }

        public List<Team> GetTeamByUserId()
        {
            //return this.context.Teams.Include(t => t.Admin).ToList();
            return [];
        }


    }
}
